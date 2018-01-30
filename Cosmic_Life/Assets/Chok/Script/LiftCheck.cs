using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCheck : MonoBehaviour {
    [SerializeField] private StageObject[] m_stageObj;
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;
    [SerializeField] private TakeDownCheck m_takeDown;
    [SerializeField] private GameObject m_moveCheck;

    private bool m_isEnd;
	// Use this for initialization
	void Start () {
        m_isEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_isEnd) return;
		foreach(var obj in m_stageObj)
        {
            if (!obj.IsLift()) continue;
            m_isEnd = true;
            if (m_image != null) m_image.SetActive(true);
            if (m_takeDown != null) m_takeDown.SetObj(obj);
            if (m_moveCheck != null) m_moveCheck.SetActive(true);
            StageManager.GetInstance().StartAction(m_action);

            break;
        }
	}
}
