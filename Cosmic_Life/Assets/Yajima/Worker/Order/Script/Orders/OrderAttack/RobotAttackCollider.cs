using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotAttackCollider : MonoBehaviour
{
    // ダメージ量
    [SerializeField]
    private int m_Damage = 1;
    // 生成パーティクル
    [SerializeField]
    private GameObject m_Particle;
    // パーティクルの大きさ
    [SerializeField]
    private float m_ParticleSize = 1.0f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") return;

        // パーティクルの生成
        var hitPos = other.ClosestPointOnBounds(this.transform.position);
        Instantiate(m_Particle, hitPos, new Quaternion());

        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(other.gameObject)) return;
        // 実行(ダメージ処理)
        ExecuteEvents.Execute<IGeneralEvent>(
            other.gameObject,
            null,
            (e, d) => { e.onDamage(m_Damage); });
    }
}
