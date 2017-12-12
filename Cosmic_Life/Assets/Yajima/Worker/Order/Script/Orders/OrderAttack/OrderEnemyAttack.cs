using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEnemyAttack : MultOrder {

    private bool m_IsAttack = false;

    private bool m_IsRotate = false;
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

        // 対象が敵でなければ、停止させる
        if (actionObj == null || actionObj.tag != "Enemy")
        {
            EndOrder(obj);
            //失敗時の命令テキストの設定
            SetFaildText();
            //SetStartOrderText();
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
        if (m_IsAttack)
        {
            Attack(deltaTime, obj, actionObj);
            return;
        }

        var length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
        if (length < 2.0f)
        {
            // 角度の計算
            // 相手との距離を求める
            //Vector3 pos = player.transform.position - this.transform.position;
            Vector2 pos = new Vector2(obj.transform.position.x, obj.transform.position.z);
            Vector2 dir = new Vector2(actionObj.transform.position.x, actionObj.transform.position.z) - pos;
            Vector2 dir2 = new Vector2(obj.transform.forward.x, obj.transform.forward.z);
            float rad = dir.x * dir2.y + dir2.x * dir.y;
            if (Mathf.Abs(rad) > 0.1f)
            {
                if (!m_IsRotate)
                {
                    OrderDirection orderDir = OrderDirection.LEFT;
                    // 方向の指定
                    if (rad > 0.0f) orderDir = OrderDirection.RIGHT;
                    // 
                    SetActionObj(obj, actionObj);
                    // 回転命令
                    ChangeOrder(obj, OrderStatus.TURN, orderDir);
                    m_IsRotate = true;
                }

                return;
            }

            //Attack(deltaTime, obj, actionObj);
            ChangeOrder(obj, OrderStatus.ATTACK);
            // 攻撃
            m_IsAttack = true;
            return;
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

        // 攻撃終了後の処理
        // 攻撃命令の終了
        m_MultOrders[OrderStatus.ATTACK].EndOrder(obj);
        // 相手を倒していたら、停止状態に遷移する
        if (actionObj == null)
        {
            EndOrder(obj);
            return;
        }

        // 角度の計算
        // 相手との距離を求める
        //Vector3 pos = player.transform.position - this.transform.position;
        Vector2 pos = new Vector2(obj.transform.position.x, obj.transform.position.z);
        Vector2 dir = new Vector2(actionObj.transform.position.x, actionObj.transform.position.z) - pos;
        Vector2 dir2 = new Vector2(obj.transform.forward.x, obj.transform.forward.z);
        float rad = dir.x * dir2.y + dir2.x * dir.y;
        m_IsRotate = false;

        if (Mathf.Abs(rad) > 0.1f)
        {
            if (!m_IsRotate)
            {
                OrderDirection orderDir = OrderDirection.LEFT;
                // 方向の指定
                if (rad > 0.0f) orderDir = OrderDirection.RIGHT;
                // 
                SetActionObj(obj, actionObj);
                // 回転命令
                ChangeOrder(obj, OrderStatus.TURN, orderDir);
                m_IsRotate = true;
            }
        }
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
