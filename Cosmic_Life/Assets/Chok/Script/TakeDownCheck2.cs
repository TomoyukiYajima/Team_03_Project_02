using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDownCheck2 : MonoBehaviour {
    private StageObject m_stageObj;
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;

    private bool m_isEnd;
    // Use this for initialization
    void Start()
    {
        m_isEnd = false;
    }


    public void SetObj(StageObject obj)
    {
        m_stageObj = obj;
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_isEnd) return;
        if (other.tag == "StageObject")
        {
            if (!other.GetComponent<StageObject>().IsLift())
            {
                m_isEnd = true;
                if (m_image != null) m_image.SetActive(true);
                StageManager.GetInstance().StartAction(m_action);
            }

        }
    }

}
