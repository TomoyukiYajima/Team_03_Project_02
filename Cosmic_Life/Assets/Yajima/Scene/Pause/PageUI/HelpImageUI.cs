using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpImageUI : PageUI {

    // 発光UI
    [SerializeField]
    private FlashImage m_FlashImage;

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public override void StartAction()
    {
        base.StartAction();
        m_FlashImage.StartFlash();
    }

    public override void EndAction()
    {
        m_FlashImage.InitFlash(null);
        base.EndAction();
    }
}
