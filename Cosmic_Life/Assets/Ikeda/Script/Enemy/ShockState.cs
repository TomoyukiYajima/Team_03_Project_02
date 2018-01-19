using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockState : EnemyState {

    [SerializeField, Tooltip("ショック状態の時間の設定")]
    private float m_SetShockTime;

    private float m_Time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        if (m_SetShockTime >= m_Time)
        {
            m_Time += deltaTime;
        }
        else
        {
            enemy.ChangeState(enemy.GetStorageEnemystatus());
        }
    }
}
