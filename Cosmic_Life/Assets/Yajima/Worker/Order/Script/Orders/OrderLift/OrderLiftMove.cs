﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを持っている
public class OrderLiftMove : MonoBehaviour {

    // 持ち上げているオブジェクト
    private GameObject m_LiftObject;
    // 持ち上げたオブジェクトの初期角度
    private Vector3 m_InitAngle;

    //   // Use this for initialization
    //   void Start () {

    //}

    //   // Update is called once per frame
    //   void Update()
    //   {

    //   }

    public void CheckLiftObject(GameObject obj)
    {
        // 持ち上げているオブジェクトを確かめる
        var liftObj = obj.transform.Find("LiftObject");
        // もし何も持っていなければ、返す
        if (liftObj.childCount == 0)
        {
            //print("何も持っていません");
            // 命令失敗
            //FaildOrder(obj);
            return;
        }

        // 違うオブジェクトの場合
        if (m_LiftObject != liftObj.GetChild(0).gameObject)
        {
            m_LiftObject = liftObj.GetChild(0).gameObject;
            m_InitAngle = m_LiftObject.transform.eulerAngles;
            // 持ち上げているオブジェクトの衝突判定を設定する
            GameObject collider = m_LiftObject.transform.Find("Collider").gameObject;
        }

        //// Tweenの移動
        //m_LiftObject.transform.DOMove(m_StopPoint.position, m_AttackTime);

        // 持ち上げているオブジェクトの衝突判定をオンにする
        //m_Collider.SetActive(true);
    }

    // 持ち上げているオブジェクトの取得
    public GameObject GetLiftObject() { return m_LiftObject; }
}
