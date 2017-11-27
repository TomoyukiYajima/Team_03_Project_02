﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour
{
    [SerializeField, Range(0f, 10f), Tooltip("レイ距離")] private float m_rayDist;
    [SerializeField, Range(0f, 360f), Tooltip("プレイヤー視野角度")] private float m_rayAngle;
    //[SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private Sprite m_aimBlack;
    [SerializeField] private Sprite m_aimEffect;

    private GameObject m_colliderObj;   // 当たったオブジェクトを格納する関数
    private Transform m_rayPos;         // レイ開始位置
    private Transform m_player;         // プレイヤー
    private Transform m_rayCenter;
    private Vector3 m_rayDir;           // レイ方向

    private Image m_aim;

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        // プレイヤーのレイ開始座標
        m_rayPos = m_player.FindChild("LookPoint").transform;
        m_rayCenter = m_player.FindChild("HeadLook").transform;

        m_aim = GameObject.Find("PlayerCanvas").transform.FindChild("AimBase").GetComponent<Image>();

        m_rayDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = m_rayCenter.position - transform.position;
        float dirY = dir.y;
        dir.Normalize();
        dir.y = dirY;

        float angle = Vector3.Angle(m_player.forward, dir);

        // プレイヤー視野角度内かつプレイヤーの後ろにいるときしか更新しない
        //if (angle > m_rayAngle)
        //{
        //    m_rayDir = transform.forward;
        //}
        //else
        //{
        //    m_rayDir = m_player.forward;
        //}
        m_rayDir = dir;

        //// y軸更新
        //m_rayDir.y = transform.forward.y;

        // レイを飛ばす
        Ray ray = new Ray(m_rayPos.position, m_rayDir * m_rayDist);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_rayDist);
        // デバッグ描画
        Debug.DrawRay(m_rayPos.position, m_rayDir * m_rayDist, new Color(1f, 0, 0));

        // なにも当たってないとき
        if (hitInfo.collider == null)
        {
            EndFlash(null);
            return;
        }
        // 格納されたオブジェクトとあたっているオブジェクトが同一ではないとき
        else if (m_colliderObj != hitInfo.collider.gameObject)
        {
            EndFlash(hitInfo.collider.gameObject);
            //StartFlash(new Color(0.5f, 0.5f, 0.5f), 1.0f);
        }

        /*
        if (hitInfo.collider == null) return;

        if (hitInfo.collider.tag == "Camera" || hitInfo.collider.tag == "Robot")
        {
            if (Input.GetKeyDown(KeyCode.J))
            {

                //m_cameraManager.SwitchCamera(hitInfo.collider.gameObject.transform, 0.5f);
            }
        }
        */
    }

    private void StartFlash(Color color, float duration)
    {
        // オブジェクトがStageObjectコンポーネントを実装しているかをチェック
        StageObject material = null;
        //StageObject material = m_colliderObj.GetComponent<StageObject>();
        if ((material = m_colliderObj.GetComponent<StageObject>()) == null) return;
        // 点滅開始
        material.FlashEmission(color, duration);
        m_aim.sprite = m_aimEffect;
    }

    private void EndFlash(GameObject obj)
    {
        m_aim.sprite = m_aimBlack;
        // 格納されたオブジェクトが空ではないとき
        if (m_colliderObj != null)
        {
            // オブジェクトがStageObjectコンポーネントを実装しているかをチェック
            StageObject material = null;
            if ((material = m_colliderObj.GetComponent<StageObject>()) != null)
            {
                // 点滅終了
                material.EndFlashEmission();
            }
        }
        // 格納オブジェクトを更新
        m_colliderObj = obj;
        // 更新されたオブジェクトをロボットに送る
        SendObject(m_colliderObj);
        if (m_colliderObj == null) return;
        StartFlash(new Color(0.5f, 0.5f, 0.5f), 1.0f);
    }

    private void SendObject(GameObject obj)
    {
        // シーンにいる全部のロボットを入れる
        var robotList = GameObject.FindGameObjectsWithTag("Robot");

        // 全部のロボットにオーダーを出す
        foreach (var robot in robotList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(robot))
            {
                Debug.Log("IOrderEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (receive, y) => receive.setObject(obj));
        }

    }
}
