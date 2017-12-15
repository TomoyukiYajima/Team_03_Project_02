using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRoundState : EnemyState
{
    [SerializeField, Tooltip("巡回するか？")]
    private bool m_IsRound;


	// Use this for initialization
	void Start () {
    
    }

    // Update is called once per frame
    void Update () {

	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("人(Enemy)巡回中");

        HumanEnemy l_HumanEnemy = enemy.GetComponent<HumanEnemy>();

        //巡回する状態
        if (m_IsRound)
        {
            //プレイヤーが見えた場合
            if (l_HumanEnemy.CanSeePlayer())
            {
                l_HumanEnemy.m_Agent.isStopped = true;
                l_HumanEnemy.ChangeState(EnemyStatus.Attack);
            }
            //プレイヤーが見えなくて、目的地に到着した場合
            else if (l_HumanEnemy.HasArrived())
            {
                //目的地を次の巡回ポイントに切り替える(ランダムで)
                l_HumanEnemy.SetNewPatrolPointToDestination();
            }
        }
        else
        {
            //プレイヤーが見えた場合
            if (l_HumanEnemy.CanSeePlayer())
            {
                l_HumanEnemy.ChangeState(EnemyStatus.Attack);
            }

        }
    }

    public bool GetIsRound()
    {
        return m_IsRound;
    }
}
