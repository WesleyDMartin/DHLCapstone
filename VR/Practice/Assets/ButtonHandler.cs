using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    Button _toggleCultures;
    CultureManager _cultureHandler;

    //Use this for initialization  
    private void Start()
    {
        _toggleCultures = GameObject.Find("ToggleCultures").GetComponent<Button>();
        _cultureHandler = GameObject.FindObjectOfType<CultureManager>();
    }

    private void Update()
    {
    }

    public void toggleCultures()
    {
        _cultureHandler.ToggleButtons();    
    }

    public void SetText()
    {
    }
}