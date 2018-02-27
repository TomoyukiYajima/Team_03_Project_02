using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LockOnUi : MonoBehaviour
{

    public GameObject m_Target;
    private RectTransform m_Rect;

    private Canvas m_Canvas;
    // Use this for initialization
    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
        m_Canvas = GetComponent<Graphic>().canvas;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target == null) return;
        //ワールド座標からscreen座標へ変換
        Camera camera = transform.root.GetComponent<Camera>();

        //var ui = Camera.main.WorldToScreenPoint(m_Target.transform.position);
        var uiPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_Target.transform.position + new Vector3(0, 0.8f, 0));

        var pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), uiPos, camera, out pos);
        m_Rect.localPosition = pos;
    }
}
