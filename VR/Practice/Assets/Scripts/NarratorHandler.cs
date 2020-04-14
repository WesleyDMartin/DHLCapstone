using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NarratorHandler : MonoBehaviour
{
    private Text bubbleText;
    private AudioSource source;
    private WWW audioLoader;
    private AudioClip clip;
    private RawImage bubble;
    private List<string> cultures;
    private ICulturesAndQuestionsApi api;
    private bool playing = false;
    public delegate void DonePlayingEventHandler();
    public event DonePlayingEventHandler DonePlayingEvent;
    private RawImage guide;


    private int audioLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Narrator").GetComponent<AudioSource>();
        guide = GameObject.Find("Guide").GetComponent<RawImage>();
        api = new CulturesAndQuestionsApi();
        bubble = GameObject.Find("ShowText").GetComponent<RawImage>();
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        bubbleText.transform.localScale = new Vector3(0, 0, 0);
        bubble.transform.localScale = new Vector3(0, 0, 0);
        guide.transform.localScale = new Vector3(0, 0, 0);

        if (CultureManager.cultures.Count == 1)
        {
            StopSpeaking();
            StartCoroutine(Speak($"Welcome to the Digital Human Library virtual learning platform. " +
                $"Today we are going to learn about the {CultureManager.cultures[0]} culture. Go ahead and ask a question," +
                "and we will see if we can answer it for you!", 3, 6));
        }
        else
        {
            StopSpeaking();
            StartCoroutine(Speak("Welcome to the Digital Human Library virtual learning platform. " +
                "Please select a culture about which you would like to learn", 3, 6));
        }
    }

    public void StopSpeaking()
    {
        CommandInterpreter.ReadyToSpeak = false;
        StopAllCoroutines();
        source.Stop();
        source.clip = null;
        File.Delete("C:\\Users\\User\\AppData\\LocalLow\\DefaultCompany\\Practice\\out.wav");
        bubbleText.transform.localScale = new Vector3(0, 0, 0);
        bubble.transform.localScale = new Vector3(0, 0, 0);
        guide.transform.localScale = new Vector3(0, 0, 0);
        playing = false;
        HideBubble();
    }


    //Ouput the new value of the Dropdown into Text
    public IEnumerator Speak(string text, int speakDelay = 0, int bubbleDelay = 5)
    {
        yield return new WaitForSeconds(speakDelay);

        ShowBubble(text);
        Thread _thread = new Thread(() =>
        {
            CommandInterpreter.TextToSpeech(text);
        });
        _thread.Start();
    }

    //Ouput the new value of the Dropdown into Text
    void ShowBubble(string text)
    {
        bubbleText.text = text;
        bubbleText.transform.localScale = new Vector3(1,1,1);
        bubble.transform.localScale = new Vector3(1,1,1);
        guide.transform.localScale = new Vector3(1,1,1);
    }

    void HideBubble()
    {
        bubbleText.text = "";
        bubbleText.transform.localScale = new Vector3(0,0,0);
        bubble.transform.localScale = new Vector3(0, 0, 0);
        guide.transform.localScale = new Vector3(0, 0, 0);
    }

    IEnumerator HideBubble(int duration)
    {
        yield return new WaitForSeconds(duration);
        HideBubble();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (source.clip != null && source.clip.isReadyToPlay && CommandInterpreter.ReadyToSpeak)
            {
                CommandInterpreter.ReadyToSpeak = false;
                StartCoroutine(DonePlaying(source.clip.length));
                Debug.Log(source.clip.length);
                Debug.Log("playing");
                source.Play();
                playing = true;
            }

            if (playing && !source.isPlaying)
            {
                StartCoroutine(HideBubble(3));
                source.clip = null;
                playing = false;
            }

            if (CommandInterpreter.ReadyToSpeak)
            {
                audioLoader = new WWW("C:\\Users\\User\\AppData\\LocalLow\\DefaultCompany\\Practice\\out.wav");
                while (!audioLoader.isDone)
                {
                }


                source.clip = audioLoader.GetAudioClip(false, false, AudioType.WAV);
                audioLength = (int)source.clip.length;
            }
        }
        catch(Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }

    }


    public IEnumerator DonePlaying(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("Invoked");
        DonePlayingEvent?.Invoke();
    }
}
