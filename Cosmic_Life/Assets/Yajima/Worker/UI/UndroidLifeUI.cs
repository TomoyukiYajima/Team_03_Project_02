using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndroidLifeUI : MonoBehaviour {

    // 表示
    [SerializeField]
    private Sprite m_DrawImage;
    // 非表示
    [SerializeField]
    private Sprite m_LostDrawImage;

    // イメージ
    private Image m_Image;

    // Use this for initialization
    void Start () {
        m_Image = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // ひょゆじの切り替え
    public void ChangeDraw(bool isLost)
    {
        if (isLost) m_Image.sprite = m_LostDrawImage;
        else m_Image.sprite = m_DrawImage;
    }
}
