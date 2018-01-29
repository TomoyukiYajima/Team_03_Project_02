using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextController : MonoBehaviour
{
    [SerializeField] private Text m_header;
    [SerializeField] private string[] m_scenerios;
    [SerializeField] private string[] m_headerText;
    [SerializeField] private Image[] m_sprite;

    private Text m_text;

    private string m_curText;
    private int m_displayChar;

    private int count;
    private bool m_isEnd;

    public bool IsEnd { get { return m_isEnd; } private set { } }

    // Use this for initialization
    void Start()
    {
        m_text = GetComponent<Text>();
    }

    public void Init()
    {
        count = 0;
        m_isEnd = false;
        SetText(m_scenerios[count]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("OK"))
        {
            if (m_displayChar < m_curText.Length)
            {
                m_displayChar = m_curText.Length;
                m_text.text = m_curText;
            }
            else
            {
                count++;
                if (count < m_scenerios.Length)
                {
                    SetText(m_scenerios[count]);
                }
                else
                {
                    m_isEnd = true;
                }
            }
        }
    }

    private IEnumerator DisplayText()
    {
        while (m_displayChar < m_curText.Length)
        {
            if (m_displayChar + 6 < m_curText.Length)
            {
                if (m_curText.Substring(m_displayChar, 6) == "<color")
                {
                    for (int i = m_displayChar; i < m_curText.Length; ++i)
                    {
                        string text = m_curText.Substring(i, 8);
                        if (text != "</color>")
                        {
                            continue;
                        }
                        m_displayChar = i + 8;
                        break;
                    }
                }
            }
            m_displayChar++;
            m_text.text = m_curText.Substring(0, m_displayChar);

            yield return new WaitForSeconds(0.1f); ;
        }
    }

    public void SetText(string text)
    {
        if (m_headerText[count] != "") m_header.text = m_headerText[count];

        if (count != 0)
        {
            if (m_sprite[count - 1] != null)
            {
                m_sprite[count - 1].DOFade(0.0f, 0.5f);
            }
        }
        if (m_sprite[count] != null)
        {
            m_sprite[count].DOFade(1.0f, 0.5f);
        }

        if (text != "")
        {
            m_text.text = text;
            m_displayChar = 0;
            m_curText = m_text.text;
            m_text.text = m_curText.Substring(0, m_displayChar);
            StopAllCoroutines();
            StartCoroutine(DisplayText());
        }

    }
}
