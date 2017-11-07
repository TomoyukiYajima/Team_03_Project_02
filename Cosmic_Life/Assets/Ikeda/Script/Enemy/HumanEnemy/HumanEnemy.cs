using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanEnemy : Enemy {

    //ナビメッシュ
    private NavMeshAgent m_Agent;

    //現在の巡回ポイントのインデックス
    private int m_CurrentPatrolPointIndex;
    //巡回ポイントの保存
    private int m_BeforePatrolPoint;

    //見える距離
    [SerializeField, Tooltip("見える距離の設定")]
    private float m_ViewingDistance;

    //視野角
    [SerializeField, Tooltip("視野角の設定")]
    private float m_ViewingAngle;

    //プレイヤーへの注視点
    Transform m_PlayerLookPoint;

    //自身の目の位置
    Transform m_EyePoint;

    // Use this for initialization
    public override void Start(){
        //基底クラスの初期化
        base.Start();

        //エージェントを取得
        m_Agent = GetComponent<NavMeshAgent>();

        //最初の状態を設定
        ChangeState(EnemyStatus.RoundState);

        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");

        //目的地を設定する
        SetNewPatrolPointToDestination();

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
        Vector3 l_position = transform.position;
        l_position.y = 0;
        return (Vector3.Distance(m_Agent.destination, l_position) < 0.5f);
    }

    public void SetNewPatrolPointToDestination()
    {
        if (m_RoundPoints[0] == null) return;
        m_CurrentPatrolPointIndex = Random.Range(0, m_RoundPoints.Length);

        m_Agent.destination = m_RoundPoints[m_CurrentPatrolPointIndex].position;

        //現在の選択された要素と最後尾の要素を入れ替える
        var temp = m_RoundPoints[m_RoundPoints.Length - 1];
        m_RoundPoints[m_RoundPoints.Length - 1] = m_RoundPoints[m_CurrentPatrolPointIndex];
        m_RoundPoints[m_CurrentPatrolPointIndex] = temp;
    }

}
