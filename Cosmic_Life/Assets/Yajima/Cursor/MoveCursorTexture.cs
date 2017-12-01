using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorTexture : MonoBehaviour {
    // 戻す量
    [SerializeField]
    private float m_BackLength = 20.0f;
    // 移動する座標
    [SerializeField]
    private Transform m_MovePoint;
    // テクスチャ配列
    [SerializeField]
    private Transform[] m_Textures;

    // 速度
    [SerializeField]
    private float m_Speed = 10.0f;
    // 前回の方向格納配列
    private List<Vector3> m_PrevDirs = new List<Vector3>();

    // Use this for initialization
    void Start () {
        // 方向を格納する
        for (int i = 0; i != m_Textures.Length; ++i)
        {
            var dir = (m_MovePoint.localPosition - m_Textures[i].localPosition).normalized;
            m_PrevDirs.Add(dir);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // 38

        for (int i = 0; i != m_Textures.Length; ++i)
        {
            var dir = (m_MovePoint.localPosition - m_Textures[i].localPosition).normalized;
            m_Textures[i].localPosition += dir * (m_Speed * Time.deltaTime);
            // 移動後の方向に変更
            dir = (m_MovePoint.localPosition - m_Textures[i].localPosition).normalized;
            // 前回の方向と変わっていたら、移動量分戻す
            if (Vector3.Angle(dir, m_PrevDirs[i]) > 0.01f) m_Textures[i].localPosition += dir * m_BackLength;
            else m_PrevDirs[i] = dir;
        }
    }

    // 移動速度の設定
    public void SetSpeed(float speed) { m_Speed = speed; }
}
