using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEndButton : PushButton {

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public override void DownAction()
    {
        m_IsDown = true;
        // ポーズの非表示
        StageManager.GetInstance().Pause();
    }

    public override void Init()
    {
        m_IsDown = false;
    }
}
