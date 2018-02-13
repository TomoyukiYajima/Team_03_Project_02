using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDogEnemyAnimatorController : MonoBehaviour {

    private SoundDogEnemy m_DogEnemy;

    private Animator m_Animator;

    [SerializeField]
    private GameObject m_AttackObj;

    private DogEnemyAttackState m_AttackState;

    private bool m_Once;

    private int m_Count;

    // Use this for initialization
    void Start () {
        m_DogEnemy = transform.parent.GetComponent<SoundDogEnemy>();
        m_Animator = GetComponent<Animator>();
        m_AttackState = m_AttackObj.GetComponent<DogEnemyAttackState>();
        m_Once = false;	
	}
	
	// Update is called once per frame
	void Update () {

        if (m_DogEnemy.GetEnemyStatus() == EnemyStatus.RoundState || m_DogEnemy.GetEnemyStatus() == EnemyStatus.Chasing ||
            m_DogEnemy.GetEnemyStatus() == EnemyStatus.ChasingButLosed || m_DogEnemy.GetEnemyStatus() == EnemyStatus.AudibleState)
        {
            m_Animator.SetBool("IsWalk", true);
            m_Animator.SetBool("IsIdle", false);
        }
        else if (m_DogEnemy.GetEnemyStatus() == EnemyStatus.Attack && !m_AttackState.GetIsAttack())
        {
            m_Animator.SetTrigger("IsAttack");
        }
        else if (m_DogEnemy.GetEnemyStatus() == EnemyStatus.Attack)
        {
            m_Animator.SetBool("IsWalk", false);
            m_Animator.SetBool("IsIdle", true);
        }
        else
        {
            m_Animator.SetBool("IsWalk", false);
            m_Animator.SetBool("IsIdle", true);
        }
    }
}
