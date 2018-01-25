using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageChangeUI : MonoBehaviour {
    // 切り替え用UI
    [SerializeField]
    private GameObject[] m_PauseUIs;
    // ページテキスト
    [SerializeField]
    private Text m_PageText;
    // 移動制限時間
    [SerializeField]
    private float m_StopTime = 1.0f;

    // 現在の表示カウント
    private int m_PauseCount;
    // 現在の時間
    private float m_Timer;

    // Use this for initialization
    void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        m_Timer = Mathf.Max(m_Timer - Time.deltaTime, 0.0f);
        // タイマが0でなければ返す
        if (m_Timer > 0.0f) return;

        // トリガーが押されたら、表示UIを切り替える
        if (Input.GetButtonDown("Triggrt_Left"))
        {
            Change(-1);
        }

        if (Input.GetButtonDown("Triggrt_Right"))
        {
            Change(1);
        }
    }

    // 初期化
    public void Init()
    {
        m_PauseUIs[m_PauseCount].SetActive(false);
        // 最初のUIを表示する
        m_PauseCount = 0;
        m_PauseUIs[m_PauseCount].SetActive(true);
        m_PageText.text = "1";
    }

    // 切り替え
    private void Change(int count)
    {
        // 移動制限時間を入れる
        m_Timer = m_StopTime;
        // 切り替え
        // 前回表示していたUIを非表示にする
        m_PauseUIs[m_PauseCount].SetActive(false);

        // カウントの加算
        m_PauseCount += count;
        if (m_PauseCount < 0) m_PauseCount = m_PauseUIs.Length - 1;
        else if (m_PauseCount > m_PauseUIs.Length - 1) m_PauseCount = 0;
        // ページテキストの更新
        m_PageText.text = (m_PauseCount + 1).ToString();

        // 表示する
        m_PauseUIs[m_PauseCount].SetActive(true);
    }
}
