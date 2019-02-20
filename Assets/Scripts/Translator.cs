using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;


public class Translator : MonoBehaviour {

    public static Translator instance;
    private string translationTextEndpoint = "https://api.cognitive.microsofttranslator.com/v2/http.svc/Translate?";
    private string translationTokenEndpoint = "https://centralus.api.cognitive.microsoft.com/sts/v1.0/issueToken?";
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

    private const string authorizationKey = "1922157948de4017b038fb590def9679";
    private string authorizationToken;

    public enum Languages { en, ru, es}
    public Languages from = Languages.en;
    public Languages to = Languages.es;
  
	// Use this for initialization
	void Start () {
        //may need to call this routine every 10 min
        StartCoroutine("GetTokenCoroutine", authorizationKey);
		
	}
	
	// Update is called once per frame
	void Awake () {
        instance = this;
	}

    private IEnumerator GetTokenCoroutine(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Authorization key not set.");
        }
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(translationTokenEndpoint, string.Empty))
        {
            unityWebRequest.SetRequestHeader("Ocp-Apim-Subscription-Key", key);
           // unityWebRequest.SetRequestHeader("Subscription-Key", key);
            yield return unityWebRequest.SendWebRequest();
           

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                yield return null;
            }
            else
            {
                authorizationToken = unityWebRequest.downloadHandler.text;
            }
        }

        MicrophoneManager.instance.StartCapturingAudio();
    }


    public IEnumerator TranslateWithUnityNetworking(string text)
    {
        string queryString = string.Concat("text=", Uri.EscapeDataString(text), "&from=", from, "&to", to);

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(translationTextEndpoint + queryString))
        {
            unityWebRequest.SetRequestHeader("Authorization", "Bearer" + authorizationToken);
            unityWebRequest.SetRequestHeader("Accept-Language", "application/xml");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                yield return null;
            }

            string result = XElement.Parse(unityWebRequest.downloadHandler.text).Value;
            Results.instance.SetTranslationResult(result);
            MicrophoneManager.instance.StopCapturingAudio();

        }
            
    }
}
