using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyAnimationController : MonoBehaviour
{

    private Animator m_Animator;

    private HumanEnemy m_HumanEnemy;

    [SerializeField, Tooltip("AttackStateを設定")]
    private GameObject m_HumanAttack;

    private HumanAttackState m_HumanAttackState;

    private bool m_Once;

    private bool m_IsHumanDead;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_HumanEnemy = transform.parent.GetComponent<HumanEnemy>();
        m_Once = true;
        m_IsHumanDead = false;
        m_HumanAttackState = m_HumanAttack.GetComponent<HumanAttackState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HumanEnemy.GetEnemyStatus() == EnemyStatus.RoundState && m_Once)
        {
            m_Once = false;
            m_Animator.SetBool("IsWalkState", true);
            m_Animator.SetBool("IsAttackIdle", false);
            m_Animator.SetBool("IsFire", false);
        }
        else if (m_HumanEnemy.GetEnemyStatus() == EnemyStatus.Attack &&
                 m_HumanAttackState.GetAttackState() == HumanAttackState.AttackState.CoolTime && !m_Once)
        {
            m_Once = true;
            m_Animator.SetBool("IsWalkState", false);
            m_Animator.SetBool("IsAttackIdle", true);
            m_Animator.SetBool("IsFire", false);
        }
        else if (m_HumanEnemy.GetEnemyStatus() == EnemyStatus.Attack &&
                 m_HumanAttackState.GetAttackState() == HumanAttackState.AttackState.Attack && m_Once)
        {
            m_Once = false;
            m_Animator.SetBool("IsWalkState", false);
            m_Animator.SetBool("IsFire", true);
            m_Animator.SetBool("IsAttackIdle", false);
        }
        else if (m_HumanEnemy.IsDead() && !m_IsHumanDead)
        {
            m_IsHumanDead = true;
            m_Animator.SetTrigger("Dead");
        }
    }
}
