using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chain : MonoBehaviour {


    public RectTransform m_RecTrans;
    Vector3 m_StartPosition;

    public float m_Rate;

	// Use this for initialization
	void Start () {
        //m_RecTrans = GetComponent<RectTransform>();
        m_StartPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        
        m_RecTrans.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(600, -25), m_Rate);
	}
}
