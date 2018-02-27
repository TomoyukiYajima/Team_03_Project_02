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
        if (other.tag == "Player" || other.tag == "Robot" || other.tag == "Untagged") return;

        // パーティクルの生成
        var hitPos = other.ClosestPointOnBounds(this.transform.position);
        Instantiate(m_Particle, hitPos, new Quaternion());
        // SEの再生
        SoundManager.Instance.PlaySe("SE_ObjectHit");

        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(other.gameObject)) return;
        // 実行(ダメージ処理)
        ExecuteEvents.Execute<IGeneralEvent>(
            other.gameObject,
            null,
            (e, d) => { e.onDamage(m_Damage); });

        // 親オブジェクトがステージオブジェクトの場合、自身にもダメージ
        Transform parent = this.transform.parent;
        if (parent.tag != "StageObject" &&
            !ExecuteEvents.CanHandleEvent<IGeneralEvent>(parent.gameObject)) return;
        int damage = m_Damage;
        // 相手もステージオブジェクトの場合は、相手の攻撃判定のダメージ量を与える
        if (other.tag == "StageObject")
        {
            var collider = other.transform.Find("Collider").GetComponent<RobotAttackCollider>();
            damage = collider.GetDamage();
        }
        // 実行(ダメージ処理)
        ExecuteEvents.Execute<IGeneralEvent>(
            parent.gameObject,
            null,
            (e, d) => { e.onDamage(1); });
    }

    // ダメージ量の取得
    public int GetDamage() { return m_Damage; }
}
