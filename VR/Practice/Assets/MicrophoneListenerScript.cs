using Assets.Scripts;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
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
    private VideoPlayer videoPlayer;
    private VideoClip[] videoClips;

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
        videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();

        button = GameObject.Find("Button").GetComponent<Button>();

        button.onClick.AddListener(ClickHandler);
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        goAudioSource = transform.GetComponent<AudioSource>();
        handler = transform.GetComponent<MicrophoneHandler>();
    }

    // Update is called once per frame
    private void Update()
    {
        //OVRInput.Update();

        //if (OVRInput.Get(OVRInput.Button.One))
        //{
        //    UnityEngine.Debug.Log("Button Pressed");
        //    if (!handler.IsRecording)
        //    {
        //        StartRecording();
        //    }
        //}
        //else
        //{
        //    if (handler.IsRecording)
        //    {
        //        StopRecording();
        //    }
        //}
        if (PythonHandler.ReadyToRead)
        {
            bubbleText.text = PythonHandler.text;

            switch (PythonHandler.text)
            {
                case "What are some popular tourist destinations in your country?":
                case "What is your country's most famous bridge?":
                    videoPlayer.clip = Resources.Load<VideoClip>("LondonBridge") as VideoClip;
                    videoPlayer.Play();
                    break;
                default:
                    videoPlayer.clip = Resources.Load<VideoClip>("default") as VideoClip;
                    videoPlayer.Play();
                    break;
            }
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
                var text = PythonHandler.GetQuestionFromText(src);

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