using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class NamingManager : MonoBehaviour
{

    // 名前入力欄
    [SerializeField]
    private GameObject m_NameTexts;
    //[SerializeField]

    //private InputField m_Field;

    // 現在のテキスト番号
    private int m_CurrentTextNumber = 0;
    // 保持しているテキスト
    private string m_Text;

    // Use this for initialization
    void Start()
    {
        //m_Field.te
    }

    // Update is called once per frame
    void Update()
    {
        // 
    }

    //public void OnGUI()
    //{
    //    Rect rect = new Rect(10, 10, 300, 50);
    //    m_Text = GUI.TextField(rect, "brew", 16);
    //}
}
