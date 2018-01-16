using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyRoundState : EnemyState {

    SoundDogEnemy m_SoundDogEnemy;
    Vector3 m_TargetPosition;

	// Use this for initialization
	void Start () {
        m_TargetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_SoundDogEnemy == null) m_SoundDogEnemy = enemy.GetComponent<SoundDogEnemy>();

        //見えた時
        if (m_SoundDogEnemy.CanSeePlayerAndRobot())
        {
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //聞こえた場合
        else if (m_SoundDogEnemy.GetIsHear())
        {
            m_SoundDogEnemy.m_IsHear = false;
            m_SoundDogEnemy.SetTargetPosition(enemy.GetPlayer().transform.position);
            enemy.ChangeState(EnemyStatus.AudibleState);
        }
        //見えなくて、目的地に到着した場合
        else if (m_SoundDogEnemy.HasArrived())
        {
            //目的地を次の巡回ポイントに切り替える
            m_SoundDogEnemy.SetNewPatrolPointToDestination();
        }
    }
}
