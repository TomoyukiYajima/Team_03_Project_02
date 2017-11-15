using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlashImage : MonoBehaviour {

    // フェード時間
    [SerializeField]
    private float m_FadeTime = 1.0f;
    // ディレイ
    [SerializeField]
    private float m_FlashDelay = 0.5f;
    // 最初に発光させるか
    [SerializeField]
    private bool m_IsStartFlash = true;

    // イメージ
    private Image m_Image;
    // 発光を停止させるか
    private bool m_IsStop = false;

	// Use this for initialization
	void Start () {
        m_Image = this.GetComponent<Image>();
        if (m_IsStartFlash) StartCoroutine(Flash());
    }
	
	// Update is called once per frame
	void Update () {
        //print(m_Image.color.ToString());
    }

    public void StartFlash()
    {
        m_IsStop = false;
        StartCoroutine(Flash());
    }

    public void EndFlash()
    {
        m_IsStop = true;
    }

    public IEnumerator Flash()
    {
        // 透明化
        m_Image.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), m_FadeTime / 2);
        // ディレイ
        yield return new WaitForSeconds(m_FadeTime / 2);
        // 非透明化
        m_Image.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), m_FadeTime / 2);
        // 再帰呼び出し
        yield return new WaitForSeconds(m_FadeTime / 2 + m_FlashDelay);
        // 停止状態ならブレイクする
        if (m_IsStop) yield break;
        StartCoroutine(Flash());
    }
}
