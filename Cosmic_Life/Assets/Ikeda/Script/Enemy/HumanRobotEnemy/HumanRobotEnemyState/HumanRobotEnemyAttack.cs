using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyAttack : EnemyState
{

    private enum AttackState
    {
        Attack,
        AttackAfter,

        None
    }

    //攻撃の範囲を設定
    [SerializeField]
    private float m_DistanceCompare;

    //衝突判定オブジェクト
    [SerializeField]
    private GameObject m_AttackCollider;

    [SerializeField]
    private float m_AttackTime = 1.0f;

    private AttackState m_AttackState;

    private float m_Timer = 0.0f;

    [SerializeField, Tooltip("クールタイムの設定(秒)")]
    private float m_SetCoolTime;

    private float m_CoolTime;
    // Use this for initialization
    void Start()
    {
        m_AttackState = AttackState.Attack;
    }

    //void Update()
    //{

    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        switch (m_AttackState)
        {
            case AttackState.Attack:
                m_AttackCollider.SetActive(true);

                if (m_AttackTime > m_Timer) m_Timer += Time.deltaTime;
                else
                {
                    m_Timer = 0;
                    m_CoolTime = m_SetCoolTime;
                    m_AttackState = AttackState.AttackAfter;
                }
                break;

            case AttackState.AttackAfter:
                m_AttackCollider.SetActive(false);
                HumanRobotEnemy l_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();
                //nullが返ってきたら(視界外にいった)、見失い中に移行する
                if (l_HumanRobotEnemy.CheckPlayerAndRobot() == null)
                {
                    m_AttackState = AttackState.Attack;
                    m_CoolTime = 0.0f;
                    enemy.ChangeState(EnemyStatus.ChasingButLosed);
                    return;
                }
                Vector3 l_TargetPosition = l_HumanRobotEnemy.CheckPlayerAndRobot().transform.position;
                //クールタイムを調べる
                if (m_CoolTime >= 0) m_CoolTime -= deltaTime;


                //攻撃後のプレイヤーとの距離を測って、離れていたら追跡中に変更
                float distance = (Vector3.Distance(l_HumanRobotEnemy.GetEnemyPosition(), l_TargetPosition));
                if (distance < m_DistanceCompare)
                {
                    //近ければプレイヤーの方向を向いて攻撃
                    Vector3 relativePos = l_HumanRobotEnemy.CheckPlayerAndRobot().transform.position - l_HumanRobotEnemy.transform.position;
                    relativePos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    l_HumanRobotEnemy.transform.rotation = Quaternion.Slerp(l_HumanRobotEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

                    //自身の前方向とプレイヤーとの角度を調べる
                    if (Vector3.Angle(l_HumanRobotEnemy.transform.forward, relativePos) <= 1.5f && m_CoolTime <= 0)
                    {
                        m_AttackState = AttackState.Attack;
                    }
                }
                else
                {
                    enemy.ChangeState(EnemyStatus.Chasing);
                    l_HumanRobotEnemy.m_Agent.isStopped = false;
                    m_AttackState = AttackState.Attack;
                }
                break;
        }
    }
}