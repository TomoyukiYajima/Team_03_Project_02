using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlashSprite : MonoBehaviour {
    // フェード時間
    [SerializeField]
    private float m_FadeTime = 1.0f;
    // ディレイ
    [SerializeField]
    private float m_FlashDelay = 0.5f;
    // 最初に発光させるか
    [SerializeField]
    private bool m_IsStartFlash = true;

    // スプライトレンダラー
    private SpriteRenderer m_Renderer;
    // イメージ
    //private Image m_Image;
    // 発光を停止させるか
    private bool m_IsStop = false;
    // 終了時に実行する処理
    //private Action m_EndAction = () => { };

    // Use this for initialization
    void Start () {
        m_Renderer = this.GetComponent<SpriteRenderer>();
        if (m_IsStartFlash) StartCoroutine(Flash());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFlash()
    {
        m_IsStop = false;
        StartCoroutine(Flash());
    }

    public void EndFlash()
    {
        //m_EndAction = action;
        m_IsStop = true;
    }

    private IEnumerator Flash()
    {
        // 非透明化
        m_Renderer.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), m_FadeTime / 2);
        // ディレイ
        yield return new WaitForSeconds(m_FadeTime / 2);
        // 透明化
        m_Renderer.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), m_FadeTime / 2);
        // 再帰呼び出し
        yield return new WaitForSeconds(m_FadeTime / 2 + m_FlashDelay);
        // 停止状態ならブレイクする
        if (m_IsStop)
        {
            yield break;
        }
        StartCoroutine(Flash());
    }

    // 発光の初期化
    public void InitFlash()
    {
        // 停止処理
        EndFlash();
        if (m_Renderer == null) m_Renderer = this.GetComponent<SpriteRenderer>();
        // ツインの強制終了
        m_Renderer.DOKill();
        Color color = Color.white;
        color.a = 0.0f;
        m_Renderer.color = color;
    }
}
