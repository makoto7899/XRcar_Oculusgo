using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading;
using MiniJSON;
using UnityEngine.UI;


public class WebSocketManager : MonoBehaviour
{

    WebSocket ws;
    public string uri = "ws://192.168.11.20:1880/testws";
    private GameObject objectTextPanel;
    private GameObject objectiPhone;
    private GameObject objectTank;


    // Use this for initialization
    void Start()
    {
        objectTextPanel = GameObject.FindGameObjectWithTag("TextPanel");
        objectiPhone = GameObject.FindGameObjectWithTag("iPhone");
        objectTank = GameObject.FindGameObjectWithTag("Tank");
        //objectiPhone = GameObject.FindGameObjectWithTag("MainCamera");

        objectTextPanel.SendMessage("connectingWebsocket", uri);
        Debug.Log(uri);
        
        var context = SynchronizationContext.Current;
        Connect();

        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("WeSocket Receive message: " + e.Data);
            context.Post(state =>
            {
                var json = Json.Deserialize(state.ToString()) as Dictionary<string, object>;

                if ((string)json["key"] == "iPhone")
                {
                    float x = float.Parse((string)json["x"]);
                    float y = float.Parse((string)json["y"]);
                    float z = float.Parse((string)json["z"]);
                    float roty = float.Parse((string)json["roty"]);

                    float[] position_and_rot = { x, y, z, roty };

                    // 小数点二位で切り捨て
                    x = Mathf.Ceil(x * 10) / 10 ;
                    y = Mathf.Ceil(y * 10) / 10;
                    z = Mathf.Ceil(z * 10) / 10;
                    roty = Mathf.Ceil(roty * 10) / 10;


                    objectiPhone.SendMessage("locateAndRotateCursor", position_and_rot);
                    objectTank.SendMessage("locateAndRotateCursor", position_and_rot);
                    objectTextPanel.SendMessage("receiveMessage", ("x:" + x.ToString() + " y:" + y.ToString() + " z:" + z.ToString()));
                    Debug.Log(state.ToString());
                }


            }, e.Data);
        };

        ws.OnError += (sender, e) => {
            Debug.Log("error: " + e.ToString());
        };

    }

    public void Connect()
    {
        Debug.Log("attempt to connect");
        ws = new WebSocket(uri);
        ws.ConnectAsync();
    }

    public void Disconnect()
    {
        ws.Close();
    }

    public void SendCommand(string msg)
    {

        var context = SynchronizationContext.Current;

        context.Post(state =>
        {
            if (ws != null && ws.IsAlive)
            {
                ws.Send(msg);
            }
        }, msg);
    }

    // Update is called once per frame
    void Update()
    {


    }
}
