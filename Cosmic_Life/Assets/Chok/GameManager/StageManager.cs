using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_playerPos;
    [SerializeField] private Transform[] m_robotPos;
    [SerializeField] private GameObject[] m_cameras;
    [SerializeField] private GameObject[] m_doors;
    [SerializeField] private GameObject[] m_spawnObj;
    [SerializeField] private System.Action[] m_action;
    [SerializeField] private Pausable m_pause;
    [SerializeField] private string m_bgmName;
    [SerializeField] private GameObject m_gameClearUI;
    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_disconnectUI;
    [SerializeField] private ChangeScene m_ChangeScene;
    [SerializeField] private GameObject m_Stages;
    [SerializeField] private GameObject m_playerCamera;
    [SerializeField] private StageAction[] m_actions;

    private bool m_isDisconnect;
    private bool m_prevPause;
    private static StageManager instance;   // 自身のインスタンス

    private bool m_isActivated;

    private int m_microphoneLength;

    // Use this for initialization
    void Start()
    {

        m_isActivated = false;

        m_microphoneLength = Microphone.devices.Length;

        //SoundManager.Instance.PlayBgm(m_bgmName);
        if (m_spawnObj.Length > 0)
        {
            foreach (var obj in m_spawnObj)
            {
                obj.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        // マイクが切断された状態
        if (m_isDisconnect)
        {
            if ((m_microphoneLength = Microphone.devices.Length) <= 0) return;

            m_isDisconnect = false;
            m_pause.pausing = m_prevPause;
            m_disconnectUI.SetActive(false);
        }
        if (m_microphoneLength <= 0 && !m_isDisconnect)
        {
            m_isDisconnect = true;
            m_prevPause = m_pause.pausing;
            m_pause.pausing = true;
            m_disconnectUI.SetActive(true);
            return;
        }
#endif

        if (Input.GetButtonDown("Start"))
        {
            Pause();
        }
        if (m_isActivated) return;
        if (Input.GetButtonDown("Triggrt_Right"))
        {
            m_isActivated = true;
            StartCoroutine(Activate());
        }
    }

    public void StartAction(StageAction action)
    {
        if (m_isActivated) return;
        m_isActivated = true;
        //m_pause.pausing = true;

        StartCoroutine(Action(action));
    }

    private IEnumerator Action(StageAction action)
    {
        action.Initialize();
        StartCoroutine(action.Action(m_pause));
        yield return new WaitWhile(() => !action.IsEnd);
        m_isActivated = false;
        //m_pause.pausing = false;
        yield return null;
    }

    public void StartBoss()
    {
        StartCoroutine(BossStage());
    }

    private IEnumerator BossStage()
    {
        FadeMgr.Instance.FadeOut(1.0f, () => ChangePosition());

        yield return new WaitForSeconds(1.0f);

        FadeMgr.Instance.FadeIn(1.0f);
        CloseDoor();

        yield return new WaitForSeconds(3.0f);

        FadeMgr.Instance.FadeOut(1.0f, () => SpawnBoss());
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(1.0f);
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeOut(1.0f, () => StartFight());
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(1.0f);

        yield return null;
    }

    private void ChangePosition()
    {
        m_pause.pausing = true;

        if (m_playerPos[0] != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = m_playerPos[0].position;
        }
        if (m_robotPos[0] != null)
        {
            var robot = GameObject.FindGameObjectWithTag("Robot");

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (target, y) => { target.stopOrder(); }
                );

            robot.GetComponent<NavMeshAgent>().enabled = false;

            robot.transform.position = m_robotPos[0].position;

            robot.GetComponent<NavMeshAgent>().enabled = true;
        }

        //m_playerCamera.SetActive(false);

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
        }

        m_cameras[0].SetActive(true);
    }

    private void CloseDoor()
    {
        foreach (var door in m_doors)
        {
            Debug.Log("Reset Door");
            // IGimmickEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IGimmickEvent>(door))
            {
                Debug.Log("IGimmickEvent未実装 : " + door);
                continue;
            }

            ExecuteEvents.Execute<IGimmickEvent>(
                door,
                null,
                (receive, y) => receive.onReset());
        }

    }

    private void SpawnBoss()
    {
        m_cameras[0].SetActive(false);
        m_cameras[1].SetActive(true);
        if (m_spawnObj.Length > 0)
        {
            foreach (var obj in m_spawnObj)
            {
                Debug.Log("Spawn Enemy : " + obj);
                obj.SetActive(true);
            }
        }
    }

    private void StartFight()
    {
        m_cameras[1].SetActive(false);
        //m_playerCamera.SetActive(true);

        m_pause.pausing = false;
    }

    private IEnumerator Activate()
    {
        FadeMgr.Instance.FadeOut(1.0f, () => ActionA(0));

        yield return new WaitForSeconds(1.0f);

        FadeMgr.Instance.FadeIn(1.0f, () => { m_isActivated = false; });
        m_pause.pausing = false;

        yield return null;
    }

    private void ActionA(int num)
    {
        m_pause.pausing = true;
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

    // インスタンスの取得を行います
    public static StageManager GetInstance()
    {
        // インスタンスが無かった生成する
        if (instance == null)
        {
            instance = (StageManager)FindObjectOfType(typeof(StageManager));
            // インスタンスが無かった場合、ログの表示
            if (instance == null) Debug.LogError("TutorialMediator Instance Error");
        }
        return instance;
    }

    public void GameClear()
    {
        m_gameClearUI.SetActive(true);
    }

    public void GameOver()
    {
        m_gameOverUI.SetActive(true);
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

    public void Pause()
    {
        if (m_pause.pausing)
        {
            if (!PauseManager.GetInstance().IsClose) return;
            PauseManager.GetInstance().Init();
        }
        m_pause.pausing = m_pause.pausing == true ? false : true;
        PauseManager.GetInstance().UIActive(m_pause.pausing);
        //if (m_pause.pausing) PauseManager.GetInstance().Init();
    }

}
