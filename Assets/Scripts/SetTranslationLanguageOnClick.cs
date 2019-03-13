using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class SetTranslationLanguageOnClick : MonoBehaviour
{
    [SerializeField]
    public string LanguageCode = "es";

    [HideInInspector]
    private Button Button;

    private void OnEnable()
    {
        this.Button = GetComponent<Button>();

        if (this.Button != null)
        {
            this.Button.OnButtonClicked += Button_OnButtonClicked;
        }
    }

    private void Button_OnButtonClicked(GameObject obj)
    {
        Translator.instance.TranslateToLanguage = LanguageCode;
    }
}
