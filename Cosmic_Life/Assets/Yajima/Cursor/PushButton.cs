using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushButton : MonoBehaviour {

    // 遷移するシーン
    [SerializeField]
    private SceneType m_Type = SceneType.LogoScene;
    // フラッシュイメージ
    [SerializeField]
    private FlashImage m_FlashImage;
    // シャドウイメージ
    [SerializeField]
    private Image m_ShadowImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 発光
    public void Flash()
    {
        if (m_ShadowImage.color.a <= 0.0f) return;
        m_FlashImage.gameObject.SetActive(true);
        // 発光
        m_FlashImage.StartFlash();
        Color color = m_ShadowImage.color;
        color.a = 0.0f;
        m_ShadowImage.color = color;
    }

    public void StopFlash()
    {
        m_FlashImage.EndFlash(ShadowOn);
    }

    // シーン遷移処理
    public void ChangeScene()
    {
        SceneMgr.Instance.SceneTransition(m_Type);
    }

    // シャドウイメージを非透明化します
    private void ShadowOn()
    {
        Color color = m_ShadowImage.color;
        color.a = 1.0f;
        m_ShadowImage.color = color;
    }
}
