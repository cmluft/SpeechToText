using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;
using UnityEngine.UI;
using HoloToolkit.Unity.Buttons;

public class Translator
{

    public static Translator instance = new Translator();
    private string translationTextEndpoint = "https://api.microsofttranslator.com/v2/http.svc/Translate?";
    private string translationTokenEndpoint = "https://centralus.api.cognitive.microsoft.com/sts/v1.0/issueToken?";
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

    private const string authorizationKey = "1922157948de4017b038fb590def9679";
    private string authorizationToken;


    public string TranslateFromLanguage = "en";
    public string TranslateToLanguage = "es";

    private float waitTime = 8 * 60;
    
    void Awake()
    {
        instance = this;

    }


    float? lastTranslateTime;

    public IEnumerator TranslateWithUnityNetworking(string text)
    {
        float time = Time.time;

        //need to only run every 8 mins
        if (lastTranslateTime == null || (time - lastTranslateTime.Value) > waitTime)
        {
          

            using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(translationTokenEndpoint, string.Empty))
            {
                unityWebRequest.SetRequestHeader("Ocp-Apim-Subscription-Key", authorizationKey);
                
                yield return unityWebRequest.SendWebRequest();


                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    yield return null;
                }
                else
                {
                    authorizationToken = unityWebRequest.downloadHandler.text;
                    lastTranslateTime = time;
                }
            }
        }
        string queryString = string.Concat("text=", Uri.EscapeDataString(text), "&from=", TranslateFromLanguage, "&to=", TranslateToLanguage);

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(translationTextEndpoint + queryString))
        {

            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + authorizationToken);
            unityWebRequest.SetRequestHeader("Accept", "application/xml");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                yield return null;
            }

            string result = XElement.Parse(unityWebRequest.downloadHandler.text).Value;
            TranslationResults.instance.SetTranslationResult(result);
            
           

        }

    }
}
