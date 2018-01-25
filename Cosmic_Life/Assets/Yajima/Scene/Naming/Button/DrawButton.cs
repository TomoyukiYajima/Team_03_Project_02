using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawButton : PushButton {

    // 押された時に表示するページUI
    [SerializeField]
    private PageUI m_PageUI;

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public override void DownAction()
    {
        m_IsDown = true;
        // 表示するUIを変更したことを知らせる
        PauseManager.GetInstance().ChangeDrawUI(this);

        // ページUIの開始処理
        m_PageUI.StartAction();
    }

    public override void Init()
    {
        m_IsDown = false;
        // ページUIの終了処理
        m_PageUI.EndAction();
    }
}
