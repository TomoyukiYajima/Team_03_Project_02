using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyRunaway : EnemyState {

    HumanRobotEnemy m_HumanRobotEnemy;

    [SerializeField]
    GameObject m_Collider;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人Robot(Enemy)暴走状態");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        m_Collider.SetActive(true);
        m_HumanRobotEnemy.m_Agent.isStopped = false;
        m_HumanRobotEnemy.m_Agent.destination = m_HumanRobotEnemy.GetPlayer().transform.position;
    }
}
