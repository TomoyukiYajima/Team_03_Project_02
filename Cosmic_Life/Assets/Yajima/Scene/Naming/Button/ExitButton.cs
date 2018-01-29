using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : PushButton {

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public override void DownAction()
    {
        m_IsDown = true;
        // ゲームの終了
        Application.Quit();
    }
}
