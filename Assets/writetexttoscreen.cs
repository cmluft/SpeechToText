using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class writetexttoscreen : MonoBehaviour {

    // Use this for initialization

    public TextMesh text;
            
	void Start () {
        text.text = "test";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void WriteText()
    {
        text.text = "Hi there idiot";
    }
}
