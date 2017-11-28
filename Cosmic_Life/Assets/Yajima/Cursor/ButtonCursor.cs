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
    //private GameObject m_NameTexts;
    private NamingBox m_NameBox;

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
    // 過去のXの位置
    private int m_PrevCursorRow = 0;
    // 過去のYの位置
    private int m_PrevCursorColumn = 0;
    //// 入力箇所
    //private int m_TextCount = 0;

    //// Xの現在位置
    //private int m_CursorCurrentRow = 0;
    //// Yの現在位置
    //private int m_CursorCurrentColumn = 0;

    private bool m_IsMove = true;

	// Use this for initialization
	void Start () {
        // ボタンの位置を調整する
        //m_CursorRow = m_TextButtons.transform.childCount - 1;
        m_PrevCursorRow = m_CursorRow;
        this.transform.position = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).position;

        // 発光
        var curchild = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn);
        var image = curchild.GetComponent<Image>();
        var color = image.color;
        color.a = 1.0f;
        image.color = color;
    }
	
	// Update is called once per frame
	void Update () {

        // m_Buttones[m_ButtonCount].Flash();
        // カーソルが指定座標に辿り着いていない場合は返す
        if (Vector3.Distance(m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).position, this.transform.position) > 0.1f) return;
        else
        {
            m_IsMove = true;
            //if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.75f) m_Buttones[m_ButtonCount].Flash();
        }

        if (!m_IsMove) return;

        // if (Input.GetButtonDown("OK") && m_TextCount < m_NameBox.transform.childCount)
        if (Input.GetButtonDown("OK"))
        {
            //audio.Play();
            //audio.PlayOneShot(m_DicisionSE);
            //m_Buttones[m_ButtonCount].ChangeScene();
            //m_IsMove = false;
            var child = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn);
            //var text = child.GetComponent<Text>();
            //if (text.text == "" || text.text == "　") return;
            //var nameText = m_NameTexts.transform.GetChild(m_TextCount).GetComponent<Text>();

            //var nameText = m_NameBox.transform.GetChild(m_TextCount).GetComponent<LanguageTextButton>();

            m_NameBox.AddText(child.GetComponent<TextButton>());


            //var inputText = child.GetComponent<TextButton>();
            //if (inputText.GetText() == "") return;
            //nameText.SetText(inputText, 0);
            //nameText.text = text.text;
            //m_TextCount++;

            return;
        }

        // if (Input.GetButtonDown("Cancel") && m_TextCount > 0)
        if (Input.GetButtonDown("Cancel"))
        {
            // テキストの削除
            //var nameText = m_NameTexts.transform.GetChild(m_TextCount - 1).GetComponent<Text>();
            //var nameText = m_NameBox.transform.GetChild(m_TextCount - 1).GetComponent<LanguageTextButton>();

            //nameText.text = "*";
            //nameText.DeleteText();

            m_NameBox.DeleteText();

            //m_TextCount--;
            return;
        }

        // 入力の値が一定値を超えた場合、ボタンカウントを変動させる
        int column = m_CursorColumn;
        if (Input.GetAxis("Vertical") > m_InputPower) m_CursorColumn = Mathf.Max(m_CursorColumn - 1, 0);
        else if (Input.GetAxis("Vertical") < -m_InputPower) m_CursorColumn = Mathf.Min(m_CursorColumn + 1, m_TextButtons.transform.GetChild(0).childCount - 1);
        int row = m_CursorRow;
        if (Input.GetAxis("Horizontal") > m_InputPower) m_CursorRow = Mathf.Min(m_CursorRow + 1, m_TextButtons.transform.childCount - 1);
        else if (Input.GetAxis("Horizontal") < -m_InputPower) m_CursorRow = Mathf.Max(m_CursorRow - 1, 0);

        // ツインを利用した移動
        // カーソルが指定座標に辿り着いる場合は返す。
        if (m_CursorColumn == column && m_CursorRow == row) return;
        // 移動中
        m_IsMove = false;
        //audio.PlayOneShot(m_SelectSE);
        this.transform.DOMove(m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn).position, m_MoveTime);

        // 発光
        //
        var curchild = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn);
        var image = curchild.GetComponent<Image>();
        if (image.sprite != null)
        {
            var color = image.color;
            color.a = 1.0f;
            image.color = color;
        }

        // 発光の停止
        var previmage = m_TextButtons.transform.GetChild(m_PrevCursorRow).GetChild(m_PrevCursorColumn).GetComponent<Image>();
        if (previmage.sprite != null)
        {
            var prevcolor = previmage.color;
            prevcolor.a = 0.0f;
            previmage.color = prevcolor;
        }

        m_PrevCursorRow = m_CursorRow;
        m_PrevCursorColumn = m_CursorColumn;
        // m_Buttones[m_ButtonCount].Flash();
        // 発光処理処理の停止
        //m_Buttones[prevCount].StopFlash();
    }
}
