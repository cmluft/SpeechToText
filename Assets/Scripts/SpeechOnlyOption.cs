using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;

[RequireComponent(typeof(Button))]

public class SpeechOnlyOption : MonoBehaviour {

    [HideInInspector]
    private Button Button;

    private void OnEnable()
    {
        this.Button = GetComponent<Button>();

        if (this.Button != null && this.Button.name == "SpeechOnly")
        {
            this.Button.OnButtonClicked += Button_OnButtonClicked;
        }
    }
    private void Button_OnButtonClicked(GameObject obj)
    {
        TranslationResults.instance.SpeechOnly = true;
       
    }
}
