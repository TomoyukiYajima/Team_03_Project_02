using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyChasing : EnemyState {

    HumanRobotEnemy m_HumanRobotEnemy;

    //攻撃を当てる距離の設定
    [SerializeField]
    private float m_PlayerStopDistance;
    //攻撃を当てる距離の設定
    [SerializeField]
    private float m_RobotStopDistance;

    private float m_StopDistance;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人Robot(Enemy)追跡中");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        //プレイヤーかロボットを見つけた場合
        if (m_HumanRobotEnemy.CanSeePlayerAndRobot())
        {
            m_HumanRobotEnemy.m_Agent.destination = m_HumanRobotEnemy.CheckPlayerAndRobot().transform.position;

            //ターゲットとの距離
            float distance = Vector3.Distance(m_HumanRobotEnemy.transform.position, m_HumanRobotEnemy.CheckPlayerAndRobot().transform.position);

            //ターゲットによって止まる距離を決める
            CheckStopDistance(enemy);

            //ターゲットとの距離を調べて、近ければ攻撃状態へ
            if (distance < m_StopDistance)
            {
                m_HumanRobotEnemy.AgentStop();
                enemy.ChangeState(EnemyStatus.Attack);
            }
        }
        //見失った場合
        else
        {
            //追跡中(見失い)に状態変更
            enemy.ChangeState(EnemyStatus.ChasingButLosed);
        }

    }

    private void CheckStopDistance(Enemy enemy)
    {
        if (enemy.GetComponent<HumanRobotEnemy>().CheckPlayerAndRobot().transform.tag == "Player")
        {
            m_StopDistance = m_PlayerStopDistance;
        }
        else
        {
            m_StopDistance = m_RobotStopDistance;
        }
    }
}

