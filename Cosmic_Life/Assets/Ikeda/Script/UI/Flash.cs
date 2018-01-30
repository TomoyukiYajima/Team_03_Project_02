using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour {

    FlashImage m_FlashImage;

    private bool m_IsOnce;

	// Use this for initialization
	void Start () {
        m_FlashImage = transform.FindChild("FlashNowLoading").GetComponent<FlashImage>();
        m_IsOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_IsOnce)
        {
            m_IsOnce = true;
            m_FlashImage.StartFlash();
        }
	}

    public void SetFalse()
    {
        m_IsOnce = false;
    }

    public void ResetFlash()
    {
        m_IsOnce = false;
        m_FlashImage.InitFlash(null);
    }
}
