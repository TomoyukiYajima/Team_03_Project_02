using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NamingButtonsBox : MonoBehaviour {

    private bool m_IsDraw = true;// = false;

    private float m_MoveTime = 1.0f;

    private Vector3 m_InitPosition;


	// Use this for initialization
	void Start () {
        m_InitPosition = this.transform.GetChild(0).localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //if (!m_IsDraw) return;

        if (Input.GetButtonDown("X"))
        {
            print(Vector3.left * m_InitPosition.x);
            this.transform.GetChild(0).DOLocalMove(Vector3.left * 1280 + Vector3.up * m_InitPosition.y, m_MoveTime);
            //this.transform.GetChild(0).DOLocalMove(-Vector3.left * m_InitPosition.x + Vector3.up * m_InitPosition.y, m_MoveTime);
            m_IsDraw = !m_IsDraw;
            //m_IsDraw = false;
        }
	}
}
