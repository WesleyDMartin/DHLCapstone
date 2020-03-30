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
    private void Start()
    {
        threeSixtyPlayer = GameObject.Find("Youtube360Player").GetComponent<YoutubePlayer>();
        standardPlayer = GameObject.Find("YoutubeAdvanced").GetComponent<YoutubePlayer>();
        button = GameObject.Find("Button").GetComponent<Button>();
        
        button.onClick.AddListener(ClickHandler);
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        goAudioSource = transform.GetComponent<AudioSource>();
        handler = transform.GetComponent<MicrophoneHandler>();
        narrator = GameObject.FindObjectOfType<NarratorHandler>();
        standardScale = standardPlayer.transform.localScale;
        threeSixtyScale = threeSixtyPlayer.transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Button.One))
        {
            UnityEngine.Debug.Log("Button Pressed");
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
            CommandInterpreter.ReadyToRead = false;
            bubbleText.text = CommandInterpreter.Response.text_answer;


            if (CommandInterpreter.Response.answer == null || CommandInterpreter.Response.answer == string.Empty)
            {
                threeSixtyPlayer.Stop();
            }
            else
            {
                StartCoroutine(narrator.Speak(CommandInterpreter.Response.text_answer));

                switch (CommandInterpreter.Response.videotype)
                {
                    case "threesixty":
                        standardPlayer.Stop();
                        standardPlayer.transform.localScale = new Vector3(0, 0, 0);
                        threeSixtyPlayer.transform.localScale = threeSixtyScale;
                        threeSixtyPlayer.Play(CommandInterpreter.Response.answer);
                        break;

                    case "standard":
                        threeSixtyPlayer.Stop();
                        threeSixtyPlayer.transform.localScale = new Vector3(0, 0, 0);
                        standardPlayer.transform.localScale = standardScale;
                        standardPlayer.Play(CommandInterpreter.Response.answer);
                        break;

                    default:
                        standardPlayer.Stop();
                        standardPlayer.transform.localScale = new Vector3(0, 0, 0);
                        threeSixtyPlayer.transform.localScale = threeSixtyScale;
                        threeSixtyPlayer.Play(CommandInterpreter.Response.answer);
                        break;
                }
            }

            //switch (CommandInterpreter.text)
            //{
            //    case "What are some popular tourist destinations in your country?":
            //    case "What is your country's most famous bridge?":
            //        videoPlayer.clip = Resources.Load<VideoClip>("LondonBridge") as VideoClip;
            //        videoPlayer.Play();
            //        break;
            //    default:
            //        videoPlayer.clip = Resources.Load<VideoClip>("default") as VideoClip;
            //        videoPlayer.Play();
            //        break;
            //}
        }
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();

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
            var text = CommandInterpreter.GetQuestionFromText(PopulateCulturesDropdown.SelectedCulture);

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
}