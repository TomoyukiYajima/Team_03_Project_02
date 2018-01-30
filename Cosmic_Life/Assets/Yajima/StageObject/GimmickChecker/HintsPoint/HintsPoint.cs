using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HintsPoint : MonoBehaviour {
    // 元画像
    [SerializeField]
    private SpriteRenderer m_Sprite;
    // 発光スプライト
    [SerializeField]
    private FlashSprite m_FlashSprite;
    // 出現時間
    [SerializeField]
    private float m_FadeTime = 1.0f;
    
    // 発光したか？
    private bool m_IsFlash = false;
    // 前回のアクティブ状態
    private bool m_IsPrevActive;

    // Use this for initialization
    void Start () {
        // 発光処理
        Flash();
        m_IsPrevActive = gameObject.activeSelf;
    }
	
	// Update is called once per frame
	void Update () {
        //m_FlashImage

        // アクティブ状態に変更があれば、処理を実行する
        if (m_IsPrevActive != gameObject.activeSelf)
        {
            Flash();
            m_IsPrevActive = gameObject.activeSelf;
        }
    }

    // 発光処理
    public void Flash()
    {
        gameObject.SetActive(true);
        m_IsFlash = true;
        m_Sprite.color = Color.white;
        //m_Sprite
        m_FlashSprite.StartFlash();
    }

    // 発光終了処理
    public void EndFlash()
    {
        m_FlashSprite.InitFlash();
        m_IsPrevActive = false;

        // m_FadeTime
        // フェードの呼び出し
        StartCoroutine(FadeEnd());
    }

    private IEnumerator FadeEnd()
    {
        // 透明化
        m_Sprite.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), m_FadeTime);
        // ディレイ
        yield return new WaitForSeconds(m_FadeTime);
        // 非アクティブ状態に変更
        gameObject.SetActive(false);

        yield return null;
    }
}
