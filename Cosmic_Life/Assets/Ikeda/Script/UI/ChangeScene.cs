using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeScene : SingletonBehaviour<ChangeScene>
{
    [SerializeField]
    private Image m_RightDoor;
    [SerializeField]
    private Image m_LeftDoor;
    [SerializeField]
    private float m_CloseTime = 2.0f;
    [SerializeField]
    private float m_OpenTime = 2.5f;

    [SerializeField, Tooltip("アニメーションの設定")]
    private AnimationCurve m_Curve;

    [SerializeField]
    private Gear [] m_Gears;

    //開くのが終わったか？
    private bool m_IsOpenDoor;

    // 音を鳴らすキー配列
    private List<Keyframe> m_Keys = new List<Keyframe>();
    // 現在のキーカウント
    private int m_CurrentKeyCount;
    // 現在のアニメーションカーブタイム
    private float m_CurveTime;

    // Use this for initialization
    void Start()
    {
        m_IsOpenDoor = false;
        CloseDoor();

        // 値が1のキーのみを格納
        var keys = m_Curve.keys;
        for(int i = 0; i != m_Curve.keys.Length; ++i)
        {
            if (m_Curve.keys[i].value >= 1.0f) m_Keys.Add(m_Curve.keys[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) OpenDoor();

        if (m_IsOpenDoor || m_CurrentKeyCount >= m_Keys.Count - 1) return;
        // カーブタイムの時間を加算
        m_CurveTime += m_Curve[m_Curve.keys.Length - 1].time / m_CloseTime * Time.deltaTime;
        // アニメーションカーブタイムがキータイムを超えたら、SEを鳴らす
        if (m_Keys[m_CurrentKeyCount].time < m_CurveTime)
        {
            SoundManager.Instance.PlaySe("SE_Loading_02");
            // 機能しない
            //SoundManager.Instance.ChangeSeVolume("SE_Loading_02", 0.5f - m_CurrentKeyCount * 0.1f);
            m_CurrentKeyCount++;
        }
    }

    //ドアを開ける
    public void OpenDoor()
    {
        //ギアの状態を変更する
        foreach (Gear gear in m_Gears)
        {
            gear.SetGearState(Gear.GearState.SpeedUpState);
        }
        m_RightDoor.transform.DOLocalMoveX(960, m_OpenTime).SetEase(Ease.InQuart);
        m_LeftDoor.transform.DOLocalMoveX(-960,m_OpenTime).SetEase(Ease.InQuart).OnComplete(() =>
        {
            m_IsOpenDoor = true;
        });

        // SEの再生
        name = "SE_Loading_01";
        SoundManager.Instance.PlaySe(name);
        SoundManager.Instance.StopSe(name);
        //StopSe(name, m_OpenTime);
    }

    //ドアを閉める
    public void CloseDoor()
    {
        m_IsOpenDoor = false;
        m_RightDoor.transform.DOLocalMoveX(320, m_CloseTime).SetEase(m_Curve);
        m_LeftDoor.transform.DOLocalMoveX(-320, m_CloseTime).SetEase(m_Curve);
        // 
        m_CurrentKeyCount = 0;
        m_CurveTime = 0.0f;
    }

    //ドアが開いたか返す
    public bool IsOpenDoor()
    {
        return m_IsOpenDoor;
    }

    // SEの停止(ディレイ付き)
    private IEnumerator StopSe(string name, float time = 1.0f)
    {
        yield return new WaitForSeconds(time);

        SoundManager.Instance.StopSe(name);

        yield return null;
    }
}
