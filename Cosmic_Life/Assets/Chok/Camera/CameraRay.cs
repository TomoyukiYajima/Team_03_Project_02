using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour
{
    [SerializeField, Range(0f, 20f), Tooltip("レイ距離")] private float m_rayDist;
    [SerializeField, Range(0f, 360f), Tooltip("プレイヤー視野角度")] private float m_rayAngle;
    //[SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private cakeslice.Outline m_playerOutline;

    private GameObject m_colliderObj;   // 当たったオブジェクトを格納する関数
    private GameObject m_colliderObject;
    public GameObject CollideObj { get { return m_colliderObject; } private set { }
    }
    private Transform m_rayPos;         // レイ開始位置
    private Transform m_player;         // プレイヤー
    private Transform m_rayCenter;
    private Vector3 m_rayDir;           // レイ方向

    public delegate void OnRayHit(bool hit);
    public event OnRayHit onRayHit;

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        // プレイヤーのレイ開始座標
        m_rayPos = transform.Find("RayStart").transform;
        m_rayCenter = transform.Find("RayCenter").transform;

        m_rayDir = Vector3.zero;

        foreach(var outline in GameObject.FindObjectsOfType<cakeslice.Outline>())
        {
            outline.enabled = false;
        }
        //m_playerOutline.enabled = false;

        m_colliderObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = m_rayCenter.position - m_rayPos.position;
        dir.y = (m_rayCenter.position - transform.position).y;
        dir.Normalize();
        //dir.y = dirY;

        float angle = Vector3.Angle(m_player.forward, dir);
        m_rayDir = dir;

        //// y軸更新
        //m_rayDir.y = transform.forward.y;

        // レイを飛ばす
        Ray ray = new Ray(m_rayPos.position, dir * m_rayDist);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_rayDist);
        // デバッグ描画
        Debug.DrawRay(m_rayPos.position, dir * m_rayDist, new Color(1f, 0, 0));

        // なにも当たってないとき
        if (hitInfo.collider == null)
        {
            EndFlash(null);
            return;
        }
        // 格納されたオブジェクトとあたっているオブジェクトが同一ではないとき
        else if (m_colliderObj != hitInfo.collider.gameObject)
        {
            EndFlash(hitInfo.collider.gameObject);
            //StartFlash(new Color(0.5f, 0.5f, 0.5f), 1.0f);
        }

}

    private void StartFlash(Color color, float duration)
    {
        // オブジェクトがStageObjectコンポーネントを実装しているかをチェック
        cakeslice.Outline material = null;
        //StageObject material = m_colliderObj.GetComponent<StageObject>();
        if ((material = m_colliderObj.GetComponent<cakeslice.Outline>()) == null)
        {
            m_colliderObject = null;
        }
        else
        {
            // 点滅開始
            material.enabled = true;
            m_playerOutline.enabled = true;

            m_colliderObject = m_colliderObj;

            if (onRayHit != null) onRayHit(true);

            //m_colliderObj.transform.FindChild("Infomation").gameObject.SetActive(true);
        }
    }

    private void EndFlash(GameObject obj)
    {
        if (onRayHit != null) onRayHit(false);
        // 格納されたオブジェクトが空ではないとき
        if (m_colliderObj != null)
        {
            // オブジェクトがStageObjectコンポーネントを実装しているかをチェック
            cakeslice.Outline material = null;
            if ((material = m_colliderObj.GetComponent<cakeslice.Outline>()) != null)
            {
                // 点滅終了
                m_playerOutline.enabled = false;
                material.enabled = false;
            }
        }
        // 格納オブジェクトを更新
        m_colliderObj = obj;
        // 更新されたオブジェクトをロボットに送る
        SendObject(m_colliderObj);
        if (m_colliderObj == null)
        {
            m_colliderObject = null;
        }
        else
        {
            StartFlash(new Color(0.5f, 0.5f, 0.5f), 1.0f);
        }
    }

    private void SendObject(GameObject obj)
    {
        // シーンにいる全部のロボットを入れる
        var robotList = GameObject.FindGameObjectsWithTag("Robot");

        // 全部のロボットにオーダーを出す
        foreach (var robot in robotList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(robot))
            {
                Debug.Log("IOrderEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (receive, y) => receive.setObject(obj));
        }

    }
}
