using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System.Threading;

public class LaserPointerOculus : MonoBehaviour {

    [SerializeField]
    private Transform _RightHandAnchor;

    [SerializeField]
    private Transform _LeftHandAnchor;

    [SerializeField]
    private Transform _CenterEyeAnchor;

    [SerializeField]
    private float _MaxDistance = 100.0f;

    [SerializeField]
    private LineRenderer _LaserPointerRenderer;

    [SerializeField]
    private Object bulletPrefab;

    [SerializeField]
    private float bulletPower = 500f;

    private GameObject objectWebSocket;

    private Transform Pointer
    {
        get
        {
            // 現在アクティブなコントローラーを取得
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ目の間からビームが出る
            return _CenterEyeAnchor;
        }
    }

    private void Shot(Transform pointer)
    {
        // 弾のインスタンスを作成
        var bulletInstance = GameObject.Instantiate(bulletPrefab, pointer.position, pointer.rotation) as GameObject;
        // 弾の発射
        bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * bulletPower);
        // 5秒後に自動で消えるように
        Destroy(bulletInstance, 3f);
        // json作成
        var context = SynchronizationContext.Current;
        context.Post(state =>
        {
            string itemJson = "{ \"key\": \"Oculus\", \"rotation\": \" " + state + "\"}";
            objectWebSocket.SendMessage("SendCommand", itemJson);
        }, pointer.rotation);
    }

    private void Start()
    {
        objectWebSocket = GameObject.FindGameObjectWithTag("WebSocketManager");
    }

    void Update()
    {
        var pointer = Pointer;
        if (pointer == null || _LaserPointerRenderer == null)
        {
            return;
        }
        // コントローラー位置からRayを飛ばす
        Ray pointerRay = new Ray(pointer.position, pointer.forward);

        // レーザーの起点
        _LaserPointerRenderer.SetPosition(0, pointerRay.origin);

        RaycastHit hitInfo;
        if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance))
        {
            // Rayがヒットしたらそこまで
            _LaserPointerRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            // Rayがヒットしなかったら向いている方向にMaxDistance伸ばす
            _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _MaxDistance);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Shot(pointer);
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("shot!");
            Shot(pointer);

        }
#endif

    }
       
}
