using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseCursor : MonoBehaviour {
    // ボタン配列
    [SerializeField]
    private GameObject m_Buttons;
    // シーン遷移オブジェクト
    [SerializeField]
    private ChangeScene m_ChangeScene;
    // 入力のパワー
    [SerializeField]
    private float m_InputPower = 0.75f;
    // 移動時間
    [SerializeField]
    private float m_MoveTime = 1.0f;

    // Xの位置
    private int m_CursorRow = 0;
    // Yの位置
    private int m_CursorColumn = 0;
    // 移動中か
    private bool m_IsMoving = false;
    // 停止させているか
    private bool m_IsStop = false;

    // Use this for initialization
    void Start () {
        // 発光
        //var curchild = m_TextButtons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn);
        //var image = curchild.GetComponent<Image>();
        //var color = image.color;
        //color.a = 1.0f;
        //image.color = color;

        // 初期座標を子オブジェクトの0番の座標とする
        this.transform.position = GetCurButton().position;
    }
	
	// Update is called once per frame
	void Update () {
        // ドアが動いている間は動かさないようにする
        if (m_ChangeScene != null)
        {
            if (m_ChangeScene.gameObject.activeSelf && !m_ChangeScene.IsOpenDoor()) return;
        }
        else
        {
            var door = GameObject.Find("Door");
            if (door != null) m_ChangeScene = door.GetComponent<ChangeScene>();
        }

        // カーソルが指定座標に辿り着いていない場合は返す
        if (Vector3.Distance(GetCurButton().position, this.transform.position) > 0.1f)
        {
            PauseManager.GetInstance().IsClose = false;
            return;
        }
        else
        {
            PauseManager.GetInstance().IsClose = true;
            m_IsMoving = false;
        }

        if (m_IsMoving || m_IsStop) return;

        if (Input.GetButtonDown("OK"))
        {
            SoundManager.Instance.PlaySe("SE_Dicision");
            GetCurButton().GetComponent<PushButton>().DownAction();
            m_IsMoving = false;
            return;
        }

        // 入力の値が一定値を超えた場合、ボタンカウントを変動させる
        int column = m_CursorColumn;
        if (Input.GetAxis("Vertical") > m_InputPower) m_CursorColumn = Mathf.Max(m_CursorColumn - 1, 0);
        else if (Input.GetAxis("Vertical") < -m_InputPower) m_CursorColumn = Mathf.Min(m_CursorColumn + 1, m_Buttons.transform.GetChild(0).childCount - 1);
        int row = m_CursorRow;
        if (Input.GetAxis("Horizontal") > m_InputPower) m_CursorRow = Mathf.Min(m_CursorRow + 1, m_Buttons.transform.childCount - 1);
        else if (Input.GetAxis("Horizontal") < -m_InputPower) m_CursorRow = Mathf.Max(m_CursorRow - 1, 0);

        // ツインを利用した移動
        // カーソルが指定座標に辿り着いる場合は返す。
        if (m_CursorColumn == column && m_CursorRow == row) return;
        SoundManager.Instance.PlaySe("SE_Select");
        this.transform.DOMove(GetCurButton().position, m_MoveTime);
        // 発光処理処理の停止
        //m_Buttones[prevCount].StopFlash();
    }

    // ボタンの取得
    private Transform GetCurButton()
    {
        return m_Buttons.transform.GetChild(m_CursorRow).GetChild(m_CursorColumn);
    }

    // シーン遷移中かを返します
    private bool IsChangeScene()
    {
        return true;
    }
}
