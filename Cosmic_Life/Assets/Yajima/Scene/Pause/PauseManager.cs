using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    // ポーズUI
    [SerializeField]
    private GameObject m_PauseUI;
    // ボタンUI配列
    [SerializeField]
    private PushButton[] m_Buttons;
    // ポーズボタン用UI
    [SerializeField]
    private GameObject m_PauseButtonUI;

    // 表示を切り替えているか？
    private bool m_IsDrawUIs = false;
    // ポーズを閉じることが可能か？
    private bool m_IsClose = true;
    // 自身のインスタンス
    private static PauseManager instance;
    // 押されたボタン
    private PushButton m_Button;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsDrawUIs == true)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                m_IsDrawUIs = false;
                m_PauseButtonUI.SetActive(true);
                // ボタン処理を初期化
                m_Button.Init();
            }
        }
    }

    // 初期化
    public void Init()
    {
        // 全ボタンの初期化
        for (int i = 0; i != m_Buttons.Length; ++i)
        {
            m_Buttons[i].Init();
        }
        // UIを表示する
        m_PauseButtonUI.SetActive(true);
    }


    // 表示するUIを変更します
    public void ChangeDrawUI(PushButton button)
    {
        m_IsDrawUIs = true;
        m_Button = button;
        m_PauseButtonUI.SetActive(false);
    }

    // ポーズ画面をアクティブ状態にするか？
    public void UIActive(bool isActive)
    {
        m_PauseUI.SetActive(isActive);
    }

    // ポーズを閉じることが可能かの設定
    public bool IsClose
    {
        get { return m_IsClose; }
        set { m_IsClose = value; }
    }

    // インスタンスの取得を行います
    public static PauseManager GetInstance()
    {
        // インスタンスが無かった生成する
        if (instance == null)
        {
            instance = (PauseManager)FindObjectOfType(typeof(PauseManager));
            // インスタンスが無かった場合、ログの表示
            if (instance == null) Debug.LogError(" PauseManager Instance Error");
        }
        return instance;
    }
}
