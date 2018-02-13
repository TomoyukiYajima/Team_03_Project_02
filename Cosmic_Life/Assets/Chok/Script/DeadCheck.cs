using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCheck : MonoBehaviour {
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private GameObject m_drop;
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;

    private bool m_isEnd;

    private Vector3 m_position;

	// Use this for initialization
	void Start () {
        m_isEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_isEnd) return;

		if(m_enemy == null)
        {
            m_isEnd = true;
            m_drop.transform.position = new Vector3(m_position.x, m_position.y + 1.6f, m_position.z);
            m_drop.SetActive(true);
            if (m_image != null) m_image.SetActive(true);
            StageManager.GetInstance().StartAction(m_action);

        }
        else
        {
            m_position = m_enemy.transform.position;
        }
	}
}
