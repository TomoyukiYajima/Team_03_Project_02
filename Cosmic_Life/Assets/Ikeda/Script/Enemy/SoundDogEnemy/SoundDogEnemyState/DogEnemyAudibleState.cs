using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyAudibleState : EnemyState
{

    SoundDogEnemy m_SoundDogEnemy;

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
        if (m_SoundDogEnemy == null) m_SoundDogEnemy = enemy.GetComponent<SoundDogEnemy>();

        m_SoundDogEnemy.m_Agent.destination = m_SoundDogEnemy.GetTargetPosition();

        if (m_SoundDogEnemy.CanSeePlayerAndRobot())
        {
            //m_SoundDogEnemy.SetAngle(180);
            SoundManager.Instance.PlaySe("SE_Droid_Discovery");
            m_SoundDogEnemy.ChangeState(EnemyStatus.Chasing);
        }
        else if (m_SoundDogEnemy.HasArrived())
        {
            m_SoundDogEnemy.SetNewPatrolPointToDestination();
            m_SoundDogEnemy.ChangeState(EnemyStatus.RoundState);
        }
    }
}
