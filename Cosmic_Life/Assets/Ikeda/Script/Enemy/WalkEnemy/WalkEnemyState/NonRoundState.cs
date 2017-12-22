using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRoundState : EnemyState {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("巡回しない状態");

        WalkEnemy l_WalkEnemy = enemy.GetComponent<WalkEnemy>();
        l_WalkEnemy.m_Agent.isStopped = true;
        //見えている場合
        if (l_WalkEnemy.CanSeePlayerAndRobot())
        {
            l_WalkEnemy.m_Agent.isStopped = false;
            SoundManager.Instance.PlaySe("SE_Droid_Discovery");
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //見失った場合
        else
        {
        }
    }
}
