using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyAttack : EnemyState {

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
    private float m_AttackTime = 15.0f; 

    private AttackState m_AttackState;

    private float m_Timer = 0.0f;

	// Use this for initialization
	void Start () {
        m_AttackState = AttackState.Attack;
	}

    //void Update()
    //{

    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        switch(m_AttackState)
        {
            case AttackState.Attack:
                m_AttackCollider.SetActive(true);

                if (m_AttackTime > m_Timer) m_Timer += Time.deltaTime;
                else
                {
                    m_Timer = 0;
                    m_AttackState = AttackState.AttackAfter;
                }
                break;

            case AttackState.AttackAfter:
                m_AttackCollider.SetActive(false);
                WalkEnemy l_WalkEnemy = enemy.GetComponent<WalkEnemy>();
                Vector3 l_TargetPosition = l_WalkEnemy.CheckPlayerAndRobot().transform.position;


                //攻撃後のプレイヤーとの距離を測って、離れていたら追跡中に変更
                float distance = (Vector3.Distance(l_WalkEnemy.GetEnemyPosition(), l_TargetPosition));
                if (distance < m_DistanceCompare)
                {
                    //近ければプレイヤーの方向を向いて攻撃
                    Vector3 relativePos = l_WalkEnemy.CheckPlayerAndRobot().transform.position - l_WalkEnemy.transform.position;
                    relativePos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    l_WalkEnemy.transform.rotation = Quaternion.Slerp(l_WalkEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

                    //自身の前方向とプレイヤーとの角度を調べる
                    if (Vector3.Angle(l_WalkEnemy.transform.forward, relativePos) <= 1.5f)
                    {
                        m_AttackState = AttackState.Attack;
                    }
                }
                else
                {
                    enemy.ChangeState(EnemyStatus.Chasing);
                    l_WalkEnemy.m_Agent.isStopped = false;
                    m_AttackState = AttackState.Attack;
                }
                break;
        }
    }
}
