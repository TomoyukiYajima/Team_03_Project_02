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

    [System.NonSerialized]
    public bool m_IsHear;

    [SerializeField, Tooltip("聞こえる範囲の設定")]
    private float m_HearRange;

    //聞こえたときのポジションを保存しておく
    private Vector3 m_TargetPosition;

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
        m_TargetPosition = Vector3.zero;
    }

    // Update is called once per frame
    //void Update () {

    //}


    //優先する方を調べて、優先するべき方を返す
    public GameObject CheckPlayer()
    {
        if (CanSeePlayer())
        {
            return m_Player;
        }

        return null;
    }

    //public bool CanSeeRobot()
    //{
    //    if (!IsRobotInViewingDistance())
    //        return false;

    //    if (!IsRobotInViewingAngle())
    //        return false;

    //    if (!CanHitRayToRobot())
    //        return false;

    //    return true;
    //}

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

    //bool IsRobotInViewingDistance()
    //{
    //    if (m_Robot == null) return false;
    //    //自身からロボットまでの距離
    //    float distanceToRobot = Vector3.Distance(m_RobotLookPoint.position, m_EyePoint.position);

    //    return (distanceToRobot <= m_ViewingDistance);
    //}

    //bool IsRobotInViewingAngle()
    //{
    //    //自身からロボットへの方向ベクトル
    //    Vector3 directionToRobot = m_RobotLookPoint.position - m_EyePoint.position;

    //    //自分と同じ高さにRobotがいるかどうか調べる
    //    float l_RobotY = m_Robot.transform.position.y;
    //    if (l_RobotY >= transform.position.y)
    //        return false;

    //    //自分の正面向きベクトルとロボットへの方向ベクトルの差分角度
    //    float angleToRobot = Vector3.Angle(m_EyePoint.forward, directionToRobot);

    //    //見える角度の範囲内にロボットがいるかどうかを返却する
    //    return (Mathf.Abs(angleToRobot) <= m_ViewingAngle);
    //}

    //bool CanHitRayToRobot()
    //{
    //    //自身からロボットへの方向ベクトル
    //    Vector3 directionToRobot = m_RobotLookPoint.position - m_EyePoint.position;

    //    RaycastHit hitInfo;
    //    bool hit = Physics.Raycast(m_EyePoint.position, directionToRobot, out hitInfo);

    //    //ロボットにRayが当たったかどうか返却する
    //    return (hit && hitInfo.collider.tag == "Robot");
    //}

    public bool CanSeePlayerAndRobot()
    {
        if (CanSeePlayer())
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

        //自分と同じ高さにPlayerがいるかどうか調べる
        float l_PlayerY = m_Player.transform.position.y;
        if (l_PlayerY >= transform.position.y)
            return false;

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


    //目標をPlayerにする。
    public override void onHear()
    {
        print("聞こえた!!!");
        //聞こえる範囲内にいるか
        if (Vector3.Distance(transform.position, m_Player.transform.position) <= m_HearRange)
            m_IsHear = true;
    }

    public bool GetIsHear()
    {
        return m_IsHear;
    }

    //ピタッと止める
    public void AgentStop()
    {
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
    }

    public void SetAgentSpeed(float speed)
    {
        m_Agent.speed = speed;
    }

    public void SetAngle(float angle)
    {
        m_ViewingAngle = angle;
    }

    public void SetTargetPosition(Vector3 position)
    {
        m_TargetPosition = position;
    }

    public Vector3 GetTargetPosition()
    {
        return m_TargetPosition;
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
    }
}
