using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAction : MonoBehaviour {

    [SerializeField] private StageAction m_action;

	// Use this for initialization
	void Start () {
        StageManager.GetInstance().StartAction(m_action);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
