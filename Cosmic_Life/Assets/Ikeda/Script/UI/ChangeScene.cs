using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeScene : MonoBehaviour
{

    [SerializeField]
    private Image m_RightDoor;
    [SerializeField]
    private Image m_LeftDoor;

    [SerializeField, Tooltip("アニメーションの設定")]
    private AnimationCurve m_Curve;

    [SerializeField]
    private Gear [] m_Gears;


    // Use this for initialization
    void Start()
    {
        //m_DoorState = DoorState.CloseDoor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ドアを開ける関数
    public void OpenDoor()
    {
        m_RightDoor.transform.DOLocalMoveX(200, 2.0f).SetEase(m_Curve);
        m_LeftDoor.transform.DOLocalMoveX(-200, 2.0f).SetEase(m_Curve);
    }

    //ドアを閉める関数
    public void CloseDoor()
    {
        //ギアの状態を変更する
        foreach (Gear gear in m_Gears)
        {
            gear.SetGearState(Gear.GearState.SpeedUpState);
        }
        m_RightDoor.transform.DOLocalMoveX(600, 2.5f).SetEase(Ease.InQuart);
        m_LeftDoor.transform.DOLocalMoveX(-600, 2.5f).SetEase(Ease.InQuart);
    }
}
