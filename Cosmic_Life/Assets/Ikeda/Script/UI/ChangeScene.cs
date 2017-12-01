using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeScene : MonoBehaviour
{
    //enum DoorState
    //{
    //    CloseDoor,
    //    IntervalDoor,
    //    OpenDoor,

    //    None
    //}

    [SerializeField]
    private Image m_RightDoor;
    [SerializeField]
    private Image m_LeftDoor;

    [SerializeField, Tooltip("アニメーションの設定")]
    private AnimationCurve m_Curve;

    //[SerializeField, Tooltip("閉めている間隔の時間の設定(秒)")]
    //private float m_SetIntervalTime;
    //private float m_Timer;

    [SerializeField]
    private Gear [] m_Gears;

    //private DoorState m_DoorState;

    // Use this for initialization
    void Start()
    {
        //m_DoorState = DoorState.CloseDoor;
        //m_Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //ドアを開ける
    public void OpenDoor()
    {
        //ギアの状態を変更する
        foreach (Gear gear in m_Gears)
        {
            gear.SetGearState(Gear.GearState.SpeedUpState);
        }
        m_RightDoor.transform.DOLocalMoveX(600, 2.5f).SetEase(Ease.InQuart);
        m_LeftDoor.transform.DOLocalMoveX(-600, 2.5f).SetEase(Ease.InQuart);
    }

    //ドアを閉める
    public void CloseDoor()
    {
        m_RightDoor.transform.DOLocalMoveX(200, 2.0f).SetEase(m_Curve);
        m_LeftDoor.transform.DOLocalMoveX(-200, 2.0f).SetEase(m_Curve);
    }

}
