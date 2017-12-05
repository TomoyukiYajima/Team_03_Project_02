using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToTarget : MonoBehaviour {
    [SerializeField] private Transform m_target;
    [SerializeField] private Vector3 m_offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = m_target.position - transform.position;
        dir.Normalize();

        transform.LookAt(new Vector3(m_target.position.x, 0, m_target.position.z));
        transform.localEulerAngles = new Vector3(90.0f, transform.localEulerAngles.y, transform.localEulerAngles.z) + m_offset;
	}
}
