using Assets.Scripts;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using YoutubeLight;
using UnityEngine.Video;
using System.Diagnostics;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneListenerScript : MonoBehaviour
{
    private Text bubbleText;
    private Button button;
    private Text buttonText;
    private AudioSource goAudioSource;
    private MicrophoneHandler handler;
    private YoutubePlayer threeSixtyPlayer;
    private YoutubePlayer standardPlayer;
    private YoutubePlayer backgroundPlayer;
    private VideoClip[] videoClips;
    private NarratorHandler narrator;
    private Vector3 standardScale;
    private Vector3 threeSixtyScale;

    private int loudCount;
    private int maxFreq;

    //A boolean that flags whether there's a connected microphone  
    private bool micConnected;

    //The maximum and minimum available recording frequencies  
    private int minFreq;
    private float nextActionTime;
    public float period = .2f;

    private int quietCount;

    // Start is called before the first frame update
    private void Awake()
    {
        threeSixtyPlayer = GameObject.Find("Youtube360Player").GetComponent<YoutubePlayer>();
        standardPlayer = GameObject.Find("YoutubeAdvanced").GetComponent<YoutubePlayer>();
        backgroundPlayer = GameObject.Find("BackgroundPlayer").GetComponent<YoutubePlayer>();
        button = GameObject.Find("RecordButton").GetComponent<Button>();
        
        button.onClick.AddListener(ClickHandler);
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        goAudioSource = transform.GetComponent<AudioSource>();
        handler = transform.GetComponent<MicrophoneHandler>();
        narrator = GameObject.FindObjectOfType<NarratorHandler>();
        narrator.DonePlayingEvent += new NarratorHandler.DonePlayingEventHandler(raiseVolume);
        standardPlayer.gameObject.SetActive(false);
        threeSixtyPlayer.gameObject.SetActive(false);
        backgroundPlayer.OnVideoStarted.AddListener(backgroundPlayer.Pause);
        backgroundPlayer.Play(@"https://youtu.be/OaH_I-c0UbY?t=112");

        standardPlayer.OnVideoFinished.AddListener(delegate { OnFinished(false); });
        threeSixtyPlayer.OnVideoFinished.AddListener(delegate { OnFinished(true); });
    }


    private void OnFinished(bool is360)
    {
        if (is360)
        {
            backgroundPlayer.gameObject.SetActive(true);
            threeSixtyPlayer.gameObject.SetActive(false);
            threeSixtyPlayer.Stop();
        }
        else
        {
            standardPlayer.Stop();
            standardPlayer.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    private void Update()
    {

        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            backgroundPlayer.gameObject.SetActive(true);
            standardPlayer.Stop();
            threeSixtyPlayer.Stop();
            standardPlayer.gameObject.SetActive(false);
            threeSixtyPlayer.gameObject.SetActive(false);
        }

        if (OVRInput.Get(OVRInput.Button.One))
        {
            if (!handler.IsRecording)
            {
                StartRecording();
            }
        }
        else
        {
            if (handler.IsRecording)
            {
                StopRecording();
            }
        }
        if (CommandInterpreter.ReadyToRead)
        {
            switch (CommandInterpreter.Error)
            {
                
                case ErrorType.NO_WORDS:
                    StartCoroutine(narrator.Speak("Sorry, I didn't quite catch that, " +
                        "please try asking your question again"));
                    break;
                case ErrorType.NO_QUESTION:
                    StartCoroutine(narrator.Speak("Sorry, I don't know how to answer that!"));
                    break;
                case ErrorType.NO_ERROR:

                    bool isSpokenAnswer = false;
                    CommandInterpreter.ReadyToRead = false;


                    if (CommandInterpreter.Response == null || CommandInterpreter.Response.answer == string.Empty)
                    {
                        threeSixtyPlayer.Stop();
                        standardPlayer.Stop();
                    }
                    else
                    {
                        if (CommandInterpreter.Response.text_answer != null && CommandInterpreter.Response.text_answer != string.Empty)
                        {
                            StartCoroutine(narrator.Speak(CommandInterpreter.Response.text_answer, 3, 8));
                            threeSixtyPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)0.1;
                            standardPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)0.1;
                        }
                        else
                        {
                            threeSixtyPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)1;
                            standardPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)1;
                        }

                        switch (CommandInterpreter.Response.videotype)
                        {
                            case "threesixty":
                                standardPlayer.Stop();
                                standardPlayer.gameObject.SetActive(false);
                                threeSixtyPlayer.gameObject.SetActive(true);
                                backgroundPlayer.gameObject.SetActive(false);
                                threeSixtyPlayer.Play(CommandInterpreter.Response.answer);
                                break;

                            case "standard":
                                threeSixtyPlayer.Stop();
                                backgroundPlayer.gameObject.SetActive(true);
                                threeSixtyPlayer.gameObject.SetActive(false);
                                standardPlayer.gameObject.SetActive(true);
                                standardPlayer.Play(CommandInterpreter.Response.answer);
                                break;

                            default:
                                standardPlayer.Stop();
                                standardPlayer.gameObject.SetActive(false);
                                threeSixtyPlayer.gameObject.SetActive(true);
                                threeSixtyPlayer.Play(CommandInterpreter.Response.answer);
                                break;
                        }
                    }
                    break;
            }
        }
        CommandInterpreter.Error = ErrorType.NO_ERROR;
    }
    

    private void ClickHandler()
    {
        if (handler.IsRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        buttonText.text = "Recording";
        threeSixtyPlayer.Stop();
        standardPlayer.Stop();
        threeSixtyPlayer.gameObject.SetActive(false);
        standardPlayer.gameObject.SetActive(false);
        handler.StartRecording();
    }

    private void StopRecording()
    {
        buttonText.text = "Not Recording";
        var sw = new Stopwatch();

        sw.Start();
        try
        {
            UnityEngine.Debug.Log($"Starting {sw.Elapsed}");
            var src = handler.StopRecording();

            //UnityEngine.Debug.Log($"Time to Stop recording {sw.Elapsed}");
            //var text = VoiceApiHandler.GetTextFromAudio(src);
            //bubbleText.text = text;
            var _thread = new Thread(() => {
                UnityEngine.Debug.Log($"Time to get text from audio {sw.Elapsed}");
            var text = CommandInterpreter.GetQuestionFromText(CultureManager.SelectedCulture);

                UnityEngine.Debug.Log($"Time to get question from text {sw.Elapsed}");
            });
            _thread.Start();


        }
        catch (Exception e)
        {
            if (!Directory.Exists("c:\\tmp"))
            {
                Directory.CreateDirectory("c:\\tmp");
            }
            File.AppendAllText("c:\\tmp\\debug.txt", e.Message);
        }
        UnityEngine.Debug.Log($"Ending {sw.Elapsed}");
        sw.Stop();
        goAudioSource.Play();
    }

    private void raiseVolume()
    {
        UnityEngine.Debug.Log("Called");
        threeSixtyPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)1;
        standardPlayer.videoPlayer.GetTargetAudioSource(0).volume = (float)1;
    }
}