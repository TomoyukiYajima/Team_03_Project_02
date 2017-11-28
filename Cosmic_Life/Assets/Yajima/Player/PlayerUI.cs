﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private Image m_background;
    [SerializeField] private RectTransform[] m_utilities;
    [SerializeField] private GameObject[] m_particles;
    [SerializeField] private float m_offsetX;
    //[SerializeField] private RectTransform m_hpBar;
    [SerializeField] private float width;
    [SerializeField] private float height;
    private Player m_player;

    private void Awake()    {
        m_player = GetComponent<Player>();

        if (m_player != null) m_player.onCollide += UpdateHPUI;

        StartCoroutine(FadeBackground());
    }

    private void OnDestroy()    {
        m_player.onCollide -= UpdateHPUI;
    }

    public void UpdateHPUI(int hp)    {
        //m_hpBar.sizeDelta = new Vector2(hp * width, height);
        foreach(var ui in m_utilities)
        {
            ui.sizeDelta = new Vector2(m_offsetX + hp * width, height);
        }
        for (int i = 0; i < m_particles.Length; ++i)
        {
            m_particles[i].SetActive(false);
        }
        for (int i = 0; i < hp; ++i)
        {
            m_particles[i].SetActive(true);
        }
    }

    private IEnumerator FadeBackground()
    {
        while (true)
        {
            m_background.DOFade(0, 1.0f).SetEase(Ease.InBack);

            yield return new WaitForSeconds(1.0f);

            m_background.DOFade(1.0f, 1.0f).SetEase(Ease.InBack);

            yield return new WaitForSeconds(1.0f);
        }
    }
}
