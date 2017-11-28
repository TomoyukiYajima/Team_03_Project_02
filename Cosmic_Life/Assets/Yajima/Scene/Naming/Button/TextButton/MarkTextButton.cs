using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTextButton : TextButton {

    enum MarkType {
        SONANTMARK = 1 << 0,    // 濁点
        PSOUNDMARK = 1 << 1     // 半濁点
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
        nameText.ChangeSonantMarkText();
        text = text.Remove(text.Length - 1, 1);
        return text += nameText.SonantMarkText;
    }
}
