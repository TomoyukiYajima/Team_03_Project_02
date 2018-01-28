using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyAnimatorController : MonoBehaviour {

    WalkEnemy m_WalkEnemy;
    Animator m_Animator;

    [SerializeField, Tooltip("AttackStateを設定")]
    private GameObject m_AttackState;

    WalkEnemyAttack m_WalkEnemyAttack;

    private bool m_Once;

    private bool m_IsDie;

    private int m_Count;


	// Use this for initialization
	void Start () {
        m_WalkEnemy = transform.parent.GetComponent<WalkEnemy>();
        m_Animator = GetComponent<Animator>();
        m_WalkEnemyAttack = m_AttackState.GetComponent<WalkEnemyAttack>();
        m_Once = false;
        m_IsDie = false;
        m_Count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_WalkEnemy.GetEnemyStatus() == EnemyStatus.RoundState || m_WalkEnemy.GetEnemyStatus() == EnemyStatus.ReturnPosition ||
            m_WalkEnemy.GetEnemyStatus() == EnemyStatus.Chasing || m_WalkEnemy.GetEnemyStatus() == EnemyStatus.ChasingButLosed && m_Once)
        {
            m_Once = false;
            m_Animator.SetBool("IsWalk", true);
            m_Animator.SetBool("IsIdle", false);
        }
        else if (m_WalkEnemyAttack.GetWalkEnemyState() == WalkEnemyAttack.AttackState.Attack &&
                 m_WalkEnemyAttack.GetIsAttack() == true && !m_Once)
        {
            m_Count++;
            if (m_Count > 1) return;
            m_Once = true;
            m_Animator.SetTrigger("Attack");
            m_Animator.SetBool("IsWalk", false);
            m_Animator.SetBool("IsIdle", true);
        }
        else if (m_WalkEnemy.IsDead())
        {
            if (!m_IsDie)
            {
                m_IsDie = true;
                m_Animator.SetTrigger("Die");
            }
        }
        else if (m_WalkEnemy.GetEnemyStatus() == EnemyStatus.NonRoundState)
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
