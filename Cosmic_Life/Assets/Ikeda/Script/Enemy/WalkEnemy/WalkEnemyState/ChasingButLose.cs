using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingButLose : EnemyState {

    private WalkEnemy m_WalkEnemy;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("追跡中(見失い中)");


        if (m_WalkEnemy == null)
            m_WalkEnemy = enemy.GetComponent<WalkEnemy>();

        //プレイヤーが見えた場合
        if (m_WalkEnemy.CanSeePlayerAndRobot())
        {
            m_WalkEnemy.m_Agent.destination = m_WalkEnemy.CheckPlayerAndRobot().transform.position;
            SoundManager.Instance.PlaySe("SE_Droid_Discovery");
            //追跡中に状態変更
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //プレイヤーを見つけられないまま目的地に到着
        else if (m_WalkEnemy.HasArrived())
        {
            if (m_WalkEnemy.GetIsPatrol())
            {
                //巡回中に状態遷移
                m_WalkEnemy.SetAngle(45.0f);
                enemy.ChangeState(EnemyStatus.RoundState);

            }
            else {
                //元の位置に戻る状態
                m_WalkEnemy.SetAngle(45.0f);
                enemy.ChangeState(EnemyStatus.ReturnPosition);

            }
        }

    }
}
