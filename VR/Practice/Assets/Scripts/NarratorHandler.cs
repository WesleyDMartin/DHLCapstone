using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
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


    private int audioLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Narrator").GetComponent<AudioSource>();
        api = new CulturesAndQuestionsApi();
        bubble = GameObject.Find("ShowText").GetComponent<RawImage>();
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        bubbleText.transform.localScale = new Vector3(0, 0, 0);
        bubble.transform.localScale = new Vector3(0, 0, 0);

        if (CultureManager.cultures.Count == 1)
        {
            StartCoroutine(Speak($"Welcome to the Digital Human Library virtual learning platform. " +
                $"Today we are going to learn about the {CultureManager.cultures[0]} culture. Go ahead and ask a question," +
                "and we will see if we can answer it for you!", 3, 6));
        }
        else
        {
            StartCoroutine(Speak("Welcome to the Digital Human Library virtual learning platform. " +
                "Please select a culture about which you would like to learn", 3, 6));
        }
    }



    //Ouput the new value of the Dropdown into Text
    public IEnumerator Speak(string text, int speakDelay = 0, int bubbleDelay = 0)
    {
        yield return new WaitForSeconds(speakDelay);

        ShowBubble(text, bubbleDelay + speakDelay);
        Thread _thread = new Thread(() =>
        {
            CommandInterpreter.TextToSpeech(text);
        });
        _thread.Start();
    }

    //Ouput the new value of the Dropdown into Text
    void ShowBubble(string text, int duration)
    {
        bubbleText.text = text;
        bubbleText.transform.localScale = new Vector3(1,1,1);
        bubble.transform.localScale = new Vector3(1,1,1);
        StartCoroutine(HideBubble(duration));
    }

    IEnumerator HideBubble(int duration)
    {
        yield return new WaitForSeconds(duration);
        bubbleText.text = "";
        bubbleText.transform.localScale = new Vector3(0,0,0);
        bubble.transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (source.clip != null && source.clip.isReadyToPlay && CommandInterpreter.ReadyToSpeak)
        {
            StartCoroutine(DonePlaying(source.clip.length));
            Debug.Log(source.clip.length);
            Debug.Log("playing");
            source.Play();
            playing = true;
        }

        if (playing && !source.isPlaying)
        {
            source.clip = null;
            playing = false;
            CommandInterpreter.ReadyToSpeak = false;
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


    public IEnumerator DonePlaying(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("Invoked");
        DonePlayingEvent?.Invoke();
    }
}
