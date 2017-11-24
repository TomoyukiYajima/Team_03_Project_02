using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private RectTransform m_hpBar;
    private Player m_player;

    private void Awake()    {
        m_player = GetComponent<Player>();

        if (m_player != null) m_player.onCollide += UpdateHPUI;
    }

    private void OnDestroy()    {
        m_player.onCollide -= UpdateHPUI;
    }

    public void UpdateHPUI()    {
        float x = Mathf.Max(m_hpBar.sizeDelta.x - 256, 0);
        m_hpBar.sizeDelta = new Vector2(x, m_hpBar.sizeDelta.y);
    }
}
