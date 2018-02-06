using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderFollow : Order {

    // プレイヤー
    private Transform m_Player;
    // 初期SE再生間隔
    private float m_InitSeDelay = 0.5f;
    // SE再生間隔
    private float m_SeDelay;
    // アニメーションを変更したか
    private bool m_IsChangeAnim = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_SeDelay = m_InitSeDelay;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj = null)
    {
        //base.StartAction(obj, actionObj);
        //m_ActionNumber = ActionNumber.OBJECT_ACTION;

        // UIに命令テキストの設定
        //SetStartOrderText();

        m_Undroid = obj.GetComponent<Worker>();
        m_Undroid.GetNavMeshAgent().isStopped = false;

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        float len = 1.5f;
        if (m_Undroid.GetPlayerLength() >= len) ChangeAnimation(obj, UndroidAnimationStatus.WALK);
        else ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
        m_IsChangeAnim = true;

        // 命令承認SEの再生
        SoundManager.Instance.PlaySe("SE_Undroid_Order");
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        // プレイヤーの後ろの位置に移動
        if (m_Player == null)
        {
            // プレイヤーがいなければ命令失敗
            FaildOrder(obj);
            return;
        }

        //m_Player
        Vector3 backPos = m_Player.position;// - m_Player.forward;
        m_Undroid.ChangeAgentMovePoint(backPos);

        float len = 1.5f;
        // 物を持っている場合
        if (obj.transform.Find("LiftObject").childCount != 0) len = 3.5f;

        if (m_Undroid.GetAgentPointLength() < len)
        {
            // エージェントが停止していたら返す
            if (m_Undroid.GetNavMeshAgent().isStopped) return;
            m_Undroid.AgentStop();
            // アニメーションの変更
            //if (!m_IsChangeAnim) 
            ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
            m_IsChangeAnim = false;
            m_SeDelay = m_InitSeDelay;
        }
        else
        {
            if (!m_Undroid.GetNavMeshAgent().isStopped)
            {
                // SEのループ再生
                //if (!SoundManager.Instance.IsPlaySe("SE_Undroid_Move"))
                //    SoundManager.Instance.PlaySe("SE_Undroid_Move");
                m_SeDelay = Mathf.Max(m_SeDelay - deltaTime, 0.0f);
                if(m_SeDelay == 0.0f)
                {
                    SoundManager.Instance.PlaySe("SE_Undroid_Move");
                    m_SeDelay = m_InitSeDelay;
                }

                return;
            }
            // 移動の再開
            m_Undroid.GetNavMeshAgent().isStopped = false;
            // アニメーションの変更
            //if (!m_IsChangeAnim)
            ChangeAnimation(obj, UndroidAnimationStatus.WALK);
            m_IsChangeAnim = false;
            // 初期SE再生
            SoundManager.Instance.PlaySe("SE_Undroid_Move");
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
        m_SeDelay = m_InitSeDelay;
    }
}
