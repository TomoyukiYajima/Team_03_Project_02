using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class NamingManager : MonoBehaviour
{

    // 名前入力欄
    [SerializeField]
    private NamingBox m_NamingBox;
    // 名前決定ボックス
    [SerializeField]
    private GameObject m_NameCheckBox;
    // カーソル
    [SerializeField]
    private GameObject m_Cursor;

    // 名前決定中か？
    private bool m_IsDecision = false;

    // Use this for initialization
    void Start()
    {
        //m_Field.te
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            // テキストが無かったら、返す
            if (m_NamingBox.GetTextCount() == 0) return;
            // 表示を切り替え
            m_IsDecision = !m_IsDecision;
            // 名前決定ボックス
            m_NameCheckBox.SetActive(m_IsDecision);
            // コマンドボックス

        }
    }

    //public void OnGUI()
    //{
    //    Rect rect = new Rect(10, 10, 300, 50);
    //    m_Text = GUI.TextField(rect, "brew", 16);
    //}
}
