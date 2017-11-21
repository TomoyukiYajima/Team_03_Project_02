using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyChasingButLose : EnemyState
{

    HumanRobotEnemy m_HumanRobotEnemy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人Robot(Enemy)見失い中");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        //が見えた場合
        if (m_HumanRobotEnemy.CanSeePlayerAndRobot())
        {
            m_HumanRobotEnemy.m_Agent.destination = m_HumanRobotEnemy.CheckPlayerAndRobot().transform.position;
            //追跡中に状態変更
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //プレイヤーを見つけられないまま目的地に到着
        else
        {
            //巡回中に状態遷移
            enemy.ChangeState(EnemyStatus.RoundState);
        }
    }
}
