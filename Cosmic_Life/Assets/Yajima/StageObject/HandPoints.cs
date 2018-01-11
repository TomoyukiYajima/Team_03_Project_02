using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoints : MonoBehaviour {
    //
    [SerializeField]
    private Transform m_CenterPoint;
    [SerializeField]
    private Transform m_LeftPoint;
    [SerializeField]
    private Transform m_RightPoint;

    private float m_CenterLength;
    private Vector3 m_Distance;

    // Use this for initialization
    void Start () {
        //m_CenterLength = Mathf.Abs(m_LeftPoint.position.y - m_CenterPoint.position.y);
        m_CenterLength = Vector3.Distance(this.transform.position, m_CenterPoint.position);
        m_Distance = this.transform.position - m_CenterPoint.position;
    }
	
	// Update is called once per frame
	void Update () {
        // 座標更新
        var pos = this.transform.position;
        //pos = m_CenterPoint.position + new Vector3(1.0f, 0.0f, 1.0f) * m_CenterLength;
        pos = m_CenterPoint.position + m_Distance * m_CenterLength;
        pos.y = this.transform.position.y;
        this.transform.position = pos;
        this.transform.localRotation = Quaternion.Inverse(m_CenterPoint.parent.rotation);

        //// ハンドポイントの座標更新
        //var leftPos = m_LeftPoint.position;
        //leftPos.y = m_CenterPoint.position.y - m_CenterLength;
        //m_LeftPoint.position = leftPos;

        //var rightPos = m_RightPoint.position;
        //rightPos.y = m_CenterPoint.position.y - m_CenterLength;
        //m_RightPoint.position = rightPos;
        //m_LeftPoint.position = m_CenterPoint.position + Vector3.down * m_CenterLength;
    }

    // 左のポイントを返します
    public Transform GetLeftPoint() { return m_LeftPoint; }

    // 右のポイントを返します
    public Transform GetRightPoint() { return m_RightPoint; }
}
