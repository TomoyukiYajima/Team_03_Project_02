using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlaneMove : MonoBehaviour {

    enum MoveDirection {
        LEFT    = 1 << 0,
        RIGHT   = 1 << 1
    }

    // 移動方向
    [SerializeField]
    private MoveDirection m_MoveDirection = MoveDirection.LEFT;
    // 動かすプレーン
    [SerializeField]
    private GameObject m_MovePlane;
    // 展開するプレーン
    [SerializeField]
    private GameObject m_ExpandPlane;

    // 移動開始座標
    [SerializeField]
    private Transform m_StartPoint;
    // 移動終了座標
    [SerializeField]
    private Transform m_GoalPoint;
    // 展開開始座標
    //[SerializeField]
    //private Transform m_ExpandPoint;
    // 動かす時間
    [SerializeField]
    private float m_MoveTime = 1.0f;
    [SerializeField]
    private float m_StartDelayTime = 1.0f;

    // 合計移動時間
    private float m_TotalMoveTime = 0.0f;
    // 計算方向
    private int m_Direction = -1;
    private float m_Delay = 0.32f;

	// Use this for initialization
	void Start () {
        // 動かすプレーンを、開始座標の座標に変更する
        m_MovePlane.transform.position = m_StartPoint.position;
        //// 移動
        //m_MovePlane.transform.DOMove(m_GoalPoint.position, m_MoveTime);
        //// 拡大
        //m_MovePlane.transform.DOScale(new Vector3(1.2f, 1.2f, 1.0f), m_MoveTime * m_Delay);

        //switch (m_MoveDirection) { }
        // 方向データが変わっていたら、方向を変更する
        if (m_MoveDirection == MoveDirection.RIGHT) m_Direction = 1;

        // 展開するプレーンの角度の設定
        m_ExpandPlane.transform.Rotate(Vector3.up * (m_Direction * 90));
        //// 角度をもとに戻す
        //m_ExpandPlane.transform.DORotate(Vector3.zero, m_MoveTime / 2);


        //StartCoroutine(PlaneRotate());
        StartCoroutine(PlaneMoves());
    }
	
	// Update is called once per frame
	void Update () {

        //var dir = m_ExpandPoint.position - m_MovePlane.transform.position;
        //// 展開開始地点を超えたら、
        //if(dir.x < 0.0f)
        //{

        //}
	}

    private IEnumerator PlaneMoves()
    {
        // ディレイ
        yield return new WaitForSeconds(m_StartDelayTime);
        // 移動
        m_MovePlane.transform.DOMove(m_GoalPoint.position, m_MoveTime);
        // 拡大
        m_MovePlane.transform.DOScale(new Vector3(1.2f, 1.2f, 1.0f), m_MoveTime * m_Delay);

        // ディレイ
        yield return new WaitForSeconds(m_MoveTime * m_Delay);
        // 角度をもとに戻す
        m_ExpandPlane.transform.DORotate(Vector3.zero, m_MoveTime * (1 - m_Delay));
        // 縮小
        m_MovePlane.transform.DOScale(Vector3.one, m_MoveTime * (1 - m_Delay));
    }

    private IEnumerator PlaneRotate()
    {
        // ディレイ
        //float value = 0.325f;
        yield return new WaitForSeconds(m_MoveTime * m_Delay);
        // 角度をもとに戻す
        m_ExpandPlane.transform.DORotate(Vector3.zero, m_MoveTime * (1 - m_Delay));
        // 縮小
        m_MovePlane.transform.DOScale(Vector3.one, m_MoveTime * (1 - m_Delay));
    }
}
