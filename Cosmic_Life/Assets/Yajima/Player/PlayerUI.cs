using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUI : MonoBehaviour {
    //[SerializeField] private Image m_background;
    //[SerializeField] private RectTransform[] m_utilities;
    //[SerializeField] private GameObject[] m_particles;
    //[SerializeField] private float m_offsetX;
    ////[SerializeField] private RectTransform m_hpBar;
    //[SerializeField] private float width;
    //[SerializeField] private float height;
    private Player m_player;
    private Animator m_animator;

    private void Awake()    {
        //if (m_background == null) m_background = GameObject.Find("LifeGroup").transform.FindChild("Life0").GetComponent<Image>();
        //for (int i = 0; i < m_utilities.Length; ++i)
        //{
        //    if (m_utilities[i] == null)
        //    {
        //        if (i < 4) m_utilities[i] = GameObject.Find("LifeGroup").transform.FindChild("Life" + i).GetComponent<RectTransform>();
        //        else m_utilities[i] = GameObject.Find("LifeCanvas").transform.FindChild("Life" + i).GetComponent<RectTransform>();
        //    }
        //}
        //for (int i = 0; i < m_particles.Length; ++i)
        //{
        //    if (m_particles[i] == null)
        //    {
        //        m_particles[i] = GameObject.Find("ParticleCanvas").transform.FindChild("LifeParticle" + i).gameObject;
        //    }
        //}

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (m_player != null) m_player.onCollide += UpdateHPUI;

        m_animator = GetComponent<Animator>();
        //StartCoroutine(FadeBackground());
    }

    private void OnDestroy()    {
        m_player.onCollide -= UpdateHPUI;
    }

    public void UpdateHPUI(int hp)    {
        m_animator.SetFloat("", hp);
        //m_hpBar.sizeDelta = new Vector2(hp * width, height);
        //foreach(var ui in m_utilities)
        //{
        //    ui.sizeDelta = new Vector2(m_offsetX + hp * width, height);
        //}
        //for (int i = 0; i < m_particles.Length; ++i)
        //{
        //    m_particles[i].SetActive(false);
        //}
        //for (int i = 0; i < hp; ++i)
        //{
        //    m_particles[i].SetActive(true);
        //}
    }

    //private IEnumerator FadeBackground()
    //{
    //    while (true)
    //    {
    //        m_background.DOFade(0, 1.0f).SetEase(Ease.InBack);

    //        yield return new WaitForSeconds(1.0f);

    //        m_background.DOFade(1.0f, 1.0f).SetEase(Ease.InBack);

    //        yield return new WaitForSeconds(1.0f);
    //    }
    //}
}
