using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int volume = Mathf.Min((int)GetComponent<VolumeCheck>().GetAveragedVolume(), 4);
        GetComponent<SpriteAnimation>().SetSprite(volume);
    }
}
