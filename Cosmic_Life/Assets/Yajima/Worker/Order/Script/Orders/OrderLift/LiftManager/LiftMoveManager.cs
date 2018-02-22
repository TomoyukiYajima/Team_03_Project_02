﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LiftMoveManager : MonoBehaviour {

    // 持ち上げているオブジェクト
    private GameObject m_LiftObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 持ち上げているオブジェクトの取得
    public GameObject GetLiftObject() { return m_LiftObject; }

    // オブジェクトを持ち上げているかのチェック
    public bool CheckLiftObject(GameObject obj)
    {
        // 持ち上げているオブジェクトを確かめる
        var liftObj = obj.transform.Find("LiftObject");
        // もし何も持っていなければ、返す
        if (liftObj.childCount == 0)
        {
            print("何も持っていません");
            // 空にする
            m_LiftObject = null;
            return false;
        }

        // 同一のオブジェクトの場合は返す
        if (m_LiftObject == liftObj.GetChild(0).gameObject) return true;
        // オブジェクトを入れる
        m_LiftObject = liftObj.GetChild(0).gameObject;
        return true;
    }

    // 持ち上げているオブジェクトの親子関係を解除する
    public void ReleaseObject(GameObject obj, GameObject colliders)
    {
        if (m_LiftObject == null) return;

        var stageObj = m_LiftObject.transform.GetComponent<StageObject>();
        //colliders.transform.parent = m_LiftObject.transform;
        colliders.SetActive(false);
        colliders.transform.parent = m_LiftObject.transform;
        //colliders.transform.localPosition = Vector3.zero;

        // 剛体
        var body = stageObj.GetComponent<Rigidbody>();
        // キネマティックをオフにする
        body.isKinematic = false;
        // 重力をオンにする
        body.useGravity = true;
        // ナビメッシュオブジェクトをアクティブ状態に変更
        var nav = m_LiftObject.transform.GetComponent<NavMeshObstacle>();
        nav.enabled = true;
        // ステージオブジェクトの親を初期化する
        stageObj.InitParent();
        stageObj.InitCollider();

        // IKの初期化
        obj.GetComponent<Worker>().InitIK();
    }
}
