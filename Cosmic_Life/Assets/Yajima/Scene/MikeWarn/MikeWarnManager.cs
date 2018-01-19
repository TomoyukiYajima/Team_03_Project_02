using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeWarnManager : MonoBehaviour {

    // キャンバスオブジェクト
    [SerializeField]
    private Canvas m_Canvas;
    // シーン遷移までの時間
    [SerializeField]
    private float m_ChangeTime = 2.0f;
    // 現在の時間
    private float m_Timer;
    // シーン遷移するか？
    private bool m_IsEnd = false;

    // Use this for initialization
    void Start () {
        // カメラの設定
        var obj = GameObject.Find("GameManagerCamera");
        var camera = obj.GetComponent<Camera>();
        m_Canvas.worldCamera = camera;
    }

    // Update is called once per frame
    void Update () {
        m_Timer += Time.deltaTime;

        // ボタンが押されたらシーン遷移する
        if (Input.GetButtonDown("OK")) m_Timer = m_ChangeTime;

        // 一定時間経過したら遷移する
        if (m_Timer < m_ChangeTime || m_IsEnd) return;
        // シーン遷移処理
        SceneMgr.Instance.SceneTransitionSimple(SceneType.Title);
        m_IsEnd = true;
    }
}
