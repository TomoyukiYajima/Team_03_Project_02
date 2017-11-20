using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    private Text m_text;

    private string m_curText;
    private int m_displayChar;

	// Use this for initialization
	void Start () {
        m_text = GetComponent<Text>();

        m_curText = m_text.text;
        m_displayChar = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private IEnumerator DisplayText()
    {
        while(m_displayChar < m_curText.Length)
        {
            m_displayChar++;
            m_text.text = m_curText.Substring(0,m_displayChar);

            if (m_displayChar >= m_curText.Length) m_displayChar -= 4;

            if(m_displayChar>= m_curText.Length - 3)
            {
                yield return new WaitForSeconds(0.3f); ;
            }
            else
            {
                yield return new WaitForSeconds(0.1f); ;
            }
        }
    }

    public void SetText(string text)
    {
        m_text.text = text;
        m_displayChar = 0;
        m_curText = m_text.text;
        m_text.text = m_curText.Substring(0, m_displayChar);
        StopAllCoroutines();
        StartCoroutine(DisplayText());

    }
}
