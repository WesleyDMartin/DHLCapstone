using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopulateCulturesDropdown : MonoBehaviour, IPointerClickHandler
{
    private Button button;
    private Text buttonText;
    private AudioSource source;
    private WWW audioLoader;
    private AudioClip clip;
    public static List<string> cultures;
    public static string SelectedCulture = "";
    private ICulturesAndQuestionsApi api;
    private bool playing = false;
    Dropdown dropdown;
    bool downProcessing = false;
    bool upProcessing = false;
    bool submitProcessing = false;
    private NarratorHandler narrator;

    // Start is called before the first frame update
    void Awake()
    {
        dropdown = GameObject.Find("CultureDropdown").GetComponent<Dropdown>();
        source = transform.GetComponent<AudioSource>();
        button = GameObject.Find("Button").GetComponent<Button>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        narrator = GameObject.FindObjectOfType<NarratorHandler>();
        api = new CulturesAndQuestionsApi();
        dropdown.ClearOptions();
        cultures = api.GetCultures();
        dropdown.AddOptions(cultures);
        CommandInterpreter.SetCulture(cultures[0]);
        SelectedCulture = cultures[0];
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test1");
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        CommandInterpreter.ReadyToSpeak = false;
        buttonText.text = cultures[change.value];
        SelectedCulture = cultures[change.value];
        StartCoroutine(narrator.Speak("Great! Lets start learning about the " +
            cultures[change.value] + " culture. Go ahead and ask a question," +
            "and we will see if we can answer it for you!", 2, 7));
        Thread _thread = new Thread(() =>
        {
            CommandInterpreter.SetCulture(cultures[change.value]);
        });
        _thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();


        //if (!submitProcessing && OVRInput.Get(OVRInput.Button.Two))
        //{
        //    submitProcessing = true;
        //    DropdownValueChanged(dropdown);
        //}

        //if (!upProcessing && OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        //{ 
        //    if (dropdown.value > 0)
        //    {
        //        dropdown.Set(dropdown.value - 1);
        //    }
        //}

        //if (!OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        //{
        //    upProcessing = false;
        //}

        //if (!downProcessing && OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        //{
        //    if (dropdown.value < cultures.Count - 1)
        //    {
        //        dropdown.Set(dropdown.value + 1);
        //    }
        //}

        //if (!OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        //{
        //    downProcessing = false;
        //}



        if (source.clip != null && source.clip.isReadyToPlay && CommandInterpreter.ReadyToSpeak)
        {
            Debug.Log("playing");
            source.Play();
            CommandInterpreter.ReadyToSpeak = false;
            playing = true;
        }

        if (playing && !source.isPlaying)
        {
            source.clip = null;
            playing = false;
            submitProcessing = false;
        }

        if (CommandInterpreter.ReadyToSpeak)
        {
            audioLoader = new WWW("C:\\Users\\User\\AppData\\LocalLow\\DefaultCompany\\Practice\\out.wav");
            while (!audioLoader.isDone)
            {
                Debug.Log("uploading");
            }

            Debug.Log("1");

            source.clip = audioLoader.GetAudioClip(false, false, AudioType.WAV);
        }
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();

    }
}
