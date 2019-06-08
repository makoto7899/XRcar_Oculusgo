using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    [SerializeField]
    private GameObject camera;
    int a;

    [SerializeField]
    private GameObject menu_panel; // メニューパネル
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.Back))
        {
            ///戻るボタン押されたら
            
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            ////タッチパッド押されたら
        }
        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
        {
            ///タッチパッド触れられたら

            //pink.SetActive(false);
        }

        Vector2 pt = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad); ///タッチパッドの位置

        if (pt.x < -0.5 && -0.5 < pt.y && pt.y < 0.5)///左側？
        {
            camera.transform.position += new Vector3(-0.1f * Time.deltaTime, 0, 0);

        }
        if (pt.x > 0.5 && -0.5 < pt.y && pt.y < 0.5)///右側？
        {
            camera.transform.position += new Vector3(0.1f * Time.deltaTime, 0, 0);

        }
        if (pt.y < -0.5 && -0.5 < pt.x && pt.x < 0.5)///下側？
        {
            camera.transform.position += new Vector3(0, 0, -0.1f * Time.deltaTime);

        }
        if (pt.y > 0.5 && -0.5 < pt.x && pt.x < 0.5)///上側？
        {
            camera.transform.position += new Vector3(0, 0, 0.1f * Time.deltaTime);

        }
    }
}
