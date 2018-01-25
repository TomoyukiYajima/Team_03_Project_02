using System;
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
    // 終了時に実行する処理
    private Action m_EndAction = () => { };

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

    public void EndFlash(Action action)
    {
        m_EndAction = action;
        m_IsStop = true;
    }

    private IEnumerator Flash()
    {
        // 非透明化
        m_Image.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), m_FadeTime / 2);
        // ディレイ
        yield return new WaitForSeconds(m_FadeTime / 2);
        // 透明化
        m_Image.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), m_FadeTime / 2);
        // 再帰呼び出し
        yield return new WaitForSeconds(m_FadeTime / 2 + m_FlashDelay);
        // 停止状態ならブレイクする
        if (m_IsStop)
        {
            // 終了時に設定されたメソッドを実行
            m_EndAction();
            yield break;
        }
        StartCoroutine(Flash());
    }

    // 発光の初期化
    public void InitFlash(Action action)
    {
        // 停止処理
        EndFlash(action);
        // ツインの強制終了
        m_Image.DOKill();
        Color color = Color.white;
        color.a = 0.0f;
        if (m_Image != null) m_Image.color = color;
    }
}
