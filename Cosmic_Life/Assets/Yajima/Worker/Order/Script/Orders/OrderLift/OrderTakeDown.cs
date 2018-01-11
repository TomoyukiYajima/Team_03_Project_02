using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち下げ命令クラス
public class OrderTakeDown : Order {

    // 置くポイント
    [SerializeField]
    private Transform m_Point;
    // リフトムーブマネージャ
    private LiftMoveManager m_LiftManager;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // マネージャの追加
        m_LiftManager = this.transform.parent.GetComponent<LiftMoveManager>();
    }

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        // 何も持っていなければ、空の命令に変更
        if (!m_LiftManager.CheckLiftObject(obj))
        {
            //ChangeOrder(obj, OrderStatus.NULL);
            // 命令失敗
            FaildOrder(obj);
            return;
        }

        SetStartOrderText();

        // 親子関係を解除する
        var colliders = m_LiftManager.GetLiftObject().transform.parent.Find("Colliders").gameObject;
        m_LiftManager.ReleaseObject(obj, colliders);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        base.UpdateAction(deltaTime, obj);
    }
}
