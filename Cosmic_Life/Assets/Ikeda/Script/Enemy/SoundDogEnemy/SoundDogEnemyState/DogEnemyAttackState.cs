using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyAttackState : EnemyState {

    private enum AttackState
    {
        Attack,
        AttackAfter,

        None
    }

    //攻撃の距離を設定
    [SerializeField]
    private float m_PlayerStopDistance;

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

    SoundDogEnemy m_SoundDogEnemy;

    // Use this for initialization
    void Start()
    {
        m_AttackState = AttackState.Attack;
        m_DistanceCompare = m_PlayerStopDistance;
    }

    //void Update()
    //{
    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_SoundDogEnemy == null) m_SoundDogEnemy = enemy.GetComponent<SoundDogEnemy>();

        switch (m_AttackState)
        {
            case AttackState.Attack:
                m_AttackCollider.SetActive(true);
                //SoundManager.Instance.PlaySe("SE_Droid_Attack_01");

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
                if (m_SoundDogEnemy == null)
                    m_SoundDogEnemy = enemy.GetComponent<SoundDogEnemy>();
                //NULLだったら状態を変更
                m_TargetObject = m_SoundDogEnemy.CheckPlayer();
                if (m_TargetObject == null)
                {
                    m_SoundDogEnemy.m_Agent.isStopped = false;
                    m_SoundDogEnemy.SetNewPatrolPointToDestination();
                    m_SoundDogEnemy.ChangeState(EnemyStatus.RoundState);
                    return;
                }

                //クールタイムを調べる
                if (m_CoolTime >= 0) m_CoolTime -= deltaTime;

                //CheckStopDistance(enemy);

                //攻撃後のプレイヤーとの距離を測って、離れていたら追跡中に変更
                float distance = (Vector3.Distance(m_SoundDogEnemy.GetEnemyPosition(), m_TargetObject.transform.position));
                if (distance < m_DistanceCompare)
                {
                    //近ければプレイヤーの方向を向いて攻撃
                    Vector3 relativePos = m_SoundDogEnemy.CheckPlayer().transform.position - m_SoundDogEnemy.transform.position;
                    relativePos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    m_SoundDogEnemy.transform.rotation = Quaternion.Slerp(m_SoundDogEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

                    //自身の前方向とプレイヤーとの角度を調べる
                    if (Vector3.Angle(m_SoundDogEnemy.transform.forward, relativePos) <= 1.5f && m_CoolTime <= 0)
                    {
                        m_AttackState = AttackState.Attack;
                    }
                }
                else
                {
                    m_AttackState = AttackState.Attack;
                    m_SoundDogEnemy.m_Agent.isStopped = false;
                    enemy.ChangeState(EnemyStatus.Chasing);
                }
                break;
        }
    }


    //private void CheckStopDistance(Enemy enemy)
    //{
    //    if (enemy.GetComponent<WalkEnemy>().CheckPlayerAndRobot() == null) return;

    //    if (enemy.GetComponent<WalkEnemy>().CheckPlayerAndRobot().transform.tag == "Player")
    //    {
    //        m_DistanceCompare = m_PlayerStopDistance;
    //    }
    //    else
    //    {
    //        m_DistanceCompare = m_RobotStopDistance;
    //    }
    //}
}
