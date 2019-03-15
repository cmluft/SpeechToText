using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MicrophoneManager : MonoBehaviour {

    public static MicrophoneManager instance;
    private AudioSource audioSource;
    private bool microphoneDetected;
    private DictationRecognizer dictationRecognizer;


	// Use this for initialization
	void Awake () {
        instance = this;
        
    }


    private void OnApplicationQuit()
    {
        StopCapturingAudio();
    }

    // Update is called once per frame
    void Start () {
		
          if(Microphone.devices.Length > 0)
        {
            audioSource = GetComponent<AudioSource>();
            microphoneDetected = true;
            TranslationResults.instance.SetMicrophoneStatus("Working");
            StartCapturingAudio();

        }
        else
        {
            TranslationResults.instance.SetMicrophoneStatus("No Microphone Detected");
        }
	}
  
    public void StartCapturingAudio()
    {
      
        if (microphoneDetected )
        {
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationReconizer_DictationResult;
           
           
            TranslationResults.instance.SetMicrophoneStatus("Capturing...");
            dictationRecognizer.Start();
        }

    }
    public void StopCapturingAudio()
    {
        TranslationResults.instance.SetMicrophoneStatus("Mic sleeping");
        Microphone.End(null);
        dictationRecognizer.DictationResult -= DictationReconizer_DictationResult;
        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }

    private void DictationReconizer_DictationResult(string text, ConfidenceLevel confidence)
    {
       
        TranslationResults.instance.SetDicationResult(text);
        TranslationResults.instance.SetTranslationResult("") ;
        if (!TranslationResults.instance.SpeechOnly)
       
        {
            StartCoroutine(Translator.instance.TranslateWithUnityNetworking(text));
        }
       

       
    }

 }
