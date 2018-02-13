using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.PostProcessing;

public class PlayerUI : MonoBehaviour
{
    //[SerializeField] private Image m_background;
    //[SerializeField] private RectTransform[] m_utilities;
    //[SerializeField] private GameObject[] m_particles;
    //[SerializeField] private float m_offsetX;
    ////[SerializeField] private RectTransform m_hpBar;
    //[SerializeField] private float width;
    //[SerializeField] private float height;
    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private PostProcessingProfile m_profile;
    private Player m_player;
    //private Animator m_animator;


    private void Awake()
    {
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
        m_profile.grain.enabled = false;
        var setting = m_profile.grain.settings;
        setting.intensity = 0.5f;
        setting.luminanceContribution = 0.8f;
        setting.size = 1.5f;
        m_profile.grain.settings = setting;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (m_player != null) m_player.onCollide += UpdateHPUI;

        //m_animator = GetComponent<Animator>();
        //StartCoroutine(FadeBackground());
    }

    private void OnDestroy()
    {
        m_player.onCollide -= UpdateHPUI;
    }

    public void UpdateHPUI(float hp)
    {
        if (hp >= m_player.MaxHP)
        {
            m_profile.grain.enabled = false;
            var setting = m_profile.grain.settings;
            setting.intensity = 0.5f;
            setting.luminanceContribution = 0.8f;
            setting.size = 1.5f;
            m_profile.grain.settings = setting;
        }
        else
        {
            m_profile.grain.enabled = true;
            var setting = m_profile.grain.settings;
            setting.intensity = 0.5f + Mathf.Min(0.5f,0.5f * (( m_player.MaxHP - hp ) / m_player.MaxHP ));
            setting.luminanceContribution = 0.8f - Mathf.Min(0.8f, 0.8f * ((m_player.MaxHP - hp) / m_player.MaxHP));
            setting.size = 1.5f + Mathf.Min(1.5f, 1.5f * ((m_player.MaxHP - hp) / m_player.MaxHP));
            m_profile.grain.settings = setting;

            if (hp <= 0)
            {
                StartCoroutine(GameOver());
            }
        }

        //m_animator.SetFloat("Health", hp);
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

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.0f);
        m_gameOverUI.SetActive(true);

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
