using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TilingImage : MonoBehaviour {

    // タイル範囲
    [SerializeField]
    private Vector2 m_TilingSize = Vector2.one;
    // オフセット
    [SerializeField]
    private Vector2 m_OffSet = Vector2.zero;
    // マテリアル
    private Material m_Material;

	// Use this for initialization
	void Start () {
        // マテリアルの取得
        m_Material = gameObject.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        // タイルサイズの設定
        m_Material.mainTextureScale = m_TilingSize;
        // オフセットの設定
        m_Material.mainTextureOffset = m_OffSet;
	}
}
