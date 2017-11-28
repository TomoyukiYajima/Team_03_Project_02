using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamingText : MonoBehaviour {

    // イメージ
    [SerializeField]
    private Image m_TextImage;
    // 消去時の文字
    [SerializeField]
    private Text m_DeleteText;

    // 濁点
    private string m_SonantMarkText = "";
    // 半濁点
    private string m_PSoundMarkText = "";
    // 濁点用画像
    private Sprite m_SonantMarkSprite;
    // 半濁点用画像
    private Sprite m_PSoundMarkSprite;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ChangeText(TextButton button)
    {
        // 画像の変更
        var sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
        // 各オブジェクトのアクティブ状態を変更
        m_DeleteText.gameObject.SetActive(false);
        m_TextImage.gameObject.SetActive(true);
        m_TextImage.sprite = sprite;
    }

    public void DeleteText()
    {
        // 各オブジェクトのアクティブ状態を変更
        m_DeleteText.gameObject.SetActive(true);
        m_TextImage.gameObject.SetActive(false);
        // 濁点用の文字列を空にする
        m_SonantMarkText = "";
        m_PSoundMarkText = "";
        // 画像も空にする
        m_SonantMarkSprite = null;
        m_PSoundMarkSprite = null;
    }

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

    // 文字を濁点に変更
    public void ChangeSonantMarkText()
    {
        if (m_SonantMarkSprite == null) return;
        m_TextImage.sprite = m_SonantMarkSprite;
    }

    // 文字を半濁点に変更
    public void ChangePSoundMarkText()
    {
        if (m_PSoundMarkSprite == null) return;
        m_TextImage.sprite = m_PSoundMarkSprite;
    }

    // 文字の変更ができるか
    public bool IsChangeTest(TextButton button) { return button.transform.GetChild(0).GetComponent<Image>().sprite != null; }
}
