using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;
using UnityEngine.UI;


public class Translator : MonoBehaviour {

    public static Translator instance;
    private string translationTextEndpoint = "https://api.microsofttranslator.com/v2/http.svc/Translate?";
    private string translationTokenEndpoint = "https://centralus.api.cognitive.microsoft.com/sts/v1.0/issueToken?";
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

    private const string authorizationKey = "1922157948de4017b038fb590def9679";
    private string authorizationToken;

    public enum Languages { en, it, ru, es, fr}
    private Languages from = Languages.en;
    private Languages to ;
    public Dropdown dropdown;
   private float waitTime = 800.0f;
    private string languageSelected;



    void Awake () {
        instance = this;
	}

    private void GetLanguage()
    {
     
      
        languageSelected = dropdown.options[dropdown.value].text;

        if (languageSelected == "Spanish") to = Languages.es;
        if (languageSelected == "Italian") to = Languages.it;
        if (languageSelected == "Russian") to = Languages.ru;
        if (languageSelected == "French") to = Languages.fr;
    }


    public IEnumerator TranslateWithUnityNetworking(string text, float time)
    {
        //need to only run every 8 mins
        if (time + waitTime > Time.deltaTime ) { 
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(translationTokenEndpoint, string.Empty))
        {
            unityWebRequest.SetRequestHeader("Ocp-Apim-Subscription-Key", authorizationKey);
            // unityWebRequest.SetRequestHeader("Subscription-Key", key);
            yield return unityWebRequest.SendWebRequest();


            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                yield return null;
            }
            else
            {
                authorizationToken = unityWebRequest.downloadHandler.text;
                    time = Time.deltaTime;
            }
        }
       }
        GetLanguage();
        string queryString = string.Concat("text=", Uri.EscapeDataString(text), "&from=", from, "&to=", to);

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(translationTextEndpoint + queryString))
        {
            
            unityWebRequest.SetRequestHeader("Authorization", "Bearer " +  authorizationToken);
            unityWebRequest.SetRequestHeader("Accept", "application/xml");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                yield return null;
            }

            string result = XElement.Parse(unityWebRequest.downloadHandler.text).Value;
            Results.instance.SetTranslationResult(result);
           // MicrophoneManager.instance.StopCapturingAudio();

        }
            
    }
}
