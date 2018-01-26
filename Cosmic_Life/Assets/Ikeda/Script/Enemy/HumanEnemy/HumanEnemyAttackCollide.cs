using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanEnemyAttackCollide : MonoBehaviour
{

    [SerializeField]
    private int m_Damage;

    [SerializeField, Tooltip("ノックバックの強さの設定")]
    private float m_KnockBack;

    //[SerializeField]
    //private float m_KnockBackUp;

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
        if (collision.transform.tag == "Enemy") return;
        Destroy(gameObject);
        if (collision.gameObject.tag != "Player") return;
        //if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(collision.gameObject)) return;
        //実行
        ExecuteEvents.Execute<IGeneralEvent>(collision.gameObject, null, (e, d) => { e.onDamage(m_Damage); });
        Vector3 relativePos = collision.transform.position - transform.position;
        relativePos.Normalize();
        Rigidbody rb = collision.transform.GetComponent<Rigidbody>();
        rb.AddForce(/*(Vector3.up * m_KnockBackUp) + */((-relativePos) * m_KnockBack));
    }
}
