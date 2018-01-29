using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    [SerializeField] private GameObject m_dropUI;
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;
    [SerializeField] private Pausable m_pause;
    private bool m_isEnd;
    private bool m_activate;

    // Use this for initialization
    void Start()
    {
        m_isEnd = false;
        m_activate = false;

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

            if (!m_activate)
            {
                m_dropUI.SetActive(true);
                m_activate = true;
                return;

            }
            else
            {
                m_dropUI.SetActive(false);
                m_pause.pausing = false;
                m_activate = false;

                if (m_isEnd) return;
                if (m_image != null) m_image.SetActive(true);
                m_isEnd = true;
                StageManager.GetInstance().StartAction(m_action);
            }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        if (m_activate)
        {
            m_dropUI.SetActive(false);
            m_pause.pausing = false;
            m_activate = false;

            if (m_isEnd) return;
            if (m_image != null) m_image.SetActive(true);
            m_isEnd = true;
            StageManager.GetInstance().StartAction(m_action);
        }

    }




}
