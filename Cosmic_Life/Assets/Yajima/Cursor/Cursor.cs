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

    // Use this for initialization
    void Start () {
        m_ButtonCount = 0;
        // カーソルの座標をボタンの座標にする
        this.transform.position = m_Buttones[m_ButtonCount].transform.position;
        m_Buttones[m_ButtonCount].Flash();

        // 扉の更新
        //if (m_ChangeScene == null) m_ChangeScene = GameObject.Find("Door").GetComponent<ChangeScene>();
    }
	
	// Update is called once per frame
	void Update () {
        // ドアが動いている間は動かさないようにする
        if (m_ChangeScene != null)
        {
            //print("カーソル君ドア見つけた");
            if (m_ChangeScene.gameObject.activeSelf && !m_ChangeScene.IsOpenDoor()) return;
        }
        else
        {
            var door = GameObject.Find("Door");
            if (door != null) m_ChangeScene = door.GetComponent<ChangeScene>();
        }

        if (!m_IsMove) return;

        //if(m_ChangeScene.)
        //if(m_ChangeScene != null)
        //{
        //    print("カーソル君ドア見つけた");
        //    if (m_ChangeScene.gameObject.activeSelf && !m_ChangeScene.IsOpenDoor())
        //    {
        //        print("カーソル君動けない");
        //        return;
        //    }
        //}
        // Input.GetButtonDown("OK")
        //if (Input.GetButtonDown("OK")) m_Buttones[m_ButtonCount].onClick;

        // カーソルが指定座標に辿り着いていない場合は返す
        if (Vector3.Distance(m_Buttones[m_ButtonCount].transform.position, this.transform.position) > 0.1f) return;
        else
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.75f) m_Buttones[m_ButtonCount].Flash();
        }

        if (Input.GetButtonDown("OK"))
        {
            //audio.Play();
            //audio.PlayOneShot(m_DicisionSE);
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

        // ツインを利用した移動
        // カーソルが指定座標に辿り着いる場合は返す。
        if (m_ButtonCount == prevCount) return;
        SoundManager.Instance.PlaySe("SE_Select");
        this.transform.DOMove(m_Buttones[m_ButtonCount].transform.position, m_MoveTime);
        // 発光処理処理の停止
        m_Buttones[prevCount].StopFlash();
    }
}
