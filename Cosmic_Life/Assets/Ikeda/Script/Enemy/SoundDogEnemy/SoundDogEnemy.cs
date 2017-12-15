using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoundDogEnemy : Enemy
{

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


    private bool m_IsHear;


    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_Agent = GetComponent<NavMeshAgent>();

        //目的地を設定する
        SetNewPatrolPointToDestination();

        //最初の状態を設定する
        ChangeState(EnemyStatus.RoundState);

        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_RobotLookPoint = m_Robot.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");


        m_IsHear = false;
    }

    // Update is called once per frame
    //void Update () {

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



    public override void onHear()
    {
        print("聞こえた!!!");
        m_IsHear = true;
    }

    public bool GetIsHear()
    {
        return m_IsHear;
    }
}
