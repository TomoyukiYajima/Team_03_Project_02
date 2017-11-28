using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButton : MonoBehaviour {

    // 入力するテキスト
    [SerializeField]
    protected string m_InputText = "";


    // Use this for initialization
    public virtual void Start () {
        
    }

    // Update is called once per frame
    public virtual void Update () {
		
	}

    // テキストを設定します
    public virtual void SetText(TextButton text, int count) { m_InputText = text.GetText(); }

    // テキストの取得
    public string GetText() { return m_InputText; }
}
