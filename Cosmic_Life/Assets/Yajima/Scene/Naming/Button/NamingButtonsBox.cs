using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NamingButtonsBox : MonoBehaviour {
    // カーソル
    [SerializeField]
    private ButtonCursor m_Cursor;
    // 移動時間
    [SerializeField]
    private float m_MoveTime = 1.0f;

    // 表示するか？
    //private bool m_IsDraw = true;// = false;
    // 初期座標
    private Vector3 m_InitPosition;
    // 表示中か？
    private bool m_IsDraw = true;

	// Use this for initialization
	void Start () {
        m_InitPosition = this.transform.GetChild(0).localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //if (!m_IsDraw) return;

        // カーソルが移動中なら返す
        if (m_Cursor.IsMoving()) return;

        if (Input.GetButtonDown("X"))
        {
            if (m_IsDraw)
            {
                //print(Vector3.left * m_InitPosition.x);
                // 引き戻す
                this.transform.GetChild(0).DOLocalMove(Vector3.left * -728 + Vector3.up * m_InitPosition.y, m_MoveTime);
                m_Cursor.IsCursorStop = false;
            }
            else
            {
                // 表示する

            }

            // 切り替え
            m_IsDraw = !m_IsDraw;
            //this.transform.GetChild(0).DOLocalMove(-Vector3.left * m_InitPosition.x + Vector3.up * m_InitPosition.y, m_MoveTime);
        }
    }

    //// 表示の切り替え
    //private IEnumerator ChangeDraw()
    //{

    //}
}
