using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyAnimationController : MonoBehaviour
{

    private Animator m_Animator;

    private HumanEnemy m_HumanEnemy;

    [SerializeField, Tooltip("AttackStateを設定")]
    private GameObject m_HumanAttack;

    [SerializeField, Tooltip("RoundStateを設定")]
    private GameObject m_HumanRound;

    [SerializeField, Tooltip("銃のオブジェクトの設定")]
    private GameObject m_HandGun;

    private HumanAttackState m_HumanAttackState;

    private bool m_Once;

    private bool m_IsHumanDead;

    private GameObject m_Gun;
    private Transform m_RHand;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_HumanEnemy = transform.parent.GetComponent<HumanEnemy>();
        m_Once = true;
        m_IsHumanDead = false;
        m_HumanAttackState = m_HumanAttack.GetComponent<HumanAttackState>();


        m_RHand = m_Animator.GetBoneTransform(HumanBodyBones.RightHand);
        m_Gun = Instantiate(m_HandGun, Vector3.zero, Quaternion.identity) as GameObject;
        //銃を手に持たせる
        HandGunHold();

        m_Animator.SetBool("IsWalkState", false);
    }

    // Update is called once per frame
    void Update()
    {
        m_Gun.transform.rotation = m_RHand.rotation * Quaternion.Euler(new Vector3(-85f, 0f, 90f));


        if (m_HumanEnemy.GetEnemyStatus() == EnemyStatus.RoundState && m_Once && m_HumanRound.GetComponent<HumanRoundState>().GetIsRound())
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

    public void HandGunHold()
    {
        m_Gun.transform.localScale *= 0.2f;
        m_Gun.transform.SetParent(m_RHand);
        m_Gun.transform.position = m_RHand.position + new Vector3(0.05f, -0.05f, 0.4f);
    }
}
