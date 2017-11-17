using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPullOut : OrderLift {

    // 後ずさりしたか
    private bool m_IsBackMove = false;
    // 後ずさりする時間
    private float m_BackTime;

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        base.UpdateAction(deltaTime, obj);

        if (m_IsBackMove) return;

        if (m_IsLift)
        {
            var speed = 1.0f;
            obj.transform.position += -obj.transform.forward * speed * deltaTime;
            m_BackTime += deltaTime;
            // 後ずさりが完了したか
            if (m_BackTime < 1.0f) return;
            m_IsBackMove = true;
            return;
        }

        // 持ち上げられるかのチェック
        if (m_CheckLiftObject.IsCheckLift(m_LiftObject))
        {
            AddLiftObj(obj);
            return;
        }

        // 移動
        Move(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);

        m_BackTime = 0.0f;
        m_IsBackMove = false;
    }
}
