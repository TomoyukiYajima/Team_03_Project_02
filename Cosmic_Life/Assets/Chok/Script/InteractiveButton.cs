using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveButton : MonoBehaviour
{

    [SerializeField] private int m_interactiveAngle = 90;
    [SerializeField] private GameObject m_gimmick;

    private bool m_isActivate = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        // プレイヤーが正面向いているなら起動可能
        Vector3 directionFromPlayer = transform.position - other.transform.position;
        directionFromPlayer.Normalize();

        float angleFromPlayer = Vector3.Angle(other.transform.forward, directionFromPlayer);

        if (angleFromPlayer <= m_interactiveAngle)
        {
            if (m_isActivate) return;
            if (Input.GetButtonDown("OK"))
            {
                StageManager.GetInstance().StartBoss();
                //Debug.Log("Activate");
                //// IRobotEventが実装されていなければreturn
                //if (!ExecuteEvents.CanHandleEvent<IGimmickEvent>(m_gimmick))
                //{
                //    Debug.Log("IGimmickEvent未実装 : " + m_gimmick);
                //    return;
                //}

                //ExecuteEvents.Execute<IGimmickEvent>(
                //    m_gimmick,
                //    null,
                //    (receive, y) => receive.onActivate());
            }
        }
    }
}
