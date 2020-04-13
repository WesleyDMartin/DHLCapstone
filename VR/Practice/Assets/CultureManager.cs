using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CultureManager : MonoBehaviour
{
    private Button button;
    private Button _toggleCultures;
    private Text buttonText;
    private AudioSource source;
    private RawImage _pointer;
    private WWW audioLoader;
    private AudioClip clip;
    public static List<string> cultures;
    public static string SelectedCulture = "";
    private ICulturesAndQuestionsApi api;
    private bool playing = false;
    bool downProcessing = false;
    bool upProcessing = false;
    bool submitProcessing = false;
    private NarratorHandler narrator;
    private bool buttonsHidden = false;
    private ServiceHandler serviceHandler;

    public Button prefab;

    private List<Button> buttons;

    // Start is called before the first frame update
    void Awake()
    {
        buttons = new List<Button>();
        serviceHandler = GameObject.FindObjectOfType<ServiceHandler>();
        ServiceHandler.ServicesReady += InitialCulture;
        prefab = Resources.Load<Button>("CultureButton");
        _toggleCultures = GameObject.Find("ToggleCultures").GetComponent<Button>();
        source = transform.GetComponent<AudioSource>();
        _pointer = GameObject.Find("Pointer").GetComponent<RawImage>();
        button = GameObject.Find("RecordButton").GetComponent<Button>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        narrator = GameObject.FindObjectOfType<NarratorHandler>();
        api = new CulturesAndQuestionsApi();
        
        cultures = api.GetCultures();

        int i = 0;
        cultures.ForEach(c =>
        {
            MakeButton(i++);
        });

        SelectedCulture = cultures[0];
    }


    private void InitialCulture(object sender, EventArgs args)
    {
        CommandInterpreter.SetCulture(cultures[0]);
    }

    public void MakeButton(int verticalNumber)
    {    
        // Instantiate (clone) the prefab    
        Button _button = (Button)Instantiate(prefab);

        var panel = GameObject.Find("ControlPanel").GetComponent<Canvas>();
        _button.transform.SetParent(panel.transform);
        _button.transform.localScale = Vector3.one;
        _button.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _button.transform.localPosition = Vector3.zero;
        _button.onClick.AddListener(delegate { UpdateCulture(cultures[verticalNumber]); });
        var tran = _button.transform.GetComponent<RectTransform>();
        var x = 48F + ((int)(verticalNumber / 4) * 270);
        var y = 204F - verticalNumber * 60 + ((int)(verticalNumber / 4) * 60 * 4);
        tran.anchoredPosition = new Vector3(x, y, 0);
        _button.GetComponentInChildren<Text>().text = cultures[verticalNumber];
        buttons.Add(_button);
    }

    public void IndicateSelectedCulture(int i)
    {
        var x = 176.1F + ((int)(i / 4) * 270);
        var y = 450.9F - i * 60 + ((int)(i / 4) * 60 * 4);
        var tran = _pointer.transform.GetComponent<RectTransform>();
        tran.anchoredPosition = new Vector3(x, y, 0);
    }

    public void ToggleButtons()
    {
        if (!buttonsHidden)
        {
            buttons.ForEach(b =>
            {
                b.gameObject.SetActive(false);
            });
            _pointer.gameObject.SetActive(false);
            _toggleCultures.GetComponentInChildren<Text>().text = "Show Cultures";
        }
        else
        {
            buttons.ForEach(b =>
            {
                b.gameObject.SetActive(true);
            });
            _pointer.gameObject.SetActive(true);
            _toggleCultures.GetComponentInChildren<Text>().text = "Hide Cultures";
        }
        buttonsHidden = !buttonsHidden;
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        UpdateCulture(cultures[change.value]);
    }

    void UpdateCulture(string culture)
    {
        IndicateSelectedCulture(culture);
        CommandInterpreter.ReadyToSpeak = false;
        buttonText.text = culture;
        SelectedCulture = culture;
        StartCoroutine(narrator.Speak("Great! Lets start learning about the " +
            culture + " culture. Go ahead and ask a question," +
            "and we will see if we can answer it for you!", 2, 7));
        Thread _thread = new Thread(() =>
        {
            CommandInterpreter.SetCulture(culture);
        });
        _thread.Start();
    }

    public void IndicateSelectedCulture(string culture)
    {
        var i = 0;

        foreach(var c in cultures)
        {
            if (c == culture)
            {
                IndicateSelectedCulture(i);
                break;
            }
            i++;
        }
        return;
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
            }
            

            source.clip = audioLoader.GetAudioClip(false, false, AudioType.WAV);
        }
    }
}
