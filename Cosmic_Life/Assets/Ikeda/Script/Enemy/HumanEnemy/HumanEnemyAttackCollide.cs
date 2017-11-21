using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanEnemyAttackCollide : MonoBehaviour
{

    [SerializeField]
    private int m_Damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag != "Player") return;
        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(collision.gameObject)) return;
        //実行
        ExecuteEvents.Execute<IGeneralEvent>(collision.gameObject, null, (e, d) => { e.onDamage(m_Damage); });
    }
}
