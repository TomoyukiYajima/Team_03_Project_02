using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerUI : MonoBehaviour {

    [SerializeField] private VolumeCheck m_volume;
    [SerializeField] private SpriteAnimation m_target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int volume = Mathf.Min((int)m_volume.GetAveragedVolume(), 4);
        m_target.SetSprite(volume);
    }
}
