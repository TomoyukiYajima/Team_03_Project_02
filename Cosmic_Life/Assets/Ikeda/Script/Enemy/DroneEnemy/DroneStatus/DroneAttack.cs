using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttack : EnemyState
{
    enum DroneAttackState
    {
        ChaseState,
        Explosion
    }

    private DroneAttackState m_AttackState;
    private DroneEnemy m_DroneEnemy;

    [SerializeField]
    private GameObject m_Collide;

    [SerializeField, Tooltip("自爆攻撃のParticle設定")]
    private GameObject m_DroneExplosion;

    [SerializeField, Tooltip("止まる距離の設定")]
    private float m_StopDistance = 3.5f;
    
    [SerializeField, Tooltip("爆発するまでの時間")]
    private float m_SetExplosionTime;
    private float m_Timer;


    // Use this for initialization
    void Start () {
        m_AttackState = DroneAttackState.ChaseState;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //IEnumerator Explosion()
    //{
    //    m_Collide.SetActive(true);

    //    yield return new WaitForSeconds(0.5f);

    //    Destroy(m_DroneEnemy.gameObject);
    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_DroneEnemy == null) m_DroneEnemy = enemy.GetComponent<DroneEnemy>();

        if (m_AttackState == DroneAttackState.ChaseState)
        {
            if (Vector3.Distance(m_DroneEnemy.transform.position, m_DroneEnemy.GetTarget().transform.position) > m_StopDistance)
            {
                Vector3 relativePos = (m_DroneEnemy.GetTarget().transform.position + m_DroneEnemy.transform.up * 1.0f) - m_DroneEnemy.transform.position;
                m_DroneEnemy.transform.Translate(relativePos.normalized * m_DroneEnemy.m_Speed * deltaTime, Space.World);

                relativePos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                m_DroneEnemy.transform.rotation = Quaternion.Slerp(m_DroneEnemy.transform.rotation, rotation, deltaTime * 2.0f);
            }
            else
            {
                m_DroneEnemy.m_Speed = 0.0f;
                m_AttackState = DroneAttackState.Explosion;
            }
        }
        else if (m_AttackState == DroneAttackState.Explosion)
        {
            if (m_SetExplosionTime > m_Timer) m_Timer += deltaTime;
            
            else
            {
                Instantiate(m_Collide, m_DroneEnemy.transform.position, m_DroneEnemy.transform.rotation);
                Instantiate(m_DroneExplosion, m_DroneEnemy.transform.position, m_DroneEnemy.transform.rotation);
                Destroy(m_DroneEnemy.gameObject);
            }
        }
    }
}
