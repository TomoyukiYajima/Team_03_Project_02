using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cursor : MonoBehaviour {

    // ボタン配列
    //private Button[] m_Buttones;
    [SerializeField]
    private PushButton[] m_Buttones;
    // 左カーソル
    [SerializeField]
    private GameObject m_LeftCursor;
    // 右カーソル
    [SerializeField]
    private GameObject m_RightCursor;
    // シーン遷移オブジェクト
    [SerializeField]
    private ChangeScene m_ChangeScene;
    // 移動時間
    [SerializeField]
    private float m_MoveTime = 0.1f;

    // 現在のボタンのカウント
    private int m_ButtonCount;
    // カーソルが動くか
    private bool m_IsMove = true;
    // 現在の時間
    private float m_Timer;
    // 制御時間
    private float m_DelayTime = 1.0f;

    // Use this for initialization
    void Start () {
        m_ButtonCount = 0;
        m_Timer = 0.0f;

        // カーソルの座標をボタンの座標にする
        this.transform.position = m_Buttones[m_ButtonCount].transform.position;
        m_Buttones[m_ButtonCount].Flash();
        // カーソルの幅も合わせる
        var points = m_Buttones[m_ButtonCount].transform.Find("Points");
        m_LeftCursor.transform.DOMoveX(points.GetChild(0).position.x, 0.0f);
        m_RightCursor.transform.DOMoveX(points.GetChild(1).position.x, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        // ドアが動いている間は動かさないようにする
        if (m_ChangeScene != null)
        {
            if (m_ChangeScene.gameObject.activeSelf && !m_ChangeScene.IsOpenDoor()) return;
        }
        else
        {
            var door = GameObject.Find("Door");
            if (door != null) m_ChangeScene = door.GetComponent<ChangeScene>();
        }

        m_Timer += Time.deltaTime;
        if (m_Timer < m_DelayTime) return;

        if (!m_IsMove) return;

        // カーソルが指定座標に辿り着いていない場合は返す
        if (Vector3.Distance(m_Buttones[m_ButtonCount].transform.position, this.transform.position) > 0.1f) return;
        else
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.75f) m_Buttones[m_ButtonCount].Flash();
        }

        if (Input.GetButtonDown("OK"))
        {
            SoundManager.Instance.PlaySe("SE_Dicision");
            m_Buttones[m_ButtonCount].DownAction();
            m_IsMove = false;
            return;
        }

        // 入力の値が一定値を超えた場合、ボタンカウントを変動させる
        int prevCount = m_ButtonCount;
        if (Input.GetAxis("Vertical") > 0.75f) m_ButtonCount = Mathf.Max(m_ButtonCount - 1, 0);
        else if (Input.GetAxis("Vertical") < -0.75f)
            m_ButtonCount = Mathf.Min(m_ButtonCount + 1, m_Buttones.Length - 1);

        // カーソルが指定座標に辿り着いる場合は返す。
        if (m_ButtonCount == prevCount) return;
        SoundManager.Instance.PlaySe("SE_Select");
        // ツインを利用した移動
        this.transform.DOMove(m_Buttones[m_ButtonCount].transform.position, m_MoveTime);
        // 幅も合わせるようにする
        var points = m_Buttones[m_ButtonCount].transform.Find("Points");
        m_LeftCursor.transform.DOMoveX(points.GetChild(0).position.x, m_MoveTime);
        m_RightCursor.transform.DOMoveX(points.GetChild(1).position.x, m_MoveTime);
        // 発光処理処理の停止
        m_Buttones[prevCount].StopFlash();
    }
}
