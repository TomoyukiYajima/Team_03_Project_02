using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyAnimatorController : MonoBehaviour
{

    HumanRobotEnemy m_HumanRobotEnemy;
    Animator m_Animator;

    [SerializeField, Tooltip("AttackStateを設定")]
    private GameObject m_AttackState;

    HumanRobotEnemyAttack m_HumanRobotEnemyAttack;

    private bool m_Once;

    private bool m_IsDie;

    private int m_Count;


    // Use this for initialization
    void Start()
    {
        m_HumanRobotEnemy = transform.parent.GetComponent<HumanRobotEnemy>();
        m_Animator = GetComponent<Animator>();
        m_HumanRobotEnemyAttack = m_AttackState.GetComponent<HumanRobotEnemyAttack>();
        m_Once = false;
        m_IsDie = false;
        m_Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.RoundState || m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.ReturnPosition ||
            m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.Chasing || m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.ChasingButLosed ||
            m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.SupportState && m_Once)
        {
            m_Once = false;
            m_Animator.SetBool("IsWalk", true);
            m_Animator.SetBool("IsIdle", false);
        }
        else if (m_HumanRobotEnemyAttack.GetHumanRobotAttackState() == HumanRobotEnemyAttack.AttackState.Attack &&
                 m_HumanRobotEnemyAttack.GetIsAttack() == true && !m_Once)
        {
            m_Count++;
            if (m_Count > 1) return;
            m_Once = true;
            m_Animator.SetTrigger("Attack");
            m_Animator.SetBool("IsWalk", false);
            m_Animator.SetBool("IsIdle", true);
        }
        else if (m_HumanRobotEnemy.IsDead())
        {
            if (!m_IsDie)
            {
                m_IsDie = true;
                m_Animator.SetTrigger("Die");
            }
        }
        else if (m_HumanRobotEnemy.GetEnemyStatus() == EnemyStatus.NonRoundState)
        {
            m_Animator.SetBool("IsIdle", true);
            m_Animator.SetBool("IsWalk", false);
        }
        else
        {
            m_Once = false;
            m_Animator.SetBool("IsIdle", false);
            if (m_Count >= 2)
            {
                m_Count = 0;
            }
        }
    }
}
