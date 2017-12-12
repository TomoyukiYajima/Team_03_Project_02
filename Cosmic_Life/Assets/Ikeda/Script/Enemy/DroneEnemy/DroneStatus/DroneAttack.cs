using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttack : EnemyState
{
    DroneEnemy m_DroneEnemy;
    GameObject m_Collide;
    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_DroneEnemy == null) m_DroneEnemy = enemy.GetComponent<DroneEnemy>();

        Vector3 relativePos = m_DroneEnemy.transform.position - m_DroneEnemy.transform.position;
        m_DroneEnemy.transform.Translate(relativePos * m_DroneEnemy.m_Speed * Time.deltaTime, Space.World);

    }
}
