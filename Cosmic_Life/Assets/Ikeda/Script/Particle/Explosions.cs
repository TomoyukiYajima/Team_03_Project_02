using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(StayDestroy());
	}

    IEnumerator StayDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
