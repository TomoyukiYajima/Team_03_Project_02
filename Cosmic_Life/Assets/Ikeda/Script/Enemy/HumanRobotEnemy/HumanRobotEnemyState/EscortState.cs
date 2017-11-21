using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortState : EnemyState {

    HumanRobotEnemy m_HumanRobotEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人Robot(Enemy)護衛中");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        m_HumanRobotEnemy.m_Agent.isStopped = true;

        //プレイヤーかロボットを見つけた場合
        if (m_HumanRobotEnemy.CanSeePlayerAndRobot())
        {
            m_HumanRobotEnemy.m_Agent.isStopped = false;
            m_HumanRobotEnemy.ChangeState(EnemyStatus.Chasing);
        }
    }
}
