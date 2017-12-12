using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSample : MonoBehaviour {

    private Animator m_Animator;

	// Use this for initialization
	void Start () {
        m_Animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        print(m_Animator.velocity);
        //m_Animator.velocity = Vector3.zero;
    }
}
