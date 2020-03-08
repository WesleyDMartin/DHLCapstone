using UnityEngine;
using UnityEngine.UI;

public class RecordButtonHandler : MonoBehaviour
{
    //A handle to the attached AudioSource  
    public static bool recording = false;
    private RawImage bubble;

    private Text bubbleText;
    private int maxFreq;

    //A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;

    //The maximum and minimum available recording frequencies  
    private int minFreq;

    private Text txt;

    //Use this for initialization  
    private void Start()
    {
        bubbleText = GameObject.Find("TextBubble").GetComponent<Text>();
        bubble = GameObject.Find("ShowText").GetComponent<RawImage>();
        txt = transform.Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
    }


    public void SetText()
    {
    }
}