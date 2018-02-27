using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち上げ命令
public class OrderLiftUp : OrderLift {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj, bool isText = false)
    {
        m_ActionObject = actionObj;
        // 持っているオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        if (liftObj.childCount != 0)
        {
            // 持ち上げる動作を行う
            //m_LiftObject = liftObj.GetChild(0).gameObject;

            var stageObj = liftObj.GetChild(0).GetComponent<StageObject>();
            // 相手の持ち上げポイントを取得する
            var point = stageObj.transform.Find("LiftPoint");
            float length = m_LiftPoint.transform.position.y - point.position.y;
            stageObj.transform.position += Vector3.up * length;
            var colliders = liftObj.GetChild(1);
            colliders.transform.position += Vector3.up * length;

            // UIに命令テキストの設定
            SetStartOrderText();

            return;
        }
        else
        {
            // 命令失敗
            FaildOrder(obj);
            return;
        }

        //EndOrder(obj);
        // 持ち上げるオブジェクトの捜索
        //FindLiftObject(obj, actionObj);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        //base.UpdateAction(deltaTime, obj);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        UpdateAction(deltaTime, obj);
    }
}
