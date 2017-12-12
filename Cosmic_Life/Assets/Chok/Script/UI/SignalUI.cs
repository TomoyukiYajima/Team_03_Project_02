using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalUI : MonoBehaviour {
    private Transform m_player;
    private Transform m_robot;

    public static int Signal = 0;

	// Use this for initialization
	void Start () {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_robot = GameObject.FindGameObjectWithTag("Robot").transform;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(m_player.position, m_robot.position);
        Signal = 4 - Mathf.Min((int)(distance / 5.0f), 4);

        GetComponent<SpriteAnimation>().SetSprite(Signal);
	}
}
