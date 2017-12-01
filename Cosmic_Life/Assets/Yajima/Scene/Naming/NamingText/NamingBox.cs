using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingBox : MonoBehaviour {

    // 現在の入力カウント
    private int m_CurrentCount = 0;
    // ネーミングテキスト配列
    private List<NamingText> m_NamingTexts = new List<NamingText>();
    // テキストデータ
    private string m_UndroidName = "";

	// Use this for initialization
	void Start () {
        for (int i = 0; i != this.transform.childCount; ++i)
        {
            var text = this.transform.GetChild(i).GetComponent<NamingText>();
            if (text == null) continue;
            m_NamingTexts.Add(text);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // テキストの追加
    public void AddText(TextButton button)
    {
        var lanText = button.GetComponent<LanguageTextButton>();
        if(lanText != null)
        {
            if (m_CurrentCount - 1 >= this.transform.childCount - 1) return;
            m_NamingTexts[m_CurrentCount].SonantMarkText = lanText.SonantMarkText;
            m_NamingTexts[m_CurrentCount].PSoundMarkText = lanText.PSoundMarkText;
            // テキストに加算
            m_UndroidName += button.GetText();
            // 画面のテキスト画像を変更する
            m_NamingTexts[m_CurrentCount].ChangeText(button);
            // 画像を入れる
            m_NamingTexts[m_CurrentCount].SonantMarkSprite = lanText.SonantMarkSprite;
            m_NamingTexts[m_CurrentCount].PSoundMarkSprite = lanText.PSoundMarkSprite;
            print(m_CurrentCount);
            // カウントを増やす
            m_CurrentCount = Mathf.Min(m_CurrentCount + 1, this.transform.childCount);
            return;
        }

        // 変更する
        var markText = button.GetComponent<MarkTextButton>();
        if(markText != null)
        {
            var backCount = Mathf.Max(m_CurrentCount - 1, 0);
            if (backCount < 0) return;
            m_UndroidName = markText.ChangeText(m_NamingTexts[backCount], m_UndroidName);
            return;
        }
    }

    // テキストの削除
    public void DeleteText()
    {
        m_CurrentCount = Mathf.Max(m_CurrentCount - 1, 0);
        // 指定したネームテキストの削除
        m_NamingTexts[m_CurrentCount].DeleteText();
        // 範囲外防止
        if (m_UndroidName.Length != 0) m_UndroidName = m_UndroidName.Remove(m_UndroidName.Length - 1, 1);
    }

    // テキスト数を取得
    public int GetTextCount() { return m_CurrentCount; }
}
