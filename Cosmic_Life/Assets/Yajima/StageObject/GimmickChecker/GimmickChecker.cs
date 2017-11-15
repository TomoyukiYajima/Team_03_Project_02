using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickChecker : MonoBehaviour {

    // チェックするオブジェクト
    [SerializeField]
    protected GameObject m_CheckerObject;
    // 影響を与えるオブジェクト
    [SerializeField]
    protected GameObject m_InfluenceObject;

    // Use this for initialization
    public virtual void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

    // 相手側のオブジェクトがtrueかどうかを返します
    public virtual bool IsCheck() { return false; }
}
