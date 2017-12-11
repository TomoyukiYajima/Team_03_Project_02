﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : MonoBehaviour
{
    [SerializeField] private Sprite m_aimBlack;
    [SerializeField] private Sprite m_aimEffect;

    private CameraRay m_camera;
    private Image m_aim;

    // Use this for initialization
    void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("Player").transform.FindChild("PlayerCamera").GetComponent<CameraRay>();
        if (m_camera != null) m_camera.onRayHit += UpdateAimUI;

        m_aim = GetComponent<Image>();

    }

    private void UpdateAimUI(bool hit)
    {
        if (hit) m_aim.sprite = m_aimEffect;
        else m_aim.sprite = m_aimBlack;
    }


    private void OnDestroy()
    {
        m_camera.onRayHit -= UpdateAimUI;
    }
}