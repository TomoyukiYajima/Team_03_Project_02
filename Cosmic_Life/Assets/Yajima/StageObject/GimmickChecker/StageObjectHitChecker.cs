using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectHitChecker : MonoBehaviour
{
    // 衝突したステージオブジェクトを除外するか
    [SerializeField]
    private bool m_IsExclusion = false;
    // 指定した衝突するオブジェクト
    private bool m_IsCheckObjectHit = false;

    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // 指定したオブジェクトが衝突しているかを返します
    public bool IsCheckObjectHit() { return m_IsCheckObjectHit; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StageObject")
        {
            m_IsCheckObjectHit = true;
            StageObject obj = other.GetComponent<StageObject>();
            //
            //if (m_IsExclusion == true && obj != null) obj.Exclusion();
            //{
            //    other.tag = "Untagged";
            //    StageObject obj = other.GetComponent<StageObject>();
            //    obj.enabled = false;
            //}
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "StageObject") m_IsCheckObjectHit = false;
    }
}
