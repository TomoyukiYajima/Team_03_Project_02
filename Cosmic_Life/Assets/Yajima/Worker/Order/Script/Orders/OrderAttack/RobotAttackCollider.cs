using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotAttackCollider : MonoBehaviour
{
    [SerializeField]
    private int m_Damage = 1;   // ダメージ量

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") return;

        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(other.gameObject)) return;
        // 実行(ダメージ処理)
        ExecuteEvents.Execute<IGeneralEvent>(
            other.gameObject,
            null,
            (e, d) => { e.onDamage(m_Damage); });
    }
}
