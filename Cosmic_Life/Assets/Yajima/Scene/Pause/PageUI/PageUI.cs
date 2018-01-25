using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : MonoBehaviour {

    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    // 表示開始時に実行する関数
    public virtual void StartAction()
    {
        gameObject.SetActive(true);
    }

    // 表示終了時に実行する関数
    public virtual void EndAction()
    {
        gameObject.SetActive(false);
    }
}
