using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MicrophoneManager : MonoBehaviour {

    public static MicrophoneManager instance;
    private AudioSource audioSource;
    private bool microphoneDetected;
    private DictationRecognizer dictationRecognizer;
    private float startTime; 

	// Use this for initialization
	void Awake () {
        instance = this;
	}
	
	// Update is called once per frame
	void Start () {
		
          if(Microphone.devices.Length > 0)
        {
            audioSource = GetComponent<AudioSource>();
            microphoneDetected = true;
            Results.instance.SetMicrophoneStatus("Working");
             StartCapturingAudio();

        }
        else
        {
           // Results.instance.SetMicrophoneStatus("No Microphone Detected");
        }
	}

    public void StartCapturingAudio()
    {
      // dictationRecognizer.Stop();
        if (microphoneDetected )
        {
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationReconizer_DictationResult;
            dictationRecognizer.Start();
           // Results.instance.SetMicrophoneStatus("Capturing...");
        }

    }
    public void StopCapturingAudio()
    {
      //  Results.instance.SetMicrophoneStatus("Mic sleeping");
        Microphone.End(null);
        dictationRecognizer.DictationResult -= DictationReconizer_DictationResult;
        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }

    private void DictationReconizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Results.instance.SetDicationResult(text);
     
        StartCoroutine(Translator.instance.TranslateWithUnityNetworking(text));
    }
}
