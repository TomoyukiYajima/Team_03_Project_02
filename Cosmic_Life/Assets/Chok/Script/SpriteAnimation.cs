using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    private float m_changeFrameSecond;

    [SerializeField]
    private Image m_image;

    [SerializeField]
    private bool m_isLoop;
    [SerializeField]
    private bool m_isReverse;
    [SerializeField]
    private bool m_isStart;

    private float m_dTime;
    private int m_frameNum;

    // Use this for initialization
    void Start()
    {
        m_image = GetComponent<Image>();
        m_dTime = 0.0f;
        m_frameNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isStart) return;
        m_dTime += Time.deltaTime;
        if (m_changeFrameSecond < m_dTime)
        {
            m_dTime = 0.0f;
            m_frameNum = m_isReverse ? --m_frameNum : ++m_frameNum;

            if (m_frameNum >= m_sprites.Length || m_frameNum < 0)
            {
                if (m_isLoop)
                {
                    m_frameNum = m_isReverse ? m_sprites.Length - 1 : 0;
                }
                else
                {
                    m_frameNum = m_isReverse ? 0 : m_sprites.Length - 1;
                    m_isStart = false;
                }
            }
        }
        m_image.sprite = m_sprites[m_frameNum];
    }

    public int GetFrameNum()
    {
        return m_frameNum;
    }

    public int GetFrameLength()
    {
        return m_sprites.Length;
    }

    public bool IsEnded()
    {
        return m_frameNum >= m_sprites.Length - 1;
    }

    public void StartAnimation(int frameNum = 0, bool reverse = false)
    {
        m_frameNum = frameNum;
        m_isStart = true;
        m_isReverse = reverse;
    }

    public void EndAnimation()
    {

    }
}
