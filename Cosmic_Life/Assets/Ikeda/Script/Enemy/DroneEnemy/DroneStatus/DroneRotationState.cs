using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRotationState : EnemyState
{

    private DroneEnemy m_DroneEnemy;

    [SerializeField, Tooltip("回転の速さの設定")]
    private float m_AngleSpeed;

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
        if (m_DroneEnemy == null) m_DroneEnemy = enemy.GetComponent<DroneEnemy>();

        m_DroneEnemy.transform.Rotate(new Vector3(0, 1, 0), m_AngleSpeed);

        m_DroneEnemy.ChangeColor();

        if (m_DroneEnemy.IsSeePlayer() || m_DroneEnemy.IsSeeRobot())
        {
            m_DroneEnemy.ChangeRedColor();
            m_DroneEnemy.ChangeState(EnemyStatus.Attack);
        }
    }
}