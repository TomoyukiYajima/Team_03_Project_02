using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTextButton : TextButton {

    enum MarkType {
        SONANTMARK  = 1 << 0,   // 濁点
        PSOUNDMARK  = 1 << 1,   // 半濁点
        DELETEMARK  = 1 << 2,   // 削除
        ENDMARK     = 1 << 3    // 終了
    }

    [SerializeField]
    private MarkType m_MarkType = MarkType.SONANTMARK;

	// Use this for initialization
	public override void Start () {
		
	}

    // Update is called once per frame
    public override void Update () {
		
	}

    // テキストを設定します
    public override void SetText(TextButton text, int count)
    {
        var lanText = text.GetComponent<LanguageTextButton>();

        switch (m_MarkType)
        {
            case MarkType.SONANTMARK: lanText.ChangeText(lanText.SonantMarkText); break;
            case MarkType.PSOUNDMARK: lanText.ChangeText(lanText.PSoundMarkText); break;
        }
    }

    public string ChangeText(NamingText nameText, string text)
    {
        //text.SonantMarkText;
        //nameText.ChangeSonantMarkText();
        //print(text);
        //if (nameText.SonantMarkSprite == null) return text;
        //text = text.Remove(text.Length - 1, 1);
        //text += nameText.SonantMarkText;
        //print(text);
        //return text;

        switch (m_MarkType)
        {
            case MarkType.SONANTMARK: text = ChangeSonant(nameText, text); break;
            case MarkType.PSOUNDMARK: text = ChangePSound(nameText, text); break;
        }

        return text;
    }

    // 濁点に変更
    public string ChangeSonant(NamingText nameText, string text)
    {
        nameText.ChangeSonantMarkText();
        //print(text);
        if (nameText.SonantMarkSprite == null) return text;
        text = text.Remove(text.Length - 1, 1);
        text += nameText.SonantMarkText;
        //print(text);
        return text;
    }

    // 半濁点に変更
    public string ChangePSound(NamingText nameText, string text)
    {
        nameText.ChangePSoundMarkText();
        //print(text);
        if (nameText.PSoundMarkSprite == null) return text;
        text = text.Remove(text.Length - 1, 1);
        text += nameText.PSoundMarkText;
        return text;
    }

    // 文字の削除
    public string DeleteText(NamingText nameText, string text)
    {
        return text;
    }
}
