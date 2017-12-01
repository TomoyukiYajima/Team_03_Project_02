using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chain : MonoBehaviour {


    public RectTransform m_RecTrans;
    public RectTransform m_RecTrans2;

    Vector3 m_StartPosition;
    Vector3 m_StartPosition2;

    float m_Speed = 1.0f;

    public float m_Rate;
    public float m_Rate2;

    // Use this for initialization
    void Start () {
        //m_RecTrans = GetComponent<RectTransform>();
        m_StartPosition = m_RecTrans.localPosition;
        m_StartPosition2 = m_RecTrans2.localPosition;

    }

    // Update is called once per frame
    void Update () {

        m_RecTrans.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(600, -25), m_Rate);
        //m_RecTrans2.localPosition = Vector3.Lerp(m_StartPosition2, new Vector3(600, -25), m_Rate);
        if (m_Rate <= 1.0f) m_Rate += m_Speed * Time.deltaTime;
        else m_Rate = 0;

        //if (m_Rate2 <= 1.0f) m_Rate2 += m_Speed * Time.deltaTime;
        //else m_Rate2 = 0;

    }
}
