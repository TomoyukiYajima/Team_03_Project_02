using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyAttack : EnemyState
{

    public enum AttackState
    {
        Attack,
        AttackAfter,

        None
    }

    //攻撃の距離を設定
    [SerializeField]
    private float m_PlayerStopDistance;
    [SerializeField]
    private float m_RobotStopDistance;


    private float m_DistanceCompare;

    //衝突判定オブジェクト
    [SerializeField]
    private GameObject m_AttackCollider;

    [SerializeField]
    private float m_AttackTime = 15.0f;

    private AttackState m_AttackState;

    private float m_Timer = 0.0f;

    [SerializeField, Tooltip("クールタイムの設定(秒)")]
    private float m_SetCoolTime;

    private float m_CoolTime;

    private GameObject m_TargetObject;

    WalkEnemy m_WalkEnemy;

    private bool m_IsAttack;

    // Use this for initialization
    void Start()
    {
        m_AttackState = AttackState.Attack;
        m_IsAttack = false;
    }

    //void Update()
    //{
    //}

    public AttackState GetWalkEnemyState()
    {
        return m_AttackState;
    }

    public bool GetIsAttack()
    {
        return m_IsAttack;
    }

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_WalkEnemy == null)
            m_WalkEnemy = enemy.GetComponent<WalkEnemy>();

        if (m_WalkEnemy.CanSeePlayer())
        {
            switch (m_AttackState)
            {
                case AttackState.Attack:
                    m_IsAttack = true;

                    m_AttackCollider.SetActive(true);
                    SoundManager.Instance.PlaySe("SE_Droid_Attack_01");

                    if (m_AttackTime > m_Timer)
                        m_Timer += Time.deltaTime;
                    else
                    {
                        m_Timer = 0;
                        m_CoolTime = m_SetCoolTime;
                        m_IsAttack = false;
                        m_AttackState = AttackState.AttackAfter;
                    }
                    break;

                case AttackState.AttackAfter:
                    m_AttackCollider.SetActive(false);
                    //WalkEnemy l_WalkEnemy = enemy.GetComponent<WalkEnemy>();
                    //NULLだったら状態を変更
                    m_TargetObject = m_WalkEnemy.CheckPlayerAndRobot();
                    if (m_TargetObject == null)
                    {
                        m_WalkEnemy.m_Agent.isStopped = false;
                        m_WalkEnemy.ChangeState(EnemyStatus.ReturnPosition);
                        return;
                    }

                    //クールタイムを調べる
                    if (m_CoolTime >= 0) m_CoolTime -= deltaTime;

                    CheckStopDistance(enemy);

                    //攻撃後のプレイヤーとの距離を測って、離れていたら追跡中に変更
                    float distance = (Vector3.Distance(m_WalkEnemy.GetEnemyPosition(), m_TargetObject.transform.position));
                    if (distance < m_DistanceCompare)
                    {
                        //近ければプレイヤーの方向を向いて攻撃
                        Vector3 relativePos = m_WalkEnemy.CheckPlayerAndRobot().transform.position - m_WalkEnemy.transform.position;
                        relativePos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(relativePos);
                        m_WalkEnemy.transform.rotation = Quaternion.Slerp(m_WalkEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

                        //自身の前方向とプレイヤーとの角度を調べる
                        if (Vector3.Angle(m_WalkEnemy.transform.forward, relativePos) <= 1.5f && m_CoolTime <= 0)
                        {
                            m_AttackState = AttackState.Attack;
                        }
                    }
                    else
                    {
                        m_AttackState = AttackState.Attack;
                        m_WalkEnemy.m_Agent.isStopped = false;
                        enemy.ChangeState(EnemyStatus.Chasing);
                    }
                    break;
            }
        }
        else
        {
            m_WalkEnemy.m_Agent.isStopped = false;
            m_WalkEnemy.ChangeState(EnemyStatus.ReturnPosition);
        }
    }


    private void CheckStopDistance(Enemy enemy)
    {
        if (enemy.GetComponent<WalkEnemy>().CheckPlayerAndRobot() == null) return;

        if (enemy.GetComponent<WalkEnemy>().CheckPlayerAndRobot().transform.tag == "Player")
        {
            m_DistanceCompare = m_PlayerStopDistance;
        }
        else
        {
            m_DistanceCompare = m_RobotStopDistance;
        }
    }
}
