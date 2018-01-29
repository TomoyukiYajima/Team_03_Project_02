using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MissionCompleteUI : MonoBehaviour {

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
    // 移動終了後の非表示時間
    [SerializeField]
    private float m_ActiveTime = 1.0f;

    // 経過時間
    private float m_TotalTime;
    // 発光させたか？
    private bool m_IsFlash = false;
    // 初期座標配列
    private List<Vector3> m_InitImagePositions = new List<Vector3>();
    // 前回のアクティブ状態
    private bool m_IsPrevActive;

    // Use this for initialization
    public virtual void Start()
    {
        // 画像を移動させる
        for (int i = 0; i != m_MoveImages.Length; ++i)
        {
            m_InitImagePositions.Add(m_MoveImages[i].transform.position);
        }

        m_IsPrevActive = gameObject.activeSelf;
        // 実行
        Action();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // アクティブ状態に変更があれば、処理を実行する
        if(m_IsPrevActive != gameObject.activeSelf)
        {
            Action();
            m_IsPrevActive = gameObject.activeSelf;
        }

        m_TotalTime += Time.deltaTime;

        // 移動が終了していなければ、返す
        if (m_MoveTime > m_TotalTime || m_IsFlash) return;
        // 発光処理
        m_IsFlash = true;
        FlashImage flash = m_FlashImage.GetComponent<FlashImage>();
        flash.StartFlash();
        // 終了処理
        StartCoroutine(EndAction());
    }

    // 終了処理
    protected virtual IEnumerator EndAction()
    {
        yield return new WaitForSeconds(m_ActiveTime);

        // 発光の停止
        EndFlash();
        // 時間経過したら非表示にする
        Color color = Color.white;
        color.a = 0.0f;
        float time = 0.5f;
        ChangeColor(color, time);

        // フェードして消す
        yield return new WaitForSeconds(time);

        // 初期化
        Init();
        gameObject.SetActive(false);
        m_IsPrevActive = false;

        yield return null;
    }

    private void EndFlash()
    {
        FlashImage flash = m_FlashImage.GetComponent<FlashImage>();
        flash.InitFlash(null);
    }

    protected virtual void Init()
    {
        m_TotalTime = 0.0f;
        m_IsFlash = false;
        ChangeColor(Color.white, 0.01f);
        // 座標の初期化
        InitPosition();
    }

    // 色の変更
    private void ChangeColor(Color color, float time)
    {
        // 時間経過したら非表示にする
        for (int i = 0; i != m_MoveImages.Length; ++i)
        {
            var parent = m_MoveImages[i];
            for (int j = 0; j != m_MoveImages[i].transform.childCount; ++j)
            {
                var child = m_MoveImages[i].transform.GetChild(j);
                var image = child.GetComponent<Image>();
                image.DOColor(color, time);
            }
        }
    }

    // 座標の初期化
    private void InitPosition()
    {
        // 画像を移動させる
        for (int i = 0; i != m_MoveImages.Length; ++i)
        {
            m_MoveImages[i].transform.position = m_InitImagePositions[i];
        }
    }

    // 実行
    public void Action()
    {
        for (int i = 0; i != m_MoveImages.Length; ++i)
        {
            m_MoveImages[i].transform.DOMove(m_MovePoints[i].position, m_MoveTime);
        }

        SoundManager.Instance.PlaySe("SE_Stage_Clear");
    }
}
