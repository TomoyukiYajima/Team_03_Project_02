using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEnemyAttack : MultOrder {

    private bool m_IsAttack = false;

    private bool m_IsRotate = false;
    // 到着したか
    private bool m_IsGoal = false; 

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // 命令を取得して格納する
        //m_Orders[OrderStatus.MOVE] = 
    }

    //// Update is called once per frame
    //void Update () {

    //}

    //public override void Start

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        m_IsAttack = false;
        m_IsGoal = false;

        // 対象が敵でなければ、停止させる
        if (actionObj == null || actionObj.tag != "Enemy")
        {
            // 命令失敗
            FaildOrder(obj);
            return;
        }

        if (m_Undroid == null) m_Undroid = obj.GetComponent<Worker>();
        // 命令リストの追加
        AddOrder();

        // 攻撃の開始
        //base.StartAction(obj, actionObj);
        // 移動の開始
        ChangeOrder(obj, OrderStatus.MOVE);
        m_ActionObject = actionObj;
        SetStartOrderText();
        // 目標の付近に辿り着いたら、攻撃処理を行う
        //AddOrder();
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        UpdateAction(deltaTime, obj, m_ActionObject);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        if (actionObj == null)
        {
            // 攻撃命令の取得
            var orderAttack = m_MultOrders[OrderStatus.ATTACK].GetComponent<OrderAttack>();
            if (!orderAttack.IsAttackEnd()) return;
            m_MultOrders[OrderStatus.MOVE].EndOrder(obj);
            m_MultOrders[OrderStatus.TURN].EndOrder(obj);
            m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
            EndOrder(obj);
            return;
        }

        if (m_IsAttack)
        {
            Attack(deltaTime, obj, actionObj);
            //return;
        }

        var length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
        if (length < 2.0f)
        {
            // 角度の計算
            var pos = actionObj.transform.position;
            pos.y = obj.transform.position.y;
            var dir = actionObj.transform.position - obj.transform.position;
            var cross = Vector3.Cross(obj.transform.forward, dir);
            cross = cross.normalized;
            var degree = Mathf.Atan2(dir.z, dir.x) * 180 / Mathf.PI;
            degree += obj.transform.forward.z * 270;
            if (degree < 0.0f) degree += 360;
            if (degree > 360) degree -= 360;

            m_IsGoal = true;

            // cross.y < 0.0f 左
            if (Mathf.Abs(cross.y) > 0.05f && (degree > 10.0f || degree < -10.0f))
            {
                if (!m_IsRotate)
                {
                    //OrderDirection orderDir = OrderDirection.LEFT;
                    //// 方向の指定
                    //if (rad > 0.0f) orderDir = OrderDirection.RIGHT;
                    // 
                    SetActionObj(obj, actionObj);
                    // 回転命令
                    ChangeOrder(obj, OrderStatus.TURN, OrderDirection.LEFT);
                    m_IsRotate = true;
                }

                //Attack(deltaTime, obj, actionObj);
                if (!m_IsAttack)
                {
                    ChangeOrder(obj, OrderStatus.ATTACK);
                    // 攻撃
                    m_IsAttack = true;
                    //return;
                }
                //// 攻撃
                //m_IsAttack = true;

                return;
            }

            ////Attack(deltaTime, obj, actionObj);
            if (!m_IsAttack)
            {
                ChangeOrder(obj, OrderStatus.ATTACK);
                m_IsRotate = false;
                m_IsAttack = true;
                return;
            }
            // 攻撃
            //m_IsAttack = true;
            //return;
        }
        else
        {
            if (m_IsGoal)
            {
                m_MultOrders[OrderStatus.TURN].EndOrder(obj);
                m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
                // 移動再開
                ChangeOrder(obj, OrderStatus.MOVE);
                m_IsGoal = false;
            }
        }
        //base.UpdateAction(deltaTime, obj, actionObj);

        // 移動が完了したら、攻撃に遷移
        //m_MultOrders[OrderStatus.MOVE].Update();

        // 攻撃を実行する
        //Attack(deltaTime, obj, actionObj);
    }

    // 攻撃処理
    public void Attack(float deltaTime, GameObject obj, GameObject actionObj)
    {
        // 攻撃中
        //base.UpdateAction(deltaTime, obj, actionObj);
        //m_MultOrders[OrderStatus.ATTACK].Update();

        // 攻撃命令の取得
        var orderAttack = m_MultOrders[OrderStatus.ATTACK].GetComponent<OrderAttack>();
        if (!orderAttack.IsAttackEnd()) return;

        // 呼ばれていない
        // 攻撃終了後の処理
        // 攻撃命令の終了
        m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
        // 相手を倒していたら、停止状態に遷移する
        if (actionObj == null)
        {
            m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
            EndOrder(obj);
            return;
        }
        else
        {
            // 目標との距離が遠ければ、相手に対して移動する
            float length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
            if (length > 2.0f)
            {
                Worker worker = obj.GetComponent<Worker>();
                worker.ChangeAgentMovePoint(actionObj.transform.position);
            }
            //m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);

            m_IsAttack = false;
            m_IsRotate = false;
            return;
        }

        //// 角度の計算
        //// 相手との距離を求める
        //var pos = actionObj.transform.position;
        //pos.y = obj.transform.position.y;
        //var dir = actionObj.transform.position - obj.transform.position;
        //var cross = Vector3.Cross(obj.transform.forward, dir);
        //cross = cross.normalized;
        //var degree = Mathf.Atan2(dir.z, dir.x) * 180 / Mathf.PI;
        //degree += obj.transform.forward.z * 270;
        //if (degree < 0.0f) degree += 360;
        //if (degree > 360) degree -= 360;

        //// cross.y < 0.0f 左
        //if (Mathf.Abs(cross.y) > 0.05f && (degree > 10.0f || degree < -10.0f))
        //{
        //    SetActionObj(obj, actionObj);
        //    // 回転命令
        //    ChangeOrder(obj, OrderStatus.TURN, OrderDirection.LEFT);
        //    //m_IsRotate = true;
        //    m_IsRotate = true;
        //    return;
        //}

        //// 目標との距離が遠ければ、相手に対して移動する
        //float length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
        //if (length > 1.0f)
        //{
        //    Worker worker = obj.GetComponent<Worker>();
        //    worker.ChangeAgentMovePoint(actionObj.transform.position);
        //    m_IsRotate = false;
        //    return;
        //}

        //else
        //{
        //    // 目標との角度差が大きければ、方向転換する
        //    Vector3 dir = actionObj.transform.position - this.transform.position;
        //    Vector3 forward = this.transform.position + this.transform.forward;
        //    float angle;
        //    //if()


        //    // 目標との距離が遠ければ、相手に対して移動する
        //    float length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
        //    if (length > 1.0f)
        //    {
        //        Worker worker = obj.GetComponent<Worker>();
        //        worker.ChangeAgentMovePoint(actionObj.transform.position);
        //        return;
        //    }
        //}
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);

        // 持っている命令を全て停止させる
        foreach(var orders in m_MultOrders)
        {
            m_MultOrders[orders.Key].EndOrder(obj);
        }
    }

    // 命令の追加
    protected override void AddOrder()
    {
        if (m_MultOrders.Count != 0) return;
        // 命令を取得して、追加する
        m_MultOrders.Add(OrderStatus.MOVE, m_Undroid.GetOrder(OrderStatus.MOVE));
        m_MultOrders.Add(OrderStatus.TURN, m_Undroid.GetOrder(OrderStatus.TURN));
        //m_MultOrders.Add(OrderStatus.TURN, m_Undroid.GetOrder(OrderStatus.TURN));
        m_MultOrders.Add(OrderStatus.ATTACK, m_Undroid.GetOrder(OrderStatus.ATTACK));
    }
}
