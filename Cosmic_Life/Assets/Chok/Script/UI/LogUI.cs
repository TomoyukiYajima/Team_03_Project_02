using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    [SerializeField] private Text[] m_textList;

    private Order[] m_orderList;
    private int m_textCount;

    private int[,] m_logMap = new int[4, 4]
    {
        {  -67, -33,    0, -100 },
        {  100, -67,  -33,    0 },
        {   0, -100,  -67,  -33 },
        {  -33,   0, -100,  -67 }
    };

    // Use this for initialization
    void Start()
    {
        m_orderList = GameObject.FindObjectsOfType<Order>();
        foreach (var order in m_orderList)
        {
            order.setText += UpdateLogUI;
            Debug.Log(order);
        }

        m_textCount = 0;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    UpdateLogUI(Random.Range(0, 100).ToString());
        //}
    }

    private void OnDestroy()
    {
        foreach (var order in m_orderList)
        {
            order.setText -= UpdateLogUI;
        }
    }

    private void UpdateLogUI(string log)
    {
        m_textList[m_textCount].text = log;

        for (int i = 0; i < m_textList.Length; ++i)
        {
            m_textList[i].GetComponent<RectTransform>().localPosition = new Vector2(25, m_logMap[i, m_textCount]);
        }

        m_textCount = (m_textCount + 1) % m_textList.Length;
    }
}
