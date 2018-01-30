using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectGimmickChecker : GimmickChecker
{
    enum CheckNumber
    {
        CHECK_LIFT      = 1 << 0,   // ステージオブジェクトを持っているか
        CHECK_COLLIDER  = 1 << 1    // 対象のオブジェクトが衝突しているか
    }

    // 選ばれた列挙クラス判断する
    [SerializeField]
    private CheckNumber m_CheckNumber = CheckNumber.CHECK_LIFT;
    // 表示するサポートUI
    [SerializeField]
    private HintsPoint m_SupportUI;
    // 条件達成時にアクティブ状態にするか
    [SerializeField]
    private bool m_IsActive;

    // 発光させたか？
    private bool m_IsFlash = false;
    // ステージオブジェクト
    private StageObject m_StageObject;

    // 列挙クラスに応じて呼ぶメソッドを変える
    private Dictionary<CheckNumber, Action<GameObject>> m_CheckAction =
        new Dictionary<CheckNumber, Action<GameObject>>();

	// Use this for initialization
	public override void Start () {
        m_StageObject = m_CheckerObject.GetComponent<StageObject>();

        m_SupportUI.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	public override void Update () {

        bool isActive = !m_IsActive;

        if (IsCheck()) isActive = m_IsActive;

        //switch (m_CheckNumber)
        //{
        //    case CheckNumber.CHECK_LIFT: if (IsCheck()) isActive = false; break;
        //    case CheckNumber.CHECK_COLLIDER: if (IsCheck()) isActive = true; break;
        //        //case CheckNumber.CHECK_LIFT: isActive = true; if (IsCheck()) isActive = false; break;
        //        //case CheckNumber.CHECK_COLLIDER: isActive = false; if (IsCheck()) isActive = true; break;
        //}

            //bool isActive = true;

            //if (IsCheck()) isActive = false;

        m_InfluenceObject.SetActive(isActive);
        //gameObject.SetActive(isActive);

        // サポートUIの表示
        if (m_SupportUI != null)
        {
            var undroid = GameObject.Find("Undroid");
            if (undroid == null) return;
            var liftobj = undroid.transform.Find("LiftObject");
            // アンドロイドがステージオブジェクトを持っていたら、表示する
            if (liftobj.childCount != 0)
            {
                if (m_IsFlash) return; 
                m_IsFlash = true;
                m_SupportUI.Flash();
            }
            else
            {
                if (!m_IsFlash) return;
                m_IsFlash = false;
                m_SupportUI.EndFlash();
            }
        }

        //// サポートUIの表示
        //if (m_SupportUI != null)
        //{
        //    var undroid = GameObject.Find("Undroid");
        //    if (undroid == null) return;
        //    var liftobj = undroid.transform.Find("LiftObject");
        //    // アンドロイドがステージオブジェクトを持っていたら、表示する
        //    if (Input.GetKeyDown(KeyCode.W))
        //    {
        //        m_SupportUI.Flash();
        //    }
        //    else if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        m_SupportUI.EndFlash();
        //        //m_SupportUI.gameObject.SetActive(false);

        //    }
        //}
    }

    public override bool IsCheck() {

        bool check = false;

        switch (m_CheckNumber)
        {
            case CheckNumber.CHECK_LIFT: check = m_StageObject.IsLift(); break;
            case CheckNumber.CHECK_COLLIDER: check = m_CheckerObject.GetComponent<StageObjectHitChecker>().IsCheckObjectHit(); break;
        }

        return check;
    }
}
