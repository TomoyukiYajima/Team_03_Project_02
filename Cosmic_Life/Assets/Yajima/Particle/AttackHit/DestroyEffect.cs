using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour {

    // 削除までの時間
    [SerializeField]
    private float m_DestroyTime = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_DestroyTime = Mathf.Max(m_DestroyTime - Time.deltaTime, 0.0f);
        // 時間が経過したら、削除する
        if (m_DestroyTime <= 0.0f) Destroy(gameObject);
	}
}
