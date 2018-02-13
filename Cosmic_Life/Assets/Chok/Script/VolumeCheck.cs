using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeCheck : MonoBehaviour {

    private AudioSource m_audio;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_audio.clip = Microphone.Start(null, true, 999, 44100);  // マイクからのAudio-InをAudioSourceに流す
        //m_audio.loop = true;                                      // ループ再生にしておく
        //m_audio.mute = true;                                      // マイクからの入力音なので音を流す必要がない
        m_audio.Play();
    }

    void Update()
    {
        //float vol = GetAveragedVolume();
        //print(vol);
    }

    public float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        m_audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a * 4.0f;
    }

    private void OnDestroy()
    {
        Microphone.End(null);
    }


}
