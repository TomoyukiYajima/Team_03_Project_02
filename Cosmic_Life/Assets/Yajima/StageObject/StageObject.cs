﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageObject : MonoBehaviour, IGeneralEvent
{
    #region 変数
    // 耐久値
    [SerializeField]
    private int m_HP = 3;
    // 子オブジェクト
    [SerializeField]
    private GameObject m_Child;
    // 衝突判定オブジェクト(複数)
    [SerializeField]
    private GameObject m_Colliders;
    // 中心ポイント
    [SerializeField]
    private Transform m_CenterPoint;
    // 持ち上げポイント
    [SerializeField]
    private Transform m_LiftPoint;
    // ミニマップ画像
    [SerializeField]
    private Transform m_MiniMapImage;
    // 破壊時のパーティクル
    [SerializeField]
    private GameObject m_Particle;
    // 爆発時のSE名
    [SerializeField]
    private string m_ExplosionSEName = "SE_Explosion";
    // 破壊可能か？
    [SerializeField]
    private bool m_IsBrake = true;

    // 元の親オブジェクト
    private Transform m_RootParent;
    // 子オブジェクトのマテリアル配列
    private Material[] m_Materials;
    // 発光色
    private Color m_EmissionColor = Color.black;
    // 色の初期値配列
    private Color[] m_Colors;
    // 剛体
    private Rigidbody m_Rigidbody;

    // 衝突する前の座標
    private Vector3 m_PravPosition;
    // 衝突するまでの回転量
    private Quaternion m_PravRotate;
    // 衝突するまでの移動量
    private Vector3 m_PravVelocity;
    // ステージオブジェクトに衝突しているか
    private bool m_IsStageObjectHit = false;
    // 衝突しているオブジェクト
    private List<GameObject> m_HitObjects = new List<GameObject>();

    // 衝突判定の初期位置
    private Vector3 m_InitPosition;
    // 中心との距離
    private float m_CenterLength;
    // ミニマップ画像との距離
    private float m_MapLength;
    // ミニマップの初期回転角度
    private Vector3 m_InitMapRotate;

    // 点滅するか
    //private bool m_IsFlash = false;
    private int m_FlashValue = 0;

    // 接地しているか
    private bool m_isGround = false;
    #endregion

    #region 関数
    // Use this for initialization
    void Start()
    {
        // 親オブジェクトを入れる
        m_RootParent = this.transform.parent;

        // モデルを使用する場合は、こっちを適用する
        var mesh = m_Child.GetComponent<MeshRenderer>();
        if (mesh != null) m_Materials = mesh.materials;

        // モデルなしバージョン
        //m_Materials = this.GetComponent<MeshRenderer>().materials;
        // モデルありバージョン
        //for (int i = 0; i != m_Materials.Length; ++i)
        //{
        //    //m_Colors[i] = m_Materials[i].color;
        //    //print(i);
        //}

        m_InitPosition = m_Colliders.transform.localPosition;

        m_CenterLength = Vector3.Distance(m_LiftPoint.position, m_CenterPoint.position);
        // マップ画像の初期設定を保存
        m_MapLength = Vector3.Distance(m_MiniMapImage.position, m_CenterPoint.position);
        m_InitMapRotate = m_MiniMapImage.eulerAngles;

        m_Rigidbody = this.GetComponent<Rigidbody>();

        //m_PrevPosition = this.transform.position;
        m_PravPosition = this.transform.position;
        m_PravRotate = this.transform.rotation;
        m_PravVelocity = m_Rigidbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = m_PrevPosition;

        m_isGround = false;

        m_PravPosition = this.transform.position;
        m_PravRotate = this.transform.rotation;
        m_PravVelocity = m_Rigidbody.velocity;

        // リフトポイントの座標更新
        m_LiftPoint.position = m_CenterPoint.position + Vector3.down * m_CenterLength;
        // ミニマップも固定化する
        m_MiniMapImage.position = m_CenterPoint.position + Vector3.down * m_CenterLength;
        var angles = m_InitMapRotate;
        angles.y += this.transform.eulerAngles.y + m_InitMapRotate.y;
        //angles.y += m_InitMapRotate.y;
        m_MiniMapImage.eulerAngles = angles;

        //this.transform.rotation.eulerAngles
        //localRotate = + this.transform.rotation;
        //float length = Vector3.Distance(this.transform.position, m_LiftPoint.position);
        //m_LiftPoint.position = Vector3.down * length;

        if (this.transform.parent == null) return;

        // アンドロイドがオブジェクトを持っていない場合
        if(this.transform.parent.name != "LiftObject")
        {
            // 判定する幅
            float stopVelo = 0.2f;
            // if (Mathf.Abs(m_Rigidbody.velocity.x) < stopVelo && Mathf.Abs(m_Rigidbody.velocity.z) < stopVelo && m_IsStageObjectHit == false)
            if (Mathf.Abs(m_Rigidbody.velocity.x) < stopVelo && Mathf.Abs(m_Rigidbody.velocity.z) < stopVelo)
            {
                //m_Rigidbody.isKinematic = true;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                // 衝突判定も消す
                GameObject collider = this.transform.Find("Collider").gameObject;
                if (collider != null) collider.SetActive(false);
                GameObject colliderStop = this.transform.Find("Collider_Stop").gameObject;
                if (colliderStop != null) colliderStop.SetActive(false);
            }

            // 遠くに行き過ぎた場合は削除する
            if (Mathf.Abs(this.transform.position.y - m_InitPosition.y) >= 100) Explosion();
        }
    }

    // 自己発光の設定を行います
    public void EnableEmission(Color color)
    {
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].EnableKeyword("_EMISSION");
            m_Materials[i].SetColor("_EmissionColor", color);
        }
    }

    // 自己発光をオフにします
    public void DisableEmission()
    {
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].DisableKeyword("_EMISSION");
        }
    }

    // 現在の親を元の親に初期化します
    public void InitParent()
    {
        this.transform.parent = m_RootParent;
    }

    // コライダーの初期化
    public void InitCollider()
    {
        m_Colliders.transform.localPosition = m_InitPosition;
    }

    // 参照しているオブジェクトの解放
    private IEnumerator ReleasObject(GameObject obj)
    {
        yield return new WaitForSeconds(Time.deltaTime);

        m_HitObjects.Remove(obj);
    }

    private void AddObject(GameObject obj)
    {
        //yield return new WaitForSeconds(Time.deltaTime);

        // if (m_HitObjects.IndexOf(obj) != -1) return;
        if (m_HitObjects.IndexOf(obj) < 0) m_HitObjects.Add(obj);
    }

    // 爆発させます
    private void Explosion()
    {
        // パーティクルを生成して、自身を削除する
        Instantiate(m_Particle, this.transform.position, this.transform.rotation);
        // 持ち上げられていた場合は、他のオブジェクトも削除する
        if (this.transform.parent.name == "LiftObject")
        {
            if(this.transform.parent.childCount != 0)
            {
                int count = this.transform.parent.childCount;
                for (int i = this.transform.parent.childCount - 1; i >= 1; --i)
                {
                    Destroy(this.transform.parent.GetChild(i).gameObject);
                }
            }
            // IKを初期化する
            var undroid = this.transform.parent.parent.GetComponent<Worker>();
            undroid.InitIK();
        }

        Destroy(gameObject);
        // SEの再生
        SoundManager.Instance.PlaySe(m_ExplosionSEName);
    }

    // 参照しているオブジェクトのクリア
    public void ClearObject()
    {
        //m_HitObjects.Clear();
        //m_PrevHitObjects.Clear();
    }

    // ステージオブジェクトから除外します
    public void Exclusion()
    {
        this.gameObject.tag = "Untagged";
        this.enabled = false;
        EndFlash();
        // 配列に参照されている場合は、配列から削除する
        GameObject checker = GameObject.Find("ObjectChecker");
        checker.GetComponent<ObjectChecker>().DeleteStageObject(this.gameObject); 
    }

    #region 点滅処理
    // ロボットでの点滅処理を行います
    public void FlashRobot()
    {
        m_EmissionColor += Color.gray;
        if (m_FlashValue == 0) StartCoroutine(Flash(m_EmissionColor, 0.5f));
        m_FlashValue = m_FlashValue | 1 << 1;
    }

    public void FlashPlayer()
    {
        m_EmissionColor += Color.gray;
        if (m_FlashValue == 0) StartCoroutine(Flash(m_EmissionColor, 0.5f));
        m_FlashValue = m_FlashValue | 1 << 0;
    }

    // Emissionの点滅を行います
    public void FlashEmission(Color color, float time)
    {
        //m_IsFlash = true;
        //m_EmissionColor += Color.gray;
        //if (m_FlashValue == 0) StartCoroutine(Flash(m_EmissionColor, time));
        ////m_FlashValue = m_FlashValue | 1 << 0;
        //if ((m_FlashValue & 1 << 0) == 0) m_FlashValue = m_FlashValue | 1 << 0;
        //else if ((m_FlashValue & 1 << 1) == 0) m_FlashValue = m_FlashValue | 1 << 1;
    }

    public IEnumerator Flash(Color color, float time)
    {
        // Emission をオンにする
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            // Tween で色変換
            m_Materials[i].EnableKeyword("_EMISSION");
            m_Materials[i].DOColor(color, "_EmissionColor", time);
            //m_Materials[i].SetColor("_EmissionColor", color);
        }

        // ディレイ
        yield return new WaitForSeconds(time);

        // Emission をオフにする
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].DOColor(new Color(0.0f, 0.0f, 0.0f), "_EmissionColor", time);
            //m_Materials[i].DisableKeyword("_EMISSION");
        }

        // 再帰呼び出し
        //if (!m_IsFlash) yield break;
        if (m_FlashValue == 0) yield break;
        yield return new WaitForSeconds(time);
        StartCoroutine(Flash(color, time));
    }
    // Emissionの点滅を終了させます
    public void EndFlashEmission()
    {
        //m_IsFlash = false;
        m_EmissionColor -= Color.gray;
        if((m_FlashValue & 1 << 0) != 0) m_FlashValue = m_FlashValue & 1 << 1;
        else if ((m_FlashValue & 1 << 1) != 0) m_FlashValue = m_FlashValue & 1 << 0;
        //m_FlashValue = 0;
    }
    public void EndFlash()
    {
        m_EmissionColor = Color.black;
        m_FlashValue = 0;
    }
    public void EndFlashRobot()
    {
        m_EmissionColor -= Color.gray;
        m_FlashValue = m_FlashValue & 1 << 0;
    }

    public void EndFlashPlayer()
    {
        m_EmissionColor -= Color.gray;
        m_FlashValue = m_FlashValue & 1 << 1;
    }
    // 色のリセットを行います
    public void ResetColor()
    {
        // 初期値を代入する
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].color = m_Colors[i];
        }
    }
    #endregion

    // オブジェクトと衝突しているかを返します
    public bool IsHit() { return m_HitObjects.Count != 0; }

    #region イベント関数
    public void onDamage(int amount)
    {
        // 破壊可能でなければ返す
        if (!m_IsBrake) return;

        m_HP = Mathf.Max(m_HP - amount, 0);
        // 耐久値が0になったら、爆発させる
        if (m_HP == 0) Explosion();
    }

    public void onShock() { }

    public void onThrow() { }

    public void onLift(GameObject obj) {
        if(this.transform.parent.name == "LiftObject")
        {
            //// 相手の持ち上げポイントを取得する
            OrderLift lift = obj.GetComponent<OrderLift>();
            if (lift == null) return;
            var point = this.transform.Find("LiftPoint");
            float length = Mathf.Abs(point.position.y - lift.GetLiftPoint().y);
            this.transform.position += Vector3.down * length;
        }
    }

    // アンドロイドに持ち上げられているかを返します
    public bool IsLift() { return this.transform.parent.parent.GetComponent<Worker>() != null;  }

    public void onTakeDown() { }
    #endregion

    #region 衝突判定
    // 衝突時のアクション
    protected virtual void CollideEnter(Collision collision) { }
    #endregion

    #region Unity関数
    // 衝突判定
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "StageObject")
        {
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            m_IsStageObjectHit = true;
        }

        CollideEnter(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "StageObject") m_IsStageObjectHit = false;
    }
    #endregion
    #endregion
}
