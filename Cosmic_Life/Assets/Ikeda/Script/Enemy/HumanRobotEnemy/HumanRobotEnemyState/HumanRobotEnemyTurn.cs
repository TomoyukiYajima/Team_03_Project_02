using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRobotEnemyTurn : EnemyState {

    HumanRobotEnemy m_HumanRobotEnemy;

    [SerializeField]
    HumanEnemy m_HumanEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人Robot(Enemy)向く");

        if (m_HumanRobotEnemy == null)
            m_HumanRobotEnemy = enemy.GetComponent<HumanRobotEnemy>();

        //ターゲットの方向を向く
        Vector3 relativePos = m_HumanEnemy.GetTargetPosition() - m_HumanRobotEnemy.transform.position;
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        m_HumanRobotEnemy.transform.rotation = Quaternion.Slerp(m_HumanRobotEnemy.transform.rotation, rotation, Time.deltaTime * 2.0f);

        //視界に入ったら追跡中にする
        if (m_HumanRobotEnemy.CanSeePlayerAndRobot())
        {
            m_HumanRobotEnemy.ChangeState(EnemyStatus.Chasing);
        }
    }
}
