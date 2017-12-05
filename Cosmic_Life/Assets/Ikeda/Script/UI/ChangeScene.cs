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

    [SerializeField, Tooltip("アニメーションの設定")]
    private AnimationCurve m_Curve;

    [SerializeField]
    private Gear [] m_Gears;

    //開くのが終わったか？
    private bool m_IsOpenDoor;


    // Use this for initialization
    void Start()
    {
        m_IsOpenDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) OpenDoor();
    }

    //ドアを開ける
    public void OpenDoor()
    {
        //ギアの状態を変更する
        foreach (Gear gear in m_Gears)
        {
            gear.SetGearState(Gear.GearState.SpeedUpState);
        }
        m_RightDoor.transform.DOLocalMoveX(960, 2.5f).SetEase(Ease.InQuart);
        m_LeftDoor.transform.DOLocalMoveX(-960, 2.5f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            m_IsOpenDoor = true;
        });
    }

    //ドアを閉める
    public void CloseDoor()
    {
        m_RightDoor.transform.DOLocalMoveX(320, 2.0f).SetEase(m_Curve);
        m_LeftDoor.transform.DOLocalMoveX(-320, 2.0f).SetEase(m_Curve);
    }

    //ドアが開いたか返す
    public bool IsOpenDoor()
    {
        return m_IsOpenDoor;
    }
}
