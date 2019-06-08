using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextManager : MonoBehaviour {
    private Text targetText;

    // Use this for initialization
    void Start () {
        this.targetText = this.GetComponent<Text>();
        //this.targetText.text = "Start";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void connectingWebsocket(string uri) {
        this.targetText = this.GetComponent<Text>();
        this.targetText.text = "Connect to ..." + uri;
        this.targetText.text = "Player 1";
    }

    void receiveMessage(string message) {
        this.targetText = this.GetComponent<Text>();
        this.targetText.text = message;
    }
}
