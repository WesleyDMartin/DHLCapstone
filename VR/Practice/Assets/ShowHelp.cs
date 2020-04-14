using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowHelp : MonoBehaviour, IPointerClickHandler
{
    private RawImage help;
    private RawImage helpImage;
    bool isActive = false;
    CultureManager _cultureHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActive)
        {
            if (_cultureHandler.buttonsHidden)
            {
                _cultureHandler.ToggleButtons();
            }
            helpImage.gameObject.SetActive(false);
        }
        else
        {
            if (!_cultureHandler.buttonsHidden)
            {
                _cultureHandler.ToggleButtons();
            }
            helpImage.gameObject.SetActive(true);
        }
        isActive = !isActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cultureHandler = GameObject.FindObjectOfType<CultureManager>();
        help = GameObject.Find("Help").GetComponent<RawImage>();
        helpImage = GameObject.Find("HelpImage").GetComponent<RawImage>();
        helpImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
