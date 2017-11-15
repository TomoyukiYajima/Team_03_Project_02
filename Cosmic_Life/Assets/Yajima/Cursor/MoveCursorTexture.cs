using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorTexture : MonoBehaviour {

    // 移動方向
    enum MoveDirection
    {
        LEFT    = 1 << 0,
        RIGHT   = 1 << 1
    }

    // 移動方向
    [SerializeField]
    private MoveDirection m_MoveDirection = MoveDirection.LEFT;
    // テクスチャ配列
    [SerializeField]
    private Transform[] m_Textures;

    // 速度
    private float m_Speed = 10.0f;
    // 方向
    private float m_Direction;

	// Use this for initialization
	void Start () {
        switch (m_MoveDirection)
        {
            case MoveDirection.LEFT: m_Direction = -1.0f; break;
            case MoveDirection.RIGHT: m_Direction = 1.0f; break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 38

        for (int i = 0; i != m_Textures.Length; ++i)
        {
            m_Textures[i].position += -Vector3.left * (m_Direction * m_Speed * Time.deltaTime);

            if(m_MoveDirection == MoveDirection.LEFT)
            {
                if (m_Textures[i].localPosition.x < m_Direction * 21.25f) m_Textures[i].localPosition += -Vector3.left * -m_Direction * (37.5f + 12.5f);
            }
            else
            {
                // if (m_Textures[i].localPosition.x > m_Direction * 12.5f)
                // 37.5f + 12.5f
                if (m_Textures[i].localPosition.x > m_Direction * 21.25f) m_Textures[i].localPosition += -Vector3.left * -m_Direction * (37.5f + 12.5f);
            }
            //if (m_Textures[i].localPosition.x < m_Direction * 13f) m_Textures[i].localPosition += -Vector3.left * -m_Direction * (38 + 13);
        }
	}

    // 移動速度の設定
    public void SetSpeed(float speed) { m_Speed = speed; }
}
