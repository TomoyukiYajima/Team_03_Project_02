using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDownCheck : MonoBehaviour {

    private StageObject m_stageObj;
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_stageObj == null) return;
        if (!m_stageObj.IsLift())
        {
            if (m_image != null) m_image.SetActive(true);
            StageManager.GetInstance().StartAction(m_action);
        }
    }

    public void SetObj(StageObject obj)
    {
        m_stageObj = obj;
    }
}
