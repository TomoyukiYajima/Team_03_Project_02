using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour {

    // 移動速度
    [SerializeField]
    private float m_Speed = 10.0f;
    // カーソル画像配列
    [SerializeField]
    private MoveCursorTexture[] m_Textures;

	// Use this for initialization
	void Start () {
        for(int i = 0; i != m_Textures.Length; ++i)
        {
            m_Textures[i].SetSpeed(m_Speed);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
