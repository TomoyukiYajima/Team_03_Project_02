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
    public FlashImage m_FlashImage;
    // シャドウイメージ
    [SerializeField]
    public Image m_ShadowImage;

    // ボタンが押されたか？
    protected bool m_IsDown;

	// Use this for initialization
	public virtual void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
		
	}

    // 初期化
    public virtual void Init() { }

    // 発光
    public virtual void Flash()
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

    // ボタンが押された時の処理
    public virtual void DownAction()
    {
        m_IsDown = true;
        ChangeScene();
    }

    // 遷移先のステージを返します
    public SceneType GetSceneType() { return m_Type; }

    // ボタンが押されたかを返します。
    public bool GetIsDown() { return m_IsDown; }

    // シーン遷移処理
    private void ChangeScene()
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
