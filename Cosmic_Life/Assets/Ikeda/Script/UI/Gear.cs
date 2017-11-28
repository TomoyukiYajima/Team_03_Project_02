using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gear : MonoBehaviour
{

    public enum GearState
    {
        SpeedDownState,
        SpeedUpState,

        None
    }

    [SerializeField]
    private float m_Angle;

    private float m_StorageAngle;

    private GearState m_GearState;

    // Use this for initialization
    void Start()
    {
        m_GearState = GearState.SpeedDownState;
        m_StorageAngle = m_Angle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_GearState)
        {
            case GearState.SpeedDownState:
                if (m_Angle > 0)
                {
                    m_Angle -= Time.deltaTime * 4.0f;
                    if (m_Angle <= 0) AngleZero();
                }
                else if (m_Angle < 0)
                {
                    m_Angle += Time.deltaTime * 4.0f;
                    if (m_Angle >= 0) AngleZero();
                }
                break;

            case GearState.SpeedUpState:
                if (m_StorageAngle > 0)
                {
                    m_Angle -= Time.deltaTime * 4.0f;
                    if (m_Angle <= -10) m_Angle = -10.0f;
                }
                else if (m_StorageAngle < 0)
                {
                    m_Angle += Time.deltaTime * 4.0f;
                    if (m_Angle >= 10) m_Angle = 10.0f;
                }
                break;
        }
        transform.Rotate(new Vector3(0, 0, m_Angle));
    }

    public void AngleZero()
    {
        m_Angle = 0.0f;
    }

    public void SetGearState(GearState state)
    {
        m_GearState = state;
    }
}
