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
    [SerializeField]
    private float m_AngleResistance = 4.0f;

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
                    m_Angle -= Time.deltaTime * m_AngleResistance;
                    if (m_Angle <= 0) AngleZero();
                }
                else if (m_Angle < 0)
                {
                    m_Angle += Time.deltaTime * m_AngleResistance;
                    if (m_Angle >= 0) AngleZero();
                }
                break;

            case GearState.SpeedUpState:
                if (m_StorageAngle > 0)
                {
                    m_Angle = Mathf.Max(m_Angle - Time.deltaTime * m_AngleResistance, m_StorageAngle);

                    //m_Angle -= Time.deltaTime * m_AngleResistance;
                    //if (m_Angle <= -10) m_Angle = -10.0f;
                }
                else if (m_StorageAngle < 0)
                {
                    m_Angle = Mathf.Max(m_Angle + Time.deltaTime * m_AngleResistance, m_StorageAngle);

                    //m_Angle += Time.deltaTime * m_AngleResistance;
                    //if (m_Angle >= 10) m_Angle = 10.0f;
                }
                break;
        }
        transform.Rotate(Vector3.forward * m_Angle);
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
