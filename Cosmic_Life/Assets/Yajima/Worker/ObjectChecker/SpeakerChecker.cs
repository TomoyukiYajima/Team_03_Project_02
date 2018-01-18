using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerChecker : MonoBehaviour {

    // スピーカーUI
    [SerializeField] private VolumeCheck m_SpeakerUI;
    // アンドロイド
    [SerializeField] private Worker m_Undroid;
    // 認識経過時間
    private float m_TotalTime;
    // 認識終了時間
    private float m_NotHearTime;

    // テキストの追加
    public delegate void setString(string text);
    public event setString setText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var time = Time.deltaTime;

        var speechVol = (int)(m_SpeakerUI.GetAveragedVolume() / 4);
        // 声認識中
        if (speechVol > 0) m_TotalTime += time;
        else
        {
            //print(m_TotalTime);
            print(m_NotHearTime);
            // 認識経過時間が達した場合
            if (m_TotalTime > 1.0f)
            {
                // 
                m_NotHearTime += time;

                // 一定時間経過しても認識していない場合
                if (m_NotHearTime >= 1.0f)
                {
                    // メッセージを送信
                    if (setText != null) setText("命令ガ認証デキマセン");
                    // 時間のリセット
                    m_TotalTime = 0;
                    m_NotHearTime = 0;
                }
            }
            else m_TotalTime = 0;
        }
    }
}
