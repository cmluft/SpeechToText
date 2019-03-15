using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslationResults : MonoBehaviour {

    public static TranslationResults instance;

    [HideInInspector]
    public string translateResult;

    [HideInInspector]
    public string dictationResult;
    [HideInInspector]
    public string microphoneStatus;

    public TextMesh dictationText;
    public TextMesh translationText;
    public TextMesh microphoneText;
    public bool SpeechOnly = false;

	
	// Update is called once per frame
	void Awake () {
        instance = this;
	}
    public void SetMicrophoneStatus(string result)
    {
        microphoneStatus = result;
        microphoneText.text = microphoneStatus;
    }
    public void SetTranslationResult(string result)
    {
        translateResult = result;
        translationText.text = translateResult;
    }

    public void SetDicationResult(string result)
    {
        dictationResult = result;
        dictationText.text = dictationResult;
    }
}
