using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 停止命令クラス
public class OrderStop : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj = null)
    {
        //base.StartAction(obj, actionObj);
        m_ActionObject = actionObj;
        //m_Dir = obj.GetComponent<Worker>().GetOrderDir();
        // UIに命令テキストの設定
        SetStartOrderText();
        //ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
        // 命令停止SEの再生
        SoundManager.Instance.PlaySe("SE_Undroid_Stop");

        ChangeAnimation(obj, UndroidAnimationStatus.IDEL);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        base.UpdateAction(deltaTime, obj);
        //print("Stop");
    }
}
