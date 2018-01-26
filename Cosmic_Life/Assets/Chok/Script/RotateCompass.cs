using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCompass : MonoBehaviour {

    private Transform m_parent;

    private float m_angle;

	// Use this for initialization
	void Start () {
        m_parent = GameObject.FindGameObjectWithTag("Player").transform.FindChild("Model").transform;
        m_angle = m_parent.eulerAngles.y;

    }
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, 0, -m_parent.eulerAngles.y - m_angle +180.0f);
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
    }
}
