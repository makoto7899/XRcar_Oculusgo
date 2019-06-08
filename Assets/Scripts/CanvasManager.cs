using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    private GameObject _childText;
    private GameObject _childPanel;
    private Text targetText;

    // hololensの方向にパネルとテキストを向けるためにカメラ情報を設定。
    public GameObject mainCamera;

	// Use this for initialization
	void Start () {
        _childText = GameObject.Find("Text");
        _childPanel = GameObject.Find("Panel");
        targetText = _childText.GetComponent<Text>();
        // Debug.Log(_childPanel.transform.position);



    }
	
	// Update is called once per frame
	void Update () {
        //_childPanel.transform.Rotate(0f, 1f, 0f);
        //targetText.transform.Rotate(0f, 1f, 0f);

        // パネルとテキストにカメラを向いてもらう
        var dif = mainCamera.transform.position - _childPanel.transform.position;
        var rot = Quaternion.LookRotation(new Vector3(dif.x, 0, dif.z));
        _childPanel.transform.rotation = rot;
        targetText.transform.rotation = rot;
        // uGUIのオブジェクトは-z方向が正面なので、-180度回転する必要がある
        _childPanel.transform.Rotate(new Vector3(0f, -180f, 0f));
        targetText.transform.Rotate(new Vector3(0f, -180f, 0f));

    }


    void connectingWebsocket(string uri)
    {
        this.targetText.text = "Connect to ..." + uri;
        this.targetText.text = "Player 1";
    }

    void receiveMessage(string message)
    {
        this.targetText = _childText.GetComponent<Text>();
        this.targetText.text = message;
    }
}

