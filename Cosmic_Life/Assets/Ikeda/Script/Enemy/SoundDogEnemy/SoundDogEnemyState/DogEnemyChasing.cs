using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyChasing : EnemyState {

    SoundDogEnemy m_SoundDogEnemy;

    //攻撃を当てる距離の設定
    [SerializeField]
    private float m_PlayerStopDistance;
    //攻撃を当てる距離の設定
    //[SerializeField]
    //private float m_RobotStopDistance;

    private float m_StopDistance;

    // Use this for initialization
    void Start () {
        m_StopDistance = m_PlayerStopDistance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_SoundDogEnemy == null) m_SoundDogEnemy = enemy.GetComponent<SoundDogEnemy>();

        if (m_SoundDogEnemy.CanSeePlayerAndRobot())
        {
            //ターゲットの場所へ向かう
            m_SoundDogEnemy.m_Agent.destination = m_SoundDogEnemy.CheckPlayer().transform.position;
            //ターゲットとの距離
            float distance = Vector3.Distance(m_SoundDogEnemy.GetEnemyPosition(), m_SoundDogEnemy.CheckPlayer().transform.position);

            //ターゲットによって止まる距離を決める
            //CheckStopDistance(enemy);

            //ターゲットとの距離を調べて、近ければ攻撃状態へ
            if (distance < m_StopDistance)
            {
                m_SoundDogEnemy.AgentStop();
                m_SoundDogEnemy.ChangeState(EnemyStatus.Attack);
            }
        }
        else if (m_SoundDogEnemy.GetIsHear())
        {
            m_SoundDogEnemy.m_IsHear = false;
            m_SoundDogEnemy.SetTargetPosition(enemy.GetPlayer().transform.position);
            m_SoundDogEnemy.ChangeState(EnemyStatus.AudibleState);
        }
        //見失った場合
        else
        {
            m_SoundDogEnemy.SetNewPatrolPointToDestination();
            //追跡中(見失い)に状態変更
            enemy.ChangeState(EnemyStatus.RoundState);
        }
    }

    //private void CheckStopDistance(Enemy enemy)
    //{
    //    GameObject player = enemy.GetComponent<SoundDogEnemy>().CheckPlayer();
    //    if (player.transform.tag == "Player")
    //    {
    //        m_StopDistance = m_PlayerStopDistance;
    //    }
    //    else
    //    {
    //        m_StopDistance = m_RobotStopDistance;
    //    }
    //}
}