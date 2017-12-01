using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEnemyAttack : MultOrder {

    //// Use this for initialization
    //public override void Start()
    //{
    //    base.Start();

    //    // 命令を取得して格納する
    //    //m_Orders[OrderStatus.MOVE] = 
    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        // 対象が敵でなければ、停止させる

        // 攻撃の開始
        //base.StartAction(obj, actionObj);

        // 目標の付近に辿り着いたら、攻撃処理を行う
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        //base.UpdateAction(deltaTime, obj, actionObj);

        // 移動が完了したら、攻撃に遷移

        // 攻撃を実行する
        Attack(deltaTime, obj, actionObj);
    }

    // 攻撃処理
    public void Attack(float deltaTime, GameObject obj, GameObject actionObj)
    {
        // 攻撃中
        //base.UpdateAction(deltaTime, obj, actionObj);
        m_MultOrders[OrderStatus.ATTACK].Update();

        // 攻撃命令の取得
        var orderAttack = m_MultOrders[OrderStatus.ATTACK].GetComponent<OrderAttack>();
        if (!orderAttack.IsAttackEnd()) return;

        // 攻撃終了後の処理
        // 攻撃命令の終了
        m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
        // 相手を倒していたら、停止状態に遷移する
        if (actionObj == null)
        {
            EndOrder(obj);
            return;
        }
        else
        {
            // 目標との角度差が大きければ、方向転換する
            //if()

            // 目標との距離が遠ければ、相手に対して移動する
            float length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
            if (length > 1.0f)
            {
                Worker worker = obj.GetComponent<Worker>();
                worker.ChangeAgentMovePoint(actionObj.transform.position);
                return;
            }
        }
    }

    // 命令の追加
    protected override void AddOrder()
    {
        // 命令を取得して、追加する
        m_MultOrders.Add(OrderStatus.MOVE, m_Undroid.GetOrder(OrderStatus.MOVE));
        m_MultOrders.Add(OrderStatus.TURN, m_Undroid.GetOrder(OrderStatus.TURN));
        m_MultOrders.Add(OrderStatus.ATTACK, m_Undroid.GetOrder(OrderStatus.ATTACK));
    }
}
