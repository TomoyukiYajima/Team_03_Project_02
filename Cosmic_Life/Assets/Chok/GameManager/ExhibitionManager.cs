using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionManager : SingletonBehaviour<ExhibitionManager> {

	// Use this for initialization
	void Start () {
        Screen.fullScreen = true;
        UnityEngine.Cursor.visible = false;


	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Start") && Input.GetButton("Select"))
        {
            if (SceneMgr.Instance.GetCurrentSceneType() == SceneType.Title) return;
            SceneMgr.Instance.SceneTransition(SceneType.Title);
        }
	}
}
