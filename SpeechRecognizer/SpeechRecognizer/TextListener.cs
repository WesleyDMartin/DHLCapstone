﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace SpeechRecognizer
{

    public class AsynchronousSocketListener
    {
        private static string FILE_PATH = "C:\\Users\\User\\AppData\\LocalLow\\DefaultCompany\\Practice\\test.wav";
        private static string DOT_NET = "dotnet";
        private static string GOOGLE = "google";

        private static string PLATFORM = "dotnet";

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener()
        {
        }

        public static void StartListening(string platform)
        {
            PLATFORM = platform;
            Console.WriteLine($"Started using {platform}");
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    content = content.Substring(0, content.Length - 5);
                    Send(handler, PLATFORM == GOOGLE ? GoogleSpeechToText(content) : DotNetSpeechToText(content));
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static string DotNetSpeechToText(string culture)
        {
            var recognizedQuestion = string.Empty;
            using (
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US")))
            {

                Console.WriteLine(FILE_PATH);
                recognizer.SetInputToWaveFile(FILE_PATH);
                recognizer.LoadGrammar(new DictationGrammar());
                RecognitionResult result = recognizer.Recognize();
                Console.WriteLine($"Recognized Question: {result.Text}");
                recognizedQuestion = PythonHandler.GetQuestionFromText(culture, result.Text);
                Console.WriteLine($" Interpreted Answer: {result.Text}");
                // Echo the data back to the client.  
            }
            return recognizedQuestion;
        }


        public static string GoogleSpeechToText(string culture)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Console.WriteLine($"----Starting {sw.Elapsed}");
            var speech = SpeechClient.Create();


            Console.WriteLine($"----Time to create client {sw.Elapsed}");

            var response = speech.Recognize(new RecognitionConfig
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = 44100,
                    LanguageCode = "en"
                },
                RecognitionAudio.FromFile(FILE_PATH));

            Console.WriteLine($"----Time to get text {sw.Elapsed}");
            var text = "";
            foreach (var result in response.Results)
                foreach (var alternative in result.Alternatives)
                {
                    text += alternative.Transcript;
                    Console.WriteLine(alternative.Transcript);
                }
            Console.WriteLine(text);
            Console.WriteLine($"----Time to close {sw.Elapsed}");
            return PythonHandler.GetQuestionFromText(culture, text);
        }
    }
}
