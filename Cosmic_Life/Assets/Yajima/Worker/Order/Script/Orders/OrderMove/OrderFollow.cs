using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderFollow : Order {

    // プレイヤー
    private Transform m_Player;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj = null)
    {
        //base.StartAction(obj, actionObj);
        //m_ActionNumber = ActionNumber.OBJECT_ACTION;

        // UIに命令テキストの設定
        SetStartOrderText();

        m_Undroid = obj.GetComponent<Worker>();
        m_Undroid.GetNavMeshAgent().isStopped = false;

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeAnimation(obj, UndroidAnimationStatus.WALK);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        // プレイヤーの後ろの位置に移動
        if (m_Player == null)
        {
            // プレイヤーがいなければ失敗
            SetFaildText();
            EndOrder(obj);
            return;
        }

        //m_Player
        Vector3 backPos = m_Player.position;// - m_Player.forward;
        m_Undroid.ChangeAgentMovePoint(backPos);
        if (m_Undroid.GetAgentPointLength() < 1.5f)
        {
            // エージェントが停止していたら返す
            if (m_Undroid.GetNavMeshAgent().isStopped) return;
            m_Undroid.AgentStop();
            // アニメーションの変更
            ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
        }
        else
        {
            if (!m_Undroid.GetNavMeshAgent().isStopped) return;
            // 移動の再開
            m_Undroid.GetNavMeshAgent().isStopped = false;
            // アニメーションの変更
            ChangeAnimation(obj, UndroidAnimationStatus.WALK);
        }
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        UpdateAction(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);

        m_Undroid.AgentStop();
        m_Player = null;
    }
}
