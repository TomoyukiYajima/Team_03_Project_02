using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBgm("BGM_Title");
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetButtonDown("OK"))
        //{
        //    SceneMgr.Instance.SceneTransition(SceneType.StageSample2);
        //}
	}

    public int GetNumber()
    {
        return 0;
    }

    public void StageStart()
    {
        SceneMgr.Instance.SceneTransition(SceneType.StageSample2);
    }
}
