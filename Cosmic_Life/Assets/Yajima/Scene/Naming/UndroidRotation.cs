using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndroidRotation : MonoBehaviour {

    // 回転速度
    [SerializeField]
    private float m_RotateAngle = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // 回転方向
        int dir = 0;
        // Triggrt_Left
        if (Input.GetButton("Triggrt_Left")) dir = 1;
        else if (Input.GetButton("Triggrt_Right")) dir = -1;
        // 回転
        this.transform.Rotate(Vector3.up * m_RotateAngle * dir * Time.deltaTime);
	}
}
