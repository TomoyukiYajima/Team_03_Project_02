using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonCursor : MonoBehaviour {

    // テキストボタン配列
    [SerializeField]
    private GameObject m_TextButtons;
    // ネームテキスト
    [SerializeField]
    private GameObject m_NameTexts;

    // 入力のパワー
    [SerializeField]
    private float m_InputPower = 0.75f;
    // 移動時間
    [SerializeField]
    private float m_MoveTime = 1.0f;

    // Xの位置
    private int m_CursorRow = 0;
    // Yの位置
    private int m_CursorColumn = 0;
    // 入力箇所
    private int m_TextCount = 0;
    //// Xの現在位置
    //private int m_CursorCurrentRow = 0;
    //// Yの現在位置
    //private int m_CursorCurrentColumn = 0;

    private bool m_IsMove = true;

	// Use this for initialization
	void Start () {
        this.transform.position = m_TextButtons.transform.GetChild(0).GetChild(0).position;
	}
	
	// Update is called once per frame
	void Update () {

        if (!m_IsMove) return;

        // m_Buttones[m_ButtonCount].Flash();
        // カーソルが指定座標に辿り着いていない場合は返す
        if (Vector3.Distance(m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).position, this.transform.position) > 0.1f) return;
        else
        {
            //if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.75f) m_Buttones[m_ButtonCount].Flash();
        }

        if (Input.GetButtonDown("OK") && m_TextCount < m_NameTexts.transform.childCount)
        {
            //audio.Play();
            //audio.PlayOneShot(m_DicisionSE);
            //m_Buttones[m_ButtonCount].ChangeScene();
            //m_IsMove = false;
            var text = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).GetChild(0).GetComponent<Text>();
            if (text.text == "" || text.text == "　") return;
            var nameText = m_NameTexts.transform.GetChild(m_TextCount).GetComponent<Text>();
            nameText.text = text.text;
            m_TextCount++;
            return;
        }
        //else if (Input.GetButtonDown("OK") && m_TextCount < m_NameTexts.transform.childCount)
        //{
        //    return;
        //}

        // 入力の値が一定値を超えた場合、ボタンカウントを変動させる
        int column = m_CursorColumn;
        if (Input.GetAxis("Vertical") > m_InputPower) m_CursorColumn = Mathf.Max(m_CursorColumn - 1, 0);
        else if (Input.GetAxis("Vertical") < -m_InputPower) m_CursorColumn = Mathf.Min(m_CursorColumn + 1, m_TextButtons.transform.GetChild(0).childCount - 1);
        int row = m_CursorRow;
        if (Input.GetAxis("Horizontal") > m_InputPower) m_CursorRow = Mathf.Max(m_CursorRow - 1, 0);
        else if (Input.GetAxis("Horizontal") < -m_InputPower) m_CursorRow = Mathf.Min(m_CursorRow + 1, m_TextButtons.transform.childCount - 1);

        // ツインを利用した移動
        // カーソルが指定座標に辿り着いる場合は返す。
        if (m_CursorColumn == column && m_CursorRow == row) return;
        //audio.PlayOneShot(m_SelectSE);
        this.transform.DOMove(m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).position, m_MoveTime);
        // m_Buttones[m_ButtonCount].Flash();
        // 発光処理処理の停止
        //m_Buttones[prevCount].StopFlash();
    }
}
