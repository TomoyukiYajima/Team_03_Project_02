﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkEnemy : Enemy {

    [System.NonSerialized]
    public NavMeshAgent m_Agent;

    //現在の巡回ポイントのインデックス
    private int m_CurrentPatrolPointIndex = -1;

    //見える距離
    [SerializeField, Tooltip("見える距離の設定")]
    private float m_ViewingDistance;

    //視野角
    [SerializeField, Tooltip("視野角の設定")]
    private float m_ViewingAngle;

    //プレイヤーへの注視点
    Transform m_PlayerLookPoint;

    //Robotへの注視点
    Transform m_RobotLookPoint;

    //自身の目の位置
    Transform m_EyePoint;

    //巡回するか？
    [SerializeField]
    private bool m_IsPatrol;

    //初期位置と向いている方向の保存用
    private Vector3 m_StartPosition;
    private Quaternion m_StartAngle;


    // Use this for initialization
    public override void Start() {
        base.Start();

        m_Agent = GetComponent<NavMeshAgent>();

        //目的地を設定する
        SetNewPatrolPointToDestination();

        //最初の状態を設定する
        if (m_IsPatrol)
            ChangeState(EnemyStatus.RoundState);
        else
            ChangeState(EnemyStatus.NonRoundState);

        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_RobotLookPoint = m_Robot.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");

        //初期位置と向いている方向の保存する
        m_StartPosition = transform.position;
        m_StartAngle = transform.rotation;
    }

    // Update is called once per frame
    //   void Update () {
    //}

    //優先する方を調べて、優先するべき方を返す
    public GameObject CheckPlayerAndRobot()
    {
        //どっちも見えている場合、プレイヤー優先
        if (CanSeePlayer() && CanSeeRobot())
        {
            return m_Player;
        }
        else if (CanSeePlayer())
        {
            return m_Player;
        }
        else if (CanSeeRobot())
        {
            return m_Robot;
        }

        return null;
    }
    
    public bool CanSeeRobot()
    {
        if (!IsRobotInViewingDistance())
            return false;

        if (!IsRobotInViewingAngle())
            return false;

        if (!CanHitRayToRobot())
            return false;

        return true;
    }

    public bool CanSeePlayer()
    {
        if (!IsPlayerInViewingDistance())
            return false;

        if (!IsPlayerInViewingAngle())
            return false;

        if (!CanHitRayToPlayer())
            return false;

        return true;
    }

    bool IsRobotInViewingDistance()
    {
        if (m_Robot == null) return false;
        //自身からロボットまでの距離
        float distanceToRobot = Vector3.Distance(m_RobotLookPoint.position, m_EyePoint.position);

        return (distanceToRobot <= m_ViewingDistance);
    }

    bool IsRobotInViewingAngle()
    {
        //自身からロボットへの方向ベクトル
        Vector3 directionToRobot = m_RobotLookPoint.position - m_EyePoint.position;

        //自分の正面向きベクトルとロボットへの方向ベクトルの差分角度
        float angleToRobot = Vector3.Angle(m_EyePoint.forward, directionToRobot);

        //見える角度の範囲内にロボットがいるかどうかを返却する
        return (Mathf.Abs(angleToRobot) <= m_ViewingAngle);
    }

    bool CanHitRayToRobot()
    {
        //自身からロボットへの方向ベクトル
        Vector3 directionToRobot = m_RobotLookPoint.position - m_EyePoint.position;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(m_EyePoint.position, directionToRobot, out hitInfo);

        //ロボットにRayが当たったかどうか返却する
        return (hit && hitInfo.collider.tag == "Robot");
    }


    public bool CanSeePlayerAndRobot()
    {
        if (CanSeePlayer() || CanSeeRobot())
            return true;

        return false;
    }

    bool IsPlayerInViewingDistance()
    {
        if (m_Player == null) return false;
        //自身からプレイヤーまでの距離
        float distanceToPlayer = Vector3.Distance(m_PlayerLookPoint.position, m_EyePoint.position);

        return (distanceToPlayer <= m_ViewingDistance);
    }

    bool IsPlayerInViewingAngle()
    {
        //自身からプレイヤーへの方向ベクトル
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        //自分の正面向きベクトルとプレイヤーへの方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        //見える角度の範囲内にプレイヤーがいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    bool CanHitRayToPlayer()
    {
        //自身からプレイヤーへの方向ベクトル
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);

        //プレイヤーにRayが当たったかどうか返却する
        return (hit && hitInfo.collider.tag == "Player");
    }


    public bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.Find("FootPosition").position) < 0.5f);
    }

    public void SetNewPatrolPointToDestination()
    {
        if (m_RoundPoints[0] == null) return;
        m_CurrentPatrolPointIndex = (m_CurrentPatrolPointIndex + 1) % m_RoundPoints.Length;

        m_Agent.destination = m_RoundPoints[m_CurrentPatrolPointIndex].position;
    }



    //ピタッと止める
    public void AgentStop()
    {
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
    }


    //巡回するかを返す
    public bool GetIsPatrol()
    {
        return m_IsPatrol;
    }

    //どれくらい視野角度の変更
    public void SetAngle(float angle)
    {
        m_ViewingAngle = angle;
    }

    public void OnDrawGizmos()
    {
        //視界の表示
        if (m_EyePoint != null)
        {
            //線の色
            Gizmos.color = new Color(0f, 0f, 1f);
            Vector3 eyePosition = m_EyePoint.position;
            Vector3 forward = m_EyePoint.forward * m_ViewingDistance;

            Gizmos.DrawRay(eyePosition, forward);
            Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, m_ViewingAngle, 0) * forward);
            Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, -m_ViewingAngle, 0) * forward);
        }

        //    //巡回ルートを描画
        //    if (m_RoundPoints != null)
        //    {
        //        Gizmos.color = new Color(0, 1, 0);

        //        for (int i = 0; i < m_RoundPoints.Length; i++)
        //        {
        //            int startIndex = i;
        //            int endIndex = i + 1;

        //            if (endIndex == m_RoundPoints.Length)
        //                endIndex = 0;

        //            Gizmos.DrawLine(m_RoundPoints[startIndex].position, m_RoundPoints[endIndex].position);
        //        }
        //    }
    }

    public Vector3 GetStartPosition()
    {
        return m_StartPosition;
    }

    public Quaternion GetStartAngle()
    {
        return m_StartAngle;
    }
}
