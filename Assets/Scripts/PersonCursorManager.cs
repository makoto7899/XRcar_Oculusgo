using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCursorManager : MonoBehaviour
{
    // 初期位置、初期回転
    public float start_x = 0;
    public float start_y = 0;
    public float start_z = 0.2f;
    public float start_rotx = 0;
    public float start_roty = 0;
    public float start_rotz = 0;

    // 現在回転
    private Quaternion diriPhone;


    // 連続タップ時に呼ばれるパーティクル
    // public GameObject particle1;
    // public GameObject particle2;



    void Start()
    {
        this.gameObject.transform.position = new Vector3(start_x, start_y, start_z);
        this.gameObject.transform.rotation = Quaternion.Euler(start_rotx, start_roty, start_rotz);
        //InputManager.Instance.AddGlobalListener(gameObject);

    }


    // Update is called once per frame
    void Update()
    {


    }

    // WebSocketManagerに呼ばれる
    void locateAndRotateCursor(float[] position_and_rot)
    {
        this.gameObject.transform.position = new Vector3(start_x + position_and_rot[0], start_y + position_and_rot[1], start_z - position_and_rot[2]);

        diriPhone = Quaternion.AngleAxis(position_and_rot[3] * Mathf.Rad2Deg, new Vector3(0, -1, 0));
        this.gameObject.transform.rotation = diriPhone;


    }

}