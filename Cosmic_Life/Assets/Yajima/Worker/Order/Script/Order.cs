﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Order : MonoBehaviour {

    enum ActionNumber
    {
        DEFAULT          = 1 << 0,
        OBJECT_ACTION   = 1 << 1
    }

    // 表示する命令テキスト(初回時)
    [SerializeField]
    private string m_StartOrderText = "実行";
    // 任意タイミングで表示するテキスト
    [SerializeField]
    private string m_OrderText = "実行中";
    // 失敗時に表示する命令テキスト
    [SerializeField]
    private string m_FaildText = "実行デキマセン";
    // 表示する命令テキスト(更新時)
    //[SerializeField]
    //private string m_UpdateOrderText = "実行中";
    // 命令が終了したか
    protected bool m_IsEndOrder = false;
    // 命令終了時に新しい命令を実行するか
    //protected bool m_IsEndPlayOrder = false;
    // 格納された命令番号
    protected OrderNumber m_OrderNumber = OrderNumber.ONE;
    // 自身の命令状態
    protected OrderStatus m_OrderState = OrderStatus.NULL;
    // 命令終了時に実行する命令
    //protected OrderStatus m_EndPlayOrder = OrderStatus.NULL;

    // 参照するオブジェクト
    protected GameObject m_ActionObject;
    // 実行するアクションの状態
    private ActionNumber m_ActionNumber = ActionNumber.DEFAULT;
    // 方向
    protected OrderDirection m_Dir = OrderDirection.FORWARD;
    // アンドロイド
    protected Worker m_Undroid;

    // テキストコントローラ
    //private TextController m_TextController;
    // テキストの追加
    public delegate void setString(string text);
    public event setString setText;

    // 文字を表示するのか
    private bool m_IsTextDraw = false;

    // Action実行配列
    private Dictionary<ActionNumber, Action<float, GameObject, GameObject>> m_Actions =
        new Dictionary<ActionNumber, Action<float, GameObject, GameObject>>();

    // Use this for initialization
    public virtual void Start()
    {
        m_Actions[ActionNumber.DEFAULT] = (deltaTime, obj, actionObj) => { UpdateAction(deltaTime, obj); };
        m_Actions[ActionNumber.OBJECT_ACTION] = (deltaTime, obj, actionObj) => { UpdateAction(deltaTime, obj, actionObj); };

        // テキストコントローラの取得
        //m_TextController = GameObject.Find("OrderText").GetComponent<TextController>();
        //m_Undroid = 
    }

    // Update is called once per frame
    public virtual void Update() { }

    // 最初の行動
    public virtual void StartAction(GameObject obj, GameObject actionObj = null, bool isText = false)
    {
        m_ActionObject = actionObj;
        m_Dir = obj.GetComponent<Worker>().GetOrderDir();
        // UIに命令テキストの設定
        SetStartOrderText();
        // 命令承認SEの再生
        SoundManager.Instance.PlaySe("SE_Undroid_Order");

        if (m_ActionObject != null) m_ActionNumber = ActionNumber.OBJECT_ACTION;
    }

    // 行動
    public void Action(float deltaTime, GameObject obj) {
        m_Actions[m_ActionNumber](deltaTime, obj, m_ActionObject);
    }

    // 更新行動
    protected virtual void UpdateAction(float deltaTime, GameObject obj) { UpdateAction(deltaTime, obj, m_ActionObject); }
    // 更新行動(オブジェクト指定)
    protected virtual void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj) { }

    // 行動終了
    public virtual void EndAction(GameObject obj)
    {
        m_IsEndOrder = false;
        m_ActionNumber = ActionNumber.DEFAULT;
        // 命令終了時に新しい命令を実行する場合
        //if (m_IsEndPlayOrder) ChangeOrder(obj, m_EndPlayOrder);
        //m_IsEndPlayOrder = false;
    }

    // 停止時の行動
    public virtual void StopAction(GameObject obj)
    {
        m_IsEndOrder = true;
        ChangeOrder(obj, OrderStatus.STOP);
    }

    // 命令が終了したかを返します
    public bool IsEndOrder() { return m_IsEndOrder; }

    // 命令番号を設定します
    public void SetOrderNumber(OrderNumber number) { m_OrderNumber = number; }

    // 命令状態の設定
    public void SetOrderState(OrderStatus state) { m_OrderState = state; }

    // ロボットをセットします
    public void SetUndroid(Worker undroid) { m_Undroid = undroid; }

    // 命令終了時に実行する命令を設定します
    //public void SetEndPlayOrder(OrderStatus order)
    //{
    //    m_IsEndPlayOrder = true;
    //    m_EndPlayOrder = order;
    //}

    // 命令番号を取得します
    public OrderNumber GetOrderNumber() { return m_OrderNumber; }

    // 命令の変更
    protected void ChangeOrder(GameObject obj, OrderStatus status)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の変更)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.onOrder(status); });
    }

    protected void ChangeOrder(GameObject obj, OrderStatus status, OrderDirection dir)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の変更)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.onOrder(status, dir); });
    }

    // 命令の終了
    public void EndOrder(GameObject obj, bool isStop = false)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の終了)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.endOrder(m_OrderNumber, isStop); });
    }

    protected void SetActionObj(GameObject obj, GameObject actionObj)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の変更)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.setObject(actionObj); });
    }

    // 持っているオブジェクトが他のオブジェクトを衝突しているか
    protected bool IsLiftHit(GameObject obj)
    {

        Transform lift = obj.transform.Find("LiftObject");
        if (lift.childCount == 0) return false;

        Transform child = lift.GetChild(0);
        StageObject liftObj = child.GetComponent<StageObject>();
        if (liftObj == null) return false;

        return liftObj.IsHit();
    }

    // オブジェクトクリア
    protected void ObjectClear(GameObject obj)
    {
        Transform lift = obj.transform.Find("LiftObject");
        if (lift.childCount == 0) return;

        Transform child = lift.GetChild(0);
        StageObject liftObj = child.GetComponent<StageObject>();
        if (liftObj == null) return;

        liftObj.ClearObject();
    }

    // UIに命令テキストの表示(更新時)
    protected void SetStartOrderText()
    {
        if (setText != null) setText(m_StartOrderText);
    }

    // 途中から表示するテキストの表示
    protected void SetOrderText()
    {
        if (setText != null) setText(m_OrderText);
    }

    // 失敗時に表示する命令テキスト
    private void SetFaildText()
    {
        if (setText != null) setText(m_FaildText);
    }
    // 表示するテキストの変更
    protected void ChangeOrderText(string text) { m_StartOrderText = text; }
    protected void ChangeFaildText(string text) { m_FaildText = text; }
    // テキストを表示するのか
    public bool IsDrawText
    {
        get { return m_IsTextDraw; }
        set { m_IsTextDraw = value; }
    }

    // 命令の失敗
    protected void FaildOrder(GameObject obj)
    {
        // 命令承認失敗SEの再生
        SoundManager.Instance.PlaySe("SE_Undroid_Error");
        SetFaildText();
        EndOrder(obj);
    }

    // アニメーションの変更
    protected void ChangeAnimation(GameObject obj, UndroidAnimationStatus state)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の終了)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.changeAnimation(state); });
    }
    // UIに命令テキストの表示(更新時)
    //protected void SetUpdateOrderText()
    //{
    //    m_TextController.SetText(m_UpdateOrderText);
    //}
}
