using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HumanRobotEnemySupport : EnemyState {

    HumanRobotEnemy m_HumanRobotEnemy;

    [SerializeField]
    private HumanEnemy m_HumanEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("援護状態");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        if (m_HumanEnemy == null)
        {
            m_HumanRobotEnemy.ChangeState(EnemyStatus.RunawayState);
            return;
        }

        m_HumanRobotEnemy.m_Agent.destination = m_HumanRobotEnemy.m_GuardPoint.transform.position;

        //ターゲットの方向を向く
        Quaternion rotation = Quaternion.LookRotation(m_HumanEnemy.transform.forward);
        m_HumanRobotEnemy.transform.rotation = Quaternion.Slerp(m_HumanRobotEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

        if (Vector3.Distance(m_HumanRobotEnemy.GetEnemyPosition(), m_HumanRobotEnemy.m_GuardPoint.transform.position) <= 1.0f)
        {
            m_HumanRobotEnemy.AgentStop();
        }
        else
        {
            m_HumanRobotEnemy.m_Agent.isStopped = false;
        }
    }
}
