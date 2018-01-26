using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class StageAction : MonoBehaviour
{

    [SerializeField] protected bool m_isEnd = false;
    [SerializeField] private bool m_isFadeOut = true;
    [SerializeField] private bool m_isFadeIn = true;
    [SerializeField] private float m_duration = 1.0f;

    [SerializeField] private Transform[] m_playerPos;
    [SerializeField] private Transform[] m_robotPos;
    [SerializeField] private GameObject[] m_cameras;
    [SerializeField] private GameObject[] m_gimmicks;
    [SerializeField] private GameObject[] m_spawnObj;

    private int m_playerIndex;
    private int m_robotIndex;
    private int m_cameraIndex;
    private int m_gimmickIndex;

    public bool IsEnd { get { return m_isEnd; } private set { } }

    private void Start()
    {
        if (m_spawnObj.Length > 0)
        {
            foreach (var obj in m_spawnObj)
            {
                Debug.Log("Spawn Enemy : " + obj);
                obj.SetActive(false);
            }
        }
        Initialize();
    }

    public void Initialize()
    {
        m_playerIndex = 0;
        m_robotIndex = 0;
        m_cameraIndex = 0;
        m_gimmickIndex = 0;
        m_isEnd = false;
    }

    public virtual IEnumerator Action(Pausable pause)
    {
        yield return null;
    }

    protected void MovePlayer()
    {
        if (m_playerIndex < 0 && m_playerIndex >= m_playerPos.Length) return;
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = m_playerPos[m_playerIndex].position;
        m_playerIndex++;
    }

    protected void MoveRobot()
    {
        if (m_robotIndex < 0 && m_robotIndex >= m_robotPos.Length) return;
        var robot = GameObject.FindGameObjectWithTag("Robot");

        ExecuteEvents.Execute<IOrderEvent>(
            robot,
            null,
            (target, y) => { target.stopOrder(); }
            );

        robot.GetComponent<NavMeshAgent>().enabled = false;

        robot.transform.position = m_robotPos[m_robotIndex].position;

        robot.GetComponent<NavMeshAgent>().enabled = true;
        m_robotIndex++;
    }

    protected void EraseAllEnemy()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
        }
    }

    protected void ChangeCamera()
    {
        var length = m_cameras.Length;
        if (m_cameraIndex >= 0 && m_cameraIndex < length)
        {
            m_cameras[m_cameraIndex].SetActive(true);
        }
        if (m_cameraIndex > 0 && m_cameraIndex <= length)
        {
            m_cameras[m_cameraIndex - 1].SetActive(false);
        }
        m_cameraIndex++;
    }

    protected void ResetGimmick()
    {
        if (m_gimmickIndex < 0 && m_gimmickIndex >= m_gimmicks.Length) return;
        if (!ExecuteEvents.CanHandleEvent<IGimmickEvent>(m_gimmicks[m_gimmickIndex]))
        {
            Debug.Log("IGimmickEvent未実装 : " + m_gimmicks[m_gimmickIndex]);
            return;
        }

        ExecuteEvents.Execute<IGimmickEvent>(
            m_gimmicks[m_gimmickIndex],
            null,
            (receive, y) => receive.onReset());

        m_gimmickIndex++;
    }

    protected void SpawnObject()
    {
        if (m_spawnObj.Length > 0)
        {
            foreach (var obj in m_spawnObj)
            {
                Debug.Log("Spawn Enemy : " + obj);
                obj.SetActive(true);
            }
        }
    }
}
