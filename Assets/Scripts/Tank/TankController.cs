using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    public float tank_scale = 10.0f;

    // 初期位置、初期回転
    public float start_x = 0;
    public float start_y = 0;
    public float start_z = 0;
    public float start_rotx = 0;
    public float start_roty = 0;
    public float start_rotz = 0;

    // カーソル位置更新時のrotation
    private Quaternion startRotation;

    // カーソル位置更新前のiPhoneの位置
    private Quaternion previousRotation;
    private Vector3 previousPosition;

    // カーソル位置更新時の前iPhoneの位置保存用
    private Quaternion tempRotation;
    private Vector3 tempPosition;

    // Use this for initialization
    void Start () {
        this.gameObject.transform.position = new Vector3(start_x, start_y, start_z);

        startRotation = Quaternion.Euler(start_rotx, start_roty, start_rotz);
        this.gameObject.transform.rotation = startRotation;

        previousPosition = new Vector3(0, 0, 0);
        previousRotation = new Quaternion(0, 0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position += transform.forward * speed * Time.deltaTime;


    }

    // WebSocketManagerに呼ばれる
    void locateAndRotateCursor(float[] position_and_rot)
    {


        tempPosition.x = position_and_rot[0];
        tempPosition.y = position_and_rot[1];
        tempPosition.z = position_and_rot[2];

        /*
        debugPanel.text = "Holo座標カーソル原点: " + start_x.ToString() + ", " +  start_y.ToString() + ", " + start_z.ToString() + "\n" +
                          "現在のiPhoneの位置: " + position_and_rot[0].ToString() + ", " + position_and_rot[1].ToString() + ", " + position_and_rot[2].ToString() + "\n" + 
                          "再設定iPhone原点: " + previousPosition.ToString() + "\n" + 
                          "iPhoneの相対座標: " + new Vector3((position_and_rot[0] - previousPosition[0]), (position_and_rot[1] - previousPosition[1]), (position_and_rot[2] - previousPosition[2])).ToString();
        */


        //           　　　　　　　　　　　　　　　　　　       相対距離
        // カーソル位置　= Holo座標でのカーソル原点 + (現在のiPhoneの位置 - 再設定iPhone原点)
        this.gameObject.GetComponent<Rigidbody>().MovePosition(new Vector3(start_x + (position_and_rot[0] - previousPosition[0]), start_y, start_z - (position_and_rot[2] - previousPosition[2])) * tank_scale);
        //this.gameObject.transform.position = new Vector3(start_x + (position_and_rot[0] - previousPosition[0]), start_y + (position_and_rot[1] - previousPosition[1]), start_z - (position_and_rot[2] - previousPosition[2]));

        tempRotation = Quaternion.AngleAxis(position_and_rot[3] * Mathf.Rad2Deg, new Vector3(0, -1, 0));
        this.gameObject.GetComponent<Rigidbody>().MoveRotation(tempRotation);
        //this.gameObject.transform.rotation = tempRotation;


    }
}
