using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageTextButton : TextButton
{
    // 濁点時のテキスト
    [SerializeField]
    private string m_SonantMarkText = "";
    // 半濁点時のテキスト
    [SerializeField]
    private string m_PSoundMarkText = "";
    // 濁点用画像
    [SerializeField]
    private Sprite m_SonantMarkSprite;
    // 半濁点用画像
    [SerializeField]
    private Sprite m_PSoundMarkSprite;

    // Use this for initialization
    public override void Start () {
        //if (m_SonantMarkText == "") m_SonantMarkText = m_InputText;
        //if (m_PSoundMarkText == "") m_PSoundMarkText = m_InputText;
        //if (m_SonantMarkSprite == null) m_SonantMarkSprite = this.GetComponent<Image>().sprite;
        //if (m_PSoundMarkSprite == null) m_SonantMarkSprite = this.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    public override void Update () {
		
	}

    // テキストを設定します
    public override void SetText(TextButton text, int count)
    {
        base.SetText(text, count);

        var nameText = this.GetComponent<Text>();
        nameText.text = m_InputText;
    }

    // テキストの変更
    public void ChangeText(string text) { m_InputText = text; }

    // 濁点の設定
    public string SonantMarkText
    {
        get { return m_SonantMarkText; }
        set { m_SonantMarkText = value; }
    }

    // 半濁点の設定
    public string PSoundMarkText
    {
        get { return m_PSoundMarkText; }
        set { m_PSoundMarkText = value; }
    }

    // 濁点用の画像の設定
    public Sprite SonantMarkSprite
    {
        get { return m_SonantMarkSprite; }
        set { m_SonantMarkSprite = value; }
    }

    // 半濁点用の画像の設定
    public Sprite PSoundMarkSprite
    {
        get { return m_PSoundMarkSprite; }
        set { m_PSoundMarkSprite = value; }
    }

    //// テキストの削除
    //public void DeleteText()
    //{
    //    var text = "*";
    //    m_InputText = text;
    //    m_SonantMarkText = "";
    //    m_PSoundMarkText = "";

    //    var nameText = this.GetComponent<Text>();
    //    nameText.text = text;
    //}
}
