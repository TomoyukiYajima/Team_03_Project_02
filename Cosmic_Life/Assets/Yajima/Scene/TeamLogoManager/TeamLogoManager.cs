using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TeamLogoManager : MonoBehaviour {

    // 動画再生
    [SerializeField]
    private VideoPlayer m_VideoPlayer;
    // 遷移先シーン
    [SerializeField]
    private SceneType m_SceneType;
    // 強制シーン遷移するか
    private bool m_IsChange = false;
    // シーン遷移するか？
    private bool m_IsEnd = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // ボタンが押されたらシーン遷移する
        if (Input.GetButtonDown("OK")) m_IsChange = true;

        // 強制シーン遷移しない場合
        if (!m_IsChange)
        {
            // 動画が終了したら、シーン遷移する
            if ((ulong)m_VideoPlayer.frame != m_VideoPlayer.frameCount) return;
        }
        // シーン遷移処理
        if (m_IsEnd) return;
        // SceneMgr.Instance.SceneTransitionSimpleを入れると、フェード無し遷移になる
        SceneMgr.Instance.SceneTransitionSimple(m_SceneType);
        // 動画の停止
        //m_VideoPlayer.Stop();
        m_IsEnd = true;
    }
}
