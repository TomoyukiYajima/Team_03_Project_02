using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 命令なしクラス
public class OrderNull : Order {

    public override void StartAction(GameObject obj, GameObject actionObj = null)
    {
        base.StartAction(obj, actionObj);

        //ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        //base.UpdateAction(deltaTime, obj);
    }
}
