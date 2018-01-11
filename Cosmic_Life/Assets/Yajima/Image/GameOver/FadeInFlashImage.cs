using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInFlashImage : MonoBehaviour {

    // 透過イメージ
    [SerializeField]
    private Image m_FadeImage;
    // 発光させるイメージ
    [SerializeField]
    private GameObject m_FlashImage;
    // 合計フェード時間
    [SerializeField]
    private float m_FadeTime = 1.0f;
    // 移動終了後のシーン遷移時間
    [SerializeField]
    private float m_ChangeSceneTime = 1.0f;

    // 経過時間
    private float m_TotalTime;
    // 発光させたか？
    private bool m_IsFlash = false;

    // Use this for initialization
    void Start () {
        //// サウンドの再生
        //SoundManager.Instance.PlaySe("SE_Stage_Clear");
        m_FadeImage.DOColor(Color.white, m_FadeTime);
    }

    // Update is called once per frame
    void Update () {
        m_TotalTime += Time.deltaTime;

        // 移動が終了していなければ、返す
        if (m_FadeTime > m_TotalTime || m_IsFlash) return;
        // 発光処理
        m_IsFlash = true;
        FlashImage flash = m_FlashImage.GetComponent<FlashImage>();
        flash.StartFlash();
        // シーン遷移
        StartCoroutine(ChangeScene());
    }

    // シーン遷移
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(m_ChangeSceneTime);

        // 時間経過したらシーン遷移
        SceneMgr.Instance.SceneTransition(SceneType.Title);

        yield return null;
    }
}
