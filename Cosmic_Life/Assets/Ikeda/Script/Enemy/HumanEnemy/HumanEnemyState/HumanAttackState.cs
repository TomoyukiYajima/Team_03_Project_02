using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackState : EnemyState
{
    public enum AttackState
    {
        Attack,
        CoolTime,

        None
    }


    [SerializeField, Tooltip("衝突判定オブジェクト")]
    private GameObject m_AttackCollision;

    [SerializeField]
    private Transform m_Muzzle;

    [SerializeField, Tooltip("弾速の設定")]
    private float m_BulletSpeed;

    [SerializeField, Tooltip("クールタイムの設定(秒)")]
    private float m_SetCoolTime;

    private float m_CoolTime;

    private AttackState m_AttackState;

    // Use this for initialization
    void Start()
    {
        m_AttackState = AttackState.Attack;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public AttackState GetAttackState()
    {
        return m_AttackState;
    }

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人(Enemy)攻撃");
        HumanEnemy l_HumanEnemy = enemy.GetComponent<HumanEnemy>();

        //プレイヤーが見えた場合
        if (l_HumanEnemy.CanSeePlayer())
        {
            if (m_AttackState == AttackState.Attack)
            {
                GameObject bullets = Instantiate(m_AttackCollision) as GameObject;

                Vector3 force = l_HumanEnemy.transform.forward * m_BulletSpeed * deltaTime;

                bullets.transform.position = m_Muzzle.transform.position;

                bullets.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                //クールタイムを入れる
                m_CoolTime = m_SetCoolTime;

                //クールタイム状態にする
                m_AttackState = AttackState.CoolTime;
            }
            else if (m_AttackState == AttackState.CoolTime)
            {
                //クールタイムを調べる
                if (m_CoolTime >= 0) m_CoolTime -= deltaTime;
                else m_AttackState = AttackState.Attack;

                //向いている方向を調べる
                float angle = Quaternion.Angle(l_HumanEnemy.transform.rotation, l_HumanEnemy.GetPlayer().transform.rotation);
                if (angle >= 1.0f)
                {
                    //その方向を向いて攻撃する
                    Vector3 relativePos = l_HumanEnemy.GetPlayer().transform.position - l_HumanEnemy.transform.position;
                    relativePos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    l_HumanEnemy.transform.rotation = Quaternion.Slerp(l_HumanEnemy.transform.rotation, rotation, Time.deltaTime * 1.5f);
                }
            }
        }
        //プレイヤーが視野角から外れた場合
        else
        {
            l_HumanEnemy.m_Agent.isStopped = false;
            transform.parent.FindChild("RoundState").GetComponent<HumanRoundState>().SetRound(true);
            l_HumanEnemy.ChangeState(EnemyStatus.RoundState);
        }
    }
}
