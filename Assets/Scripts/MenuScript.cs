using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    void Start()
    {
        AddButtonHandler("Button1", () => {
            GameObject.Find("Button1").GetComponentInChildren<Text>().text = "clicked.";
        });
        AddButtonHandler("Button2", () => {
            GameObject.Find("Button2").GetComponentInChildren<Text>().text = "clicked.";
        });

        ShowMenu(false);
    }

    private void AddButtonHandler(string buttonName, UnityEngine.Events.UnityAction func)
    {
        Button button = GameObject.Find(buttonName).GetComponent<Button>();
        button.onClick.AddListener(func);
    }

    bool _isShowMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { // OculusコントローラーのBackボタンはEscapeキー扱い
            _isShowMenu = !_isShowMenu;
            ShowMenu(_isShowMenu);
        }
    }

    private void ShowMenu(bool isShow)
    {
        _isShowMenu = isShow;

        var canvas = transform.GetChild(0);
        if (isShow)
        {
            // 自分の目の前に持ってくる(角度はすこしずらす)
            Vector3 menuLocalPos = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward * 0.2f;
            Vector3 menuWorldPos = Camera.main.transform.TransformPoint(menuLocalPos);
            transform.position = menuWorldPos;
            // こっちを向かせる(角度調整あり)
            //transform.LookAt(Camera.main.transform.position); // ……だと何故かイベントが効かなくなる
            Vector3 menuAngle = Camera.main.transform.eulerAngles;
            transform.eulerAngles = new Vector3(menuAngle.x, menuAngle.y - 30f, menuAngle.z);
            canvas.gameObject.SetActive(true);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }

    }

}