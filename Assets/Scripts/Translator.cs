using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;


public class Translator : MonoBehaviour {

    public static Translator instance;
    private string translationTextEndpoint = "https://api.microsofttranslator.com/v2/http.svc/Translate?";
    private string translationTokenEndpoint = "https://centralus.api.cognitive.microsoft.com/sts/v1.0/issueToken?";
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

    private const string authorizationKey = "1922157948de4017b038fb590def9679";
    private string authorizationToken;

    public enum Languages { en, ru, es}
    public Languages from;
    public Languages to;

   private float waitTime = 800.0f;

   

    void Awake () {
        instance = this;
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
