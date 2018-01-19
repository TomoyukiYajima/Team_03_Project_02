using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GimmickSpawn : GimmickBase {

    [SerializeField] private Transform[] m_playerPos;
    [SerializeField] private GameObject[] m_doors;
    [SerializeField] private GameObject[] m_spawnObj;
    //[SerializeField] private GameObject m_ClearUI;

    public override void onActivate()
    {
        if (m_isActivated) return;
        m_isActivated = true;
        StartCoroutine(Activate());
    }

    private void Update()
    {
        foreach (var enemy in m_spawnObj)
        {
            if (enemy != null) return;
            else continue;
        }

        if (m_spawnObj.Length != 0)
        {
            StageManager.GetInstance().GameClear();
            //StageManager.Instance.GameClear();
            //m_ClearUI.SetActive(true);
            m_spawnObj = new GameObject[0];
        }
        //SceneMgr.Instance.SceneTransition(SceneType.Title);
    }

    private IEnumerator Activate()
    {
        if (m_doors.Length > 0)
        {
            foreach(var door in m_doors)
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

        yield return new WaitForSeconds(5.0f);

        if (m_spawnObj.Length > 0)
        {
            foreach(var obj in m_spawnObj)
            {
                Debug.Log("Spawn Enemy : " + obj);
                obj.SetActive(true);
            }
        }

        yield return new WaitForSeconds(3.0f);

        Debug.Log(this.gameObject + " 削除");

        //Destroy(this.gameObject);
    }
}
