using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TeamLogoManager : MonoBehaviour {

    // 動画再生
    [SerializeField]
    private VideoPlayer m_VideoPlayer;

    private bool end = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((ulong)m_VideoPlayer.frame != m_VideoPlayer.frameCount) return;
        // シーン遷移処理
        print("シーン遷移");
        if (end) return;
        // SceneMgr.Instance.SceneTransition 第二引数にSimpleを入れると、フェード無し遷移になる
        SceneMgr.Instance.SceneTransition(SceneType.Title);
        // 動画の停止
        //m_VideoPlayer.Stop();
        end = true;
    }
}
