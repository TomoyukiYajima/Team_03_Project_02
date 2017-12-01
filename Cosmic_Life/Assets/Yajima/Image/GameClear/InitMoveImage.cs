using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InitMoveImage : MonoBehaviour {

    // 移動させるイメージ
    [SerializeField]
    private GameObject[] m_MoveImages;
    // 移動ポイント配列
    [SerializeField]
    private Transform[] m_MovePoints;
    // 発光させるイメージ
    [SerializeField]
    private GameObject m_FlashImage;
    // ディレイ
    [SerializeField]
    private float m_StartDelayTime = 1.0f;
    // 合計移動時間
    [SerializeField]
    private float m_MoveTime = 1.0f;

    // 経過時間
    private float m_TotalTime;
    // 発光させたか？
    private bool m_IsFlash = false;

	// Use this for initialization
	void Start () {
        // 画像を移動させる
        for (int i = 0; i != m_MoveImages.Length; ++i)
        {
            m_MoveImages[i].transform.DOMove(m_MovePoints[i].position, m_MoveTime);
        }

        // StartCoroutine(Flash(color, time));
    }

    // Update is called once per frame
    void Update () {
        m_TotalTime += Time.deltaTime;

        // 移動が終了していなければ、返す
        if (m_MoveTime > m_TotalTime || m_IsFlash) return;
        // 発光処理
        m_IsFlash = true;
        FlashImage flash = m_FlashImage.GetComponent<FlashImage>();
        flash.StartFlash();
        //print("発光");
	}
}
