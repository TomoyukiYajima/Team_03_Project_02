using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRoundState : EnemyState
{

    private int m_RoutePosCount = 0;
    private DroneEnemy m_DroneEnemy;

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

        Vector3 relativePos = m_DroneEnemy.GetRoundPoints()[m_RoutePosCount].transform.position - m_DroneEnemy.transform.position;
        m_DroneEnemy.transform.Translate(relativePos.normalized * m_DroneEnemy.m_Speed * Time.deltaTime, Space.World);
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        m_DroneEnemy.transform.rotation = Quaternion.Slerp(m_DroneEnemy.transform.rotation, rotation, Time.deltaTime * 1.0f);
        if (Vector3.Distance(m_DroneEnemy.transform.position, m_DroneEnemy.GetRoundPoints()[m_RoutePosCount].transform.position) <= 0.15f)
        {
            ArrivedProcessing(enemy);
        }

        m_DroneEnemy.ChangeColor();

        if (m_DroneEnemy.IsSeePlayer() || m_DroneEnemy.IsSeeRobot())
        {
            m_DroneEnemy.ChangeRedColor();
            m_DroneEnemy.ChangeState(EnemyStatus.Attack);
        }
    }

    private void ArrivedProcessing(Enemy enemy)
    {
        if (m_RoutePosCount + 1 < m_DroneEnemy.GetRoundPoints().Length)
        {
            m_RoutePosCount++;
            /* 何も入ってなかったときに0に戻す */
            if (m_DroneEnemy.GetRoundPoints()[m_RoutePosCount] == null) m_RoutePosCount = 0;
        }
        else
        {
            m_RoutePosCount = 0;
        }
    }
}
