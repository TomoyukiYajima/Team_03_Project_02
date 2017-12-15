﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class AllAttackCollider : MonoBehaviour {

    [SerializeField]
    private int m_Damage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || other.tag != "Enemy") return;
        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(other.gameObject)) return;
        //実行
        ExecuteEvents.Execute<IGeneralEvent>(other.gameObject, null, (e, d) => { e.onDamage(m_Damage); });
    }
}
