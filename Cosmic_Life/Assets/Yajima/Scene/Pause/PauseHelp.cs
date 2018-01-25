using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHelp : MonoBehaviour {
    // 現在の時間
    private float m_Timer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //m_Timer = Mathf.Max(m_Timer - Time.deltaTime, 0.0f);
        //// タイマが0でなければ返す
        //if (m_Timer > 0.0f) return;

        // 
    }

    // 初期化
    public void Init()
    {
        //m_Timer = 0.0f;

        // マネージャの初期化処理を呼ぶ
        PauseManager.GetInstance().Init();
    }

    //// 切り替え
    //private void Change(int count)
    //{
    //    // 移動制限時間を入れる
    //    m_Timer = m_StopTime;
    //    // 切り替え
    //    // 前回表示していたUIを非表示にする
    //    m_PauseUIs[m_PauseCount].SetActive(false);

    //    // カウントの加算
    //    m_PauseCount += count;
    //    if (m_PauseCount < 0) m_PauseCount = m_PauseUIs.Length - 1;
    //    else if (m_PauseCount > m_PauseUIs.Length - 1) m_PauseCount = 0;
    //    // ページテキストの更新
    //    m_PageText.text = (m_PauseCount + 1).ToString();

    //    // 表示する
    //    m_PauseUIs[m_PauseCount].SetActive(true);
    //}
}
