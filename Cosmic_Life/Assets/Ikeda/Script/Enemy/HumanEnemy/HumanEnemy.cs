using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanEnemy : Enemy
{

    //Agent
    [System.NonSerialized]
    public NavMeshAgent m_Agent;

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

    //援護状態にする距離の設定
    [SerializeField, Tooltip("援護状態にする距離の設定")]
    private float m_GuardDistance;

    //プレイヤーへの注視点
    Transform m_PlayerLookPoint;

    //自身の目の位置
    Transform m_EyePoint;

    //HumanRobotEnemyの配列
    [SerializeField]
    private HumanRobotEnemy[] m_HumanRobotEnemys;
    //どのEnemyが見つけたか保存
    private int m_FindingEnemy;
    //どのTargetを見つけたか保存
    private GameObject m_FindGameObject;

    // Use this for initialization
    public override void Start()
    {
        //基底クラスの初期化
        base.Start();

        //エージェントを取得
        m_Agent = GetComponent<NavMeshAgent>();

        //最初の状態を設定
        ChangeState(EnemyStatus.RoundState);

        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");

        //目的地を設定する
        if (transform.FindChild("StateList").FindChild("RoundState").GetComponent<HumanRoundState>().GetIsRound())
            SetNewPatrolPointToDestination();

    }

    public override void Update()
    {
        //基底クラスのアップデート
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) m_Hp = 0;

        //人型ロボットを援護状態にする
        CheckDistanceGuardState();

        //Enemyが見つけたか調べる
        if (!CheckFindTarget()) return;

        //見つけていないEnemyの状態を変更
        ChangeNonFindEnemy();
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

    //Enemyが見つけたか調べる
    public bool CheckFindTarget()
    {
        for (int i = 0; i < m_HumanRobotEnemys.Length; i++)
        {
            if (m_HumanRobotEnemys[i].CanSeePlayerAndRobot())
            {
                //どのEnemyが見つけたか保存
                m_FindingEnemy = i;
                //どのTargetを見つけたか保存
                m_FindGameObject = m_HumanRobotEnemys[i].CheckPlayerAndRobot();
                return true;
            }
        }
        return false;
    }

    //見つけていないEnemyをそっちを向く状態にする
    private void ChangeNonFindEnemy()
    {
        for (int i = 0; i < m_HumanRobotEnemys.Length; i++)
        {
            //見つけたEnemyは何もしない
            if (m_FindingEnemy == i) continue;

            m_HumanRobotEnemys[i].ChangeState(EnemyStatus.TurnState);
        }
    }

    //見つけたターゲットのPositionを返す
    public Vector3 GetTargetPosition()
    {
        Vector3 l_TargetPosition = m_FindGameObject.transform.position;

        return l_TargetPosition;
    }

    //距離を測って近かったら守る状態にする
    private void CheckDistanceGuardState()
    {
        if (Vector3.Distance(GetEnemyPosition(), GetPlayer().transform.position) <= m_GuardDistance)
        {
            foreach (HumanRobotEnemy enemy in m_HumanRobotEnemys)
            {
                if (enemy.GetEnemyStatus() == EnemyStatus.SupportState) continue;
                print("護衛艦隊");
                enemy.ChangeState(EnemyStatus.SupportState);
                enemy.m_Agent.isStopped = false;
                enemy.NotLook();
            }
        }
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

}
