using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 移動命令クラス
// public class OrderMove : Order
public class OrderMove : DirectionOrder {

    [SerializeField]
    private float m_MoveSpeed = 3.0f;           // 移動速度
    [SerializeField]
    private float m_TurnSpeed = 10.0f;  // 回転速度

    private Vector3 m_Direction = Vector3.zero; // 移動方向

    //private bool m_IsRotation = false;          // 回転したか

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_Dir = OrderDirection.FORWARD;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj, actionObj);

        //SetEndPlayOrder(OrderStatus.PROTECT);
    }

    public override void StartAction(GameObject obj, OrderDirection dir)
    {
        base.StartAction(obj, dir);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        //print("Move");
        //m_ActionObject
        //base.UpdateAction(deltaTime, obj);

        // 持っているオブジェクトが、何か(ステージオブジェクト以外)に衝突している場合は返す
        if (IsLiftHit(obj)) return;

        // 回転
        //Rotation(deltaTime, obj);
        //if (!m_IsRotation) return;
        if(m_ActionObject == null)
        {
            // 移動
            obj.transform.position += obj.transform.forward * m_MoveSpeed * deltaTime;
        }
        else
        {
            var robot = obj.GetComponent<Worker>();
            // 移動ポイントの変更
            //robot.GetNavMeshAgent().destination = m_ActionObject.transform.position;
            robot.ChangeAgentMovePoint(m_ActionObject.transform.position);
            robot.GetNavMeshAgent().isStopped = false;
        }
        //obj.transform.position += m_Direction * m_MoveSpeed * deltaTime;
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        //base.UpdateAction(deltaTime, obj, actionObj);
        var length = Vector3.Distance(actionObj.transform.position, obj.transform.position);
        if (length < 2.0f)
        {
            EndOrder(obj);
            return;
        }
        var worker = obj.GetComponent<Worker>();
        worker.ChangeAgentMovePoint(actionObj.transform.position);
        worker.GetNavMeshAgent().isStopped = false;
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);
        ObjectClear(obj);
        m_Dir = OrderDirection.FORWARD;
        // オブジェクト指定がある場合
        if(m_ActionObject != null)
        {
            var robot = obj.GetComponent<Worker>();
            // 移動ポイントの変更
            robot.GetNavMeshAgent().isStopped = true;
        }
        //m_IsRotation = false;
    }

    // 回転
    //private void Rotation(float deltaTime, GameObject obj)
    //{
    //    if (m_IsRotation) return;

    //    float angle = Vector3.Angle(obj.transform.forward, m_Direction);
    //    if(angle <= 1.0f)
    //    {
    //        m_IsRotation = true;
    //        return;
    //    }
    //    float dir = 1.0f;
    //    if (angle < 0.0f) dir = -1.0f;
    //    // 回転
    //    obj.transform.Rotate(obj.transform.up, m_TurnSpeed * dir * deltaTime);
    //}

    // 移動方向の設定
    protected override void SetDirection(GameObject obj)
    {
        //switch (m_Dir)
        //{
        //    case OrderDirection.RIGHT: m_Direction = obj.transform.right; break;
        //    case OrderDirection.LEFT: m_Direction = -obj.transform.right; break;
        //    //case OrderDirection.UP: m_Direction = obj.transform.up; break;
        //    //case OrderDirection.DOWN: m_Direction = -obj.transform.up; break;
        //    case OrderDirection.FORWARD: m_Direction = obj.transform.forward; break;
        //    case OrderDirection.BACKWARD: m_Direction = -obj.transform.forward; break;
        //}
    }

//    #region エディターのシリアライズ変更
//#if UNITY_EDITOR
//    [CustomEditor(typeof(OrderMove), true)]
//    [CanEditMultipleObjects]
//    public class OrderMoveEditor : Editor
//    {
//        SerializedProperty MoveSpeed;
//        SerializedProperty TurnSpeed;

//        public void OnEnable()
//        {
//            MoveSpeed = serializedObject.FindProperty("m_MoveSpeed");
//            TurnSpeed = serializedObject.FindProperty("m_TurnSpeed");
//        }

//        public override void OnInspectorGUI()
//        {
//            // 更新
//            serializedObject.Update();

//            // 自身の取得;
//            OrderMove order = target as OrderMove;

//            // エディタ上でのラベル表示
//            EditorGUILayout.LabelField("〇移動の命令");

//            // float
//            MoveSpeed.floatValue = EditorGUILayout.FloatField("移動速度(m/s)", order.m_MoveSpeed);
//            TurnSpeed.floatValue = EditorGUILayout.FloatField("回転速度(m/s)", order.m_TurnSpeed);

//            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//#endif

//    #endregion
}
