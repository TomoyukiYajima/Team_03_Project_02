using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_playerPos;
    [SerializeField] private Transform[] m_robotPos;
    [SerializeField] private GameObject[] m_doors;
    [SerializeField] private GameObject[] m_spawnObj;
    [SerializeField] private System.Action[] m_action;

    private bool m_isActivated;

    // Use this for initialization
    void Start()
    {
        m_isActivated = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_isActivated) return;
        if (Input.GetButtonDown("Triggrt_Right"))
        {
            m_isActivated = true;
            StartCoroutine(Activate());
        }
    }

    private IEnumerator Activate()
    {
        FadeMgr.Instance.FadeOut(1.0f, () => Action(0));

        yield return new WaitForSeconds(1.0f);

        FadeMgr.Instance.FadeIn(1.0f, () => { m_isActivated = false; });

        yield return null;
    }

    private void Action(int num)
    {
        if (m_playerPos[num] != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = m_playerPos[num].position;
        }
        if (m_robotPos[num] != null)
        {
            var robot = GameObject.FindGameObjectWithTag("Robot");

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (target, y) => { target.stopOrder(); }
                );

            robot.GetComponent<NavMeshAgent>().enabled = false;

            robot.transform.position = m_robotPos[num].position;

            robot.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    private void DebugAction(int num)
    {
        if (m_playerPos[num] != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = m_playerPos[num].position;
        }
        if (m_robotPos[num] != null)
        {
            var robot = GameObject.FindGameObjectWithTag("Robot");

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (target, y) => { target.stopOrder(); }
                );

            robot.GetComponent<NavMeshAgent>().enabled = false;

            robot.transform.position = m_robotPos[num].position;

            robot.GetComponent<NavMeshAgent>().enabled = true;
        }

    }

}
