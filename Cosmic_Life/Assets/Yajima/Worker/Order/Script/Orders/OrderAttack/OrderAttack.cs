using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 攻撃命令クラス
public class OrderAttack : Order {

    // 初期座標
    [SerializeField]
    protected Transform m_StartPoint;
    // 目的座標
    [SerializeField]
    protected Transform m_StopPoint;
    // 衝突判定オブジェクト
    [SerializeField]
    protected GameObject m_Collider;
    // 攻撃時間
    [SerializeField]
    protected float m_AttackTime = 1.0f;
    // 攻撃遅延時間
    [SerializeField]
    protected float m_DelayTime = 0.0f;
    // 戻る時間
    [SerializeField]
    protected float m_BackTime = 0.5f;

    // 時間
    protected float m_Timer;
    // 攻撃が終了したか
    private bool m_IsAttack = false;
    // 動かすオブジェクト
    protected GameObject m_MoveObject;

    private bool m_IsAttackEnd = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (m_Collider != null)
        {
            m_Collider.transform.position = m_StartPoint.position;
            m_MoveObject = m_Collider;
        }
    }

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj, actionObj);
        // アクティブ状態に変更
        m_Collider.SetActive(true);
        // Tweenの移動
        m_MoveObject.transform.DOLocalMove(m_StopPoint.localPosition, m_AttackTime);
        m_Timer = 0.0f;
        m_IsEndOrder = false;
        m_IsAttack = false;
        m_IsAttackEnd = false;

        ChangeAnimation(obj, UndroidAnimationStatus.ATTACK);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        base.UpdateAction(deltaTime, obj);
 
        m_Timer += deltaTime;
        if (m_Timer < m_AttackTime + m_DelayTime) return;

        // 攻撃戻り処理
        if (m_IsAttack)
        {
            // 戻る場合
            MoveObject();
            // 攻撃判定を非アクティブ状態に変更する
            if (m_Collider != null) m_Collider.SetActive(false);
            m_IsAttack = false;
        }

        if (m_Timer < m_AttackTime + m_DelayTime + m_BackTime) return;

        // 攻撃終了処理
        // イベントでの終了処理
        EndOrder(obj);
        m_IsAttack = true;
        // ディレイをかける

        var order = m_OrderState;
        // 攻撃を終了する場合は、停止命令に変更する
        if (m_IsEndOrder) order = OrderStatus.STOP;
        // 状態の変更
        ChangeOrder(obj, order);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        //base.UpdateAction(deltaTime, obj, actionObj);
        //if (m_ActionObject == null || m_ActionObject.tag != "Enemy")
        //{
        //    EndOrder(obj);
        //    return;
        //}

        m_Timer += deltaTime;
        if (m_Timer < m_AttackTime + m_DelayTime) return;

        // 攻撃戻り処理
        if (m_IsAttack)
        {
            // 戻る場合
            MoveObject();
            // 攻撃判定を非アクティブ状態に変更する
            if (m_Collider != null) m_Collider.SetActive(false);
            m_IsAttack = false;
        }

        if (m_Timer < m_AttackTime + m_DelayTime + m_BackTime) return;

        //// 攻撃終了処理
        //// イベントでの終了処理
        ////EndOrder(obj);
        //m_IsAttack = true;

        if (!m_IsAttackEnd) {
            m_IsAttackEnd = true;
            StartCoroutine(Attack(obj, deltaTime));
        }

        //var order = m_OrderState;
        //// 攻撃を終了する場合は、停止命令に変更する
        //if (m_ActionObject == null || m_ActionObject.tag != "Enemy")
        //{
        //    //order = OrderStatus.STOP;
        //    EndOrder(obj);
        //    return;
        //}
        //// 状態の変更
        //ChangeOrder(obj, order);
    }

    public override void StopAction(GameObject obj)
    {
        m_IsEndOrder = true;
    }

    public override void EndAction(GameObject obj)
    {
        //base.EndAction();
        //m_IsEndOrder = false;
        if (m_Collider.activeSelf) m_Collider.SetActive(false);
        // iTweeの停止
        //Tween
        //this.transform.DOComplete();
        m_MoveObject.transform.DOLocalMove(m_StopPoint.localPosition, m_AttackTime).Complete();
        m_MoveObject.transform.position = m_StartPoint.position;
        m_IsAttackEnd = false;
    }

    // 持っているオブジェクトの移動
    protected  virtual void MoveObject()
    {
        m_MoveObject.transform.DOLocalMove(m_StartPoint.localPosition, m_BackTime / 2);
    }

    // 攻撃を終了したかを返します
    public bool IsAttackEnd() { return m_IsAttack; }

    private IEnumerator Attack(GameObject obj, float deltaTime)
    {
        //m_Timer += deltaTime;
        //if (m_Timer < m_AttackTime + m_DelayTime) yield return null;

        //// 攻撃戻り処理
        //if (m_IsAttack)
        //{
        //    // 戻る場合
        //    MoveObject();
        //    // 攻撃判定を非アクティブ状態に変更する
        //    if (m_Collider != null) m_Collider.SetActive(false);
        //    m_IsAttack = false;
        //}

        //if (m_Timer < m_AttackTime + m_DelayTime + m_BackTime) yield return null;

        yield return new WaitForSeconds(0.5f);

        m_IsAttack = true;

        yield return new WaitForSeconds(0.5f);

        var order = m_OrderState;
        // 攻撃を終了する場合は、停止命令に変更する
        if (m_ActionObject == null || m_ActionObject.tag != "Enemy")
        {
            //order = OrderStatus.STOP;
            EndOrder(obj);
            //return;
            //yield return null;
        }
        else
        {
            // 状態の変更
            ChangeOrder(obj, order);
        }

        yield return null;
    }
}
