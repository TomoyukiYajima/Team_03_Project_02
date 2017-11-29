using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardImage : MonoBehaviour {

    // カメラ
    [SerializeField]
    private Transform m_Camera;
    // PlayerCamera
    // Use this for initialization
    void Start () {
        // カメラの設定
        if (m_Camera == null)
        {
            var camera = GameObject.Find("PlayerCamera").transform;
            if (camera != null) m_Camera = camera;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //ビルボード
        //Vector3 p = m_Camera.position;
        //p.y = this.transform.position.y;
        //this.transform.parent.LookAt(p);

        // 角度を合わせる
        this.transform.rotation = m_Camera.rotation;
    }
}
