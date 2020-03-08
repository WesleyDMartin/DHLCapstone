using Google.Cloud.Speech.V1;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class VoiceApiHandler
    {
        
        public static string GetTextFromAudio(string filePath)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            UnityEngine.Debug.Log($"----Starting {sw.Elapsed}");
            var speech = SpeechClient.Create();


            UnityEngine.Debug.Log($"----Time to create client {sw.Elapsed}");

            var response = speech.Recognize(new RecognitionConfig
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = 44100,
                    LanguageCode = "en"
                },
                RecognitionAudio.FromFile(filePath));

            UnityEngine.Debug.Log($"----Time to get text {sw.Elapsed}");
            var text = "";
            foreach (var result in response.Results)
            foreach (var alternative in result.Alternatives)
            {
                text += alternative.Transcript;
                Debug.Log(alternative.Transcript);
            }

            UnityEngine.Debug.Log($"----Time to close {sw.Elapsed}");
            return text;
        }
    }

    public class VoiceRecognition
    {

        public bool StartSpeechRecognition()
        {
            bool test = StreamingMicRecognizeAsync(20, "fantastisk").Result;
            return test;
        }

        static async Task<bool> StreamingMicRecognizeAsync(int inputTime, string inputWord)
        {
            bool speechSuccess = false;
            Stopwatch timer = new Stopwatch();

            Task delay = Task.Delay(TimeSpan.FromSeconds(1));

            if (NAudio.Wave.WaveIn.DeviceCount < 1)
            {
                Console.WriteLine("No microphone!");
                return false;
            }

            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = "nb",
                        },
                        InterimResults = true,
                    }
                });


            // Compare speech with the input word, finish if they are the same and speechSuccess becomes true.
            Task compareSpeech = Task.Run(async () =>
            {
                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream
                        .Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            if (alternative.Transcript.Replace(" ", String.Empty).Equals(inputWord, StringComparison.InvariantCultureIgnoreCase))
                            {
                                speechSuccess = true;

                                return;
                            }

                        }
                    }
                }
            });

            // Read from the microphone and stream to API.
            object writeLock = new object();
            bool writeMore = true;
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            waveIn.DataAvailable +=
                (object sender, NAudio.Wave.WaveInEventArgs args) =>
                {
                    lock (writeLock)
                    {
                        if (!writeMore) return;
                        streamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };

            waveIn.StartRecording();
            timer.Start();
            //Console.WriteLine("Speak now.");

            //Delay continues as long as a match has not been found between speech and inputword or time that has passed since recording is lower than inputTime.
            while (!speechSuccess && timer.Elapsed.TotalSeconds <= inputTime)
            {
                await delay;
            }

            // Stop recording and shut down.
            waveIn.StopRecording();
            timer.Stop();

            lock (writeLock) writeMore = false;

            await streamingCall.WriteCompleteAsync();
            await compareSpeech;

            //Console.WriteLine("Finished.");
            return speechSuccess;
        }
    }
}