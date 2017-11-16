using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleCamera : MonoBehaviour {

    enum MoveType
    {
        MOVE        = 1 << 0,
        ROTATION    = 1 << 1
    }

    // 動かすカメラ
    [SerializeField]
    private GameObject m_Camera;
    // カメラの移動タイプ配列
    [SerializeField]
    private MoveType[] m_MoveTypes;
    // カメラの移動ポイント配列
    [SerializeField]
    private Transform[] m_MovePoints;
    // カメラの移動時間配列
    [SerializeField]
    private float[] m_MoveTimes;
    // 回転角度
    [SerializeField]
    private float m_RotateAngle = 30.0f;
    // 回転角度
    private int m_RotateDir = 1;

    // 行動を開始したか
    private bool m_IsMove = false;

    // 現在の移動回数
    private int m_MoveCount = 0;
    // 前の位置
    private Vector3 m_PrevPosition;
    // 経過時間
    private float m_Timer = 0.0f;

    // 前半の回転時間
    private float m_FristRotateTime = 0.0f;
    // 後半の回転時間
    private float m_SecondRotateTime = 0.0f;
    // 分割時間
    private float m_ParTime = 0.0f;

    // 移動タイプ配列
    private Dictionary<MoveType, Action<int>> m_MoveActions =
        new Dictionary<MoveType, Action<int>>();

    // Use this for initialization
    void Start () {
        // 移動関数を移動タイプ配列に追加
        m_MoveActions.Add(MoveType.MOVE, (count) => { Move(); });
        m_MoveActions.Add(MoveType.ROTATION, (count) => { Rotate(); });

        for(int i = 0; i != m_MoveTimes.Length; ++i)
        {
            if ((float)i / m_MoveTimes.Length < 0.5f) m_FristRotateTime += m_MoveTimes[i];
            else m_SecondRotateTime += m_MoveTimes[i];
        }

        m_ParTime = m_FristRotateTime;
        //if (m_MoveCount / m_MoveTypes.Length < 0.5f) m_RotateDir = -1;
        //else m_RotateDir = 1;
        // m_FristRotateTime

        m_PrevPosition = this.transform.position;
        //Rotate();
    }

    // Update is called once per frame
    void Update () {

        // 移動回数を超えたら返す
        //if (m_MoveCount >= m_MoveTypes.Length) return;

        m_Timer += Time.deltaTime;
        // 
        //m_MoveActions[m_MoveTypes[m_MoveCount]](0);
        // 移動
        Move();
        // 回転
        Rotate();

        //if (!m_IsMove)
        //{
        //    m_MoveActions[m_MoveTypes[m_MoveCount]](0);
        //    m_IsMove = true;
        //}
        // m_LiftMoves[m_LiftNumber](obj);
        // 移動時間が経過時間を超えたら、次の移動を開始する
        if (m_Timer >= m_MoveTimes[m_MoveCount])
        {
            m_MoveCount++;
            if (m_MoveCount >= m_MoveTypes.Length) m_MoveCount = 0;

            var h = (float)m_MoveCount / m_MoveTypes.Length;
            if ((float)m_MoveCount / m_MoveTypes.Length < 0.5f)
            {
                m_ParTime = m_FristRotateTime;
                m_RotateDir = 1;
            }
            else
            {
                m_ParTime = m_SecondRotateTime;
                m_RotateDir = -1;
            }

            //print(m_RotateDir.ToString());

            m_PrevPosition = this.transform.position;
            m_Timer = 0.0f;
            m_IsMove = false;
        }
    }

    // 移動
    public void Move()
    {
        // ツイン移動
        //this.transform.DOMove(m_MovePoints[m_MoveCount].position, m_MoveTimes[m_MoveCount]);
        // 一定速度で移動
        this.transform.position += (m_MovePoints[m_MoveCount].position - m_PrevPosition) / (m_MoveTimes[m_MoveCount] * 60);
    }

    // 回転
    public void Rotate()
    {
        // 回転
        this.transform.Rotate(Vector3.up, 1 / m_ParTime * m_RotateDir);

        //this.transform.DORotate(Vector3.up * m_RotateAngle, m_ParTime);

        // ツイン回転
        //var pos = m_MovePoints[m_MoveCount].position - this.transform.position;
        //this.transform.DOLookAt(pos, m_MoveTimes[m_MoveCount]);
        //this.transform.DORotate(pos, m_MoveTimes[m_MoveCount]);
    }
}
