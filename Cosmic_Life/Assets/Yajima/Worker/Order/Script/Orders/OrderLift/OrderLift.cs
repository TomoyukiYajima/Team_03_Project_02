using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 持ち命令クラス
public class OrderLift : Order {

    #region 列挙クラス
    // 
    private enum LiftObjectNumber
    {
        PLAYER_LIFT_NUMBER = 1 << 0,
        OBJECT_LIFT_NUMBER = 1 << 1,
        ENEMY_LIFT_NUMBER  = 1 << 2,
    }
    #endregion

    #region 変数
    // オブジェクトチェッカー
    [SerializeField]
    private ObjectChecker m_ObjectChecker;
    // 持つオブジェクト確認用オブジェクト
    [SerializeField]
    protected CheckLiftObject m_CheckLiftObject;
    // 持つポイント
    [SerializeField]
    protected Transform m_LiftPoint;
    // 持ったか
    protected bool m_IsLift = false;
    //// 回っているか
    //private bool m_isRotate = false;
    // 持ち上げるオブジェクトの位置に到達したか
    private bool m_IsGoalPoint = false;

    // 持ち上げ状態
    private LiftObjectNumber m_LiftNumber = LiftObjectNumber.PLAYER_LIFT_NUMBER;
    // ポイントを決定したのか
    private bool m_IsSetPoint = false;

    // 持つステージオブジェクト
    protected GameObject m_LiftObject;
    // プレイヤー
    protected GameObject m_Player;

    private float m_InitSeDelay = 0.5f;
    private float m_SeDelay;

    //// 計算リスト
    //private Dictionary<LiftObjectNumber, Action<int>> m_LiftMoves =
    //    new Dictionary<LiftObjectNumber, Action<int>>();

    // 移動関数の実行リスト
    private Dictionary<LiftObjectNumber, Action<GameObject, float>> m_LiftMoves =
        new Dictionary<LiftObjectNumber, Action<GameObject, float>>();
    #endregion

    // Use this for initialization
    public override void Start()
    {
        //m_LiftMoves.Add(LiftObjectNumber.PLAYER_LIFT_NUMBER, (value) => { });
        base.Start();

        m_Player = GameObject.FindGameObjectWithTag("Player");

        // 移動関数を配列に追加
        m_LiftMoves.Add(LiftObjectNumber.PLAYER_LIFT_NUMBER, (obj, time) => { PlayerMove(obj, time); });
        m_LiftMoves.Add(LiftObjectNumber.OBJECT_LIFT_NUMBER, (obj, time) => { StageObjectMove(obj, time); });

        m_SeDelay = m_InitSeDelay;
    }

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        m_ActionObject = actionObj;
        // 持っているオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        // もし何か持っていれば、返す
        if (liftObj.childCount != 0)
        {
            // 命令失敗
            FaildOrder(obj);
            return;
        }
        else if(actionObj != null && actionObj.tag != "StageObject")
        {
            // 命令失敗
            ChangeFaildText("持テルモノヲ指定シテクダサイ");
            FaildOrder(obj);
            return;
        }

        // オブジェクトの捜索
        FindLiftObject(obj, actionObj);

        // リフトクラスを継承した子クラスのオブジェクトチェック関数を呼ぶ
        // m_LiftCheck[checkNumber].CheckObject(obj);

        // プレイヤーを持つ状態に変更する
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        if (m_IsLift) return;
        // 持てるかのチェック
        //CheckLift(obj);
        // 移動
        Move(deltaTime, obj);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj, GameObject actionObj)
    {
        UpdateAction(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        //m_IsLift = false;
        Worker robot = obj.GetComponent<Worker>();
        robot.AgentStop();

        m_IsGoalPoint = false;
        m_IsSetPoint = false;
        m_LiftObject = null;
        m_CheckLiftObject.ReleaseObject();
    }

    // 持てるかのチェックを行います
    protected void CheckLift(GameObject obj)
    {
        if (m_CheckLiftObject.IsCheckLift(m_LiftObject))
        {
            // 相手の持つポイントを取得する
            var point = m_LiftObject.transform.Find("LiftPoint");
            float length = Mathf.Abs(point.position.y - m_LiftPoint.transform.position.y);
            m_LiftObject.transform.position += Vector3.up * length;
            // 持つオブジェクトを、ロボットの持つオブジェクトに変更する
            AddLiftObj(obj);
            return;
        }
    }

    // 移動
    protected void Move(float deltaTime, GameObject obj)
    {
        //if(m_LiftObject == null)
        //{
        //    EndOrder(obj);
        //    return;
        //}

        var dis = m_LiftObject.transform.position - obj.transform.position;
        dis.y = 0.0f;
        var direction = Vector3.Normalize(dis);
        // オブジェクトの方向を向く
        //if (!m_isRotate)
        //{
        //    var dir = (
        //    new Vector3(m_LiftObject.transform.position.x, 0.0f, m_LiftObject.transform.position.z) -
        //    new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z)).normalized;
        //    obj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
        //    //obj.transform.rotation = Quaternion.FromToRotation(-obj.transform.forward, m_LiftObject.transform.position - this.transform.position);
        //    m_isRotate = true;
        //}

        if (m_IsLift) return;

        //var length = Vector3.Distance(m_LiftObject.transform.position, obj.transform.position);
        var pos = m_LiftObject.transform.position;
        pos.y = obj.transform.position.y;
        var length = Vector3.Distance(pos, obj.transform.position);

        m_LiftMoves[m_LiftNumber](obj, deltaTime);

        if (length < 5.0f && m_LiftNumber == LiftObjectNumber.OBJECT_LIFT_NUMBER)
        {
            //if (!m_IsSetPoint)
            //{
            //    m_LiftMoves[m_LiftNumber](obj);
            //    m_IsSetPoint = true;
            //}
            //m_LiftMoves[m_LiftNumber](obj);

            Worker robot = obj.GetComponent<Worker>();
            robot.GetNavMeshAgent().autoBraking = true;
            //print("ブレーキ");

            if (m_IsGoalPoint)
            {
                // オブジェクトの方向に回転
                Rotation(deltaTime, obj);
                ChangeAnimation(obj, UndroidAnimationStatus.TURN);
                return;
            }
            else if (robot.IsGoalPoint())
            {
                // ロボットのエージェントを停止させる
                robot.AgentStop();
                m_IsGoalPoint = true;
            }
        }
    }

    // オブジェクトの登録
    protected void AddLiftObj(GameObject obj)
    {
        Worker robot = obj.GetComponent<Worker>();
        robot.GetNavMeshAgent().autoBraking = false;

        var liftObj = obj.transform.Find("LiftObject");
        //m_LiftObject.transform.parent = liftObj;
        // 親を子に変更
        m_LiftObject.transform.parent = liftObj;
        var colliders = m_LiftObject.transform.Find("Colliders");
        colliders.transform.parent = liftObj;
        // 剛体のキネマティックをオンにする
        var body = m_LiftObject.transform.GetComponent<Rigidbody>();
        body.isKinematic = true;
        // 重力をオフにする
        body.useGravity = false;
        // 固定しているステータスを解除
        body.constraints = RigidbodyConstraints.None;
        m_IsLift = true;
        // ナビメッシュオブジェクトを非アクティブ状態に変更
        var nav = m_LiftObject.transform.GetComponent<NavMeshObstacle>();
        nav.enabled = false;

        // IKの設定
        SetIKTransform(robot);

        //var stageObj = liftObj.GetChild(0).GetComponent<StageObject>();
        var plane = m_LiftObject.transform.GetComponent<IronPlane>();

        if(plane != null)
        {
            m_LiftObject.transform.position = new Vector3(0.0f, 0.5f, 0.65f);
            m_LiftObject.transform.Rotate(Vector3.right * 90.0f);

            // UIに命令テキストの設定
            ChangeOrderText("軽イデス");
            SetStartOrderText();
            ChangeAnimation(obj, UndroidAnimationStatus.LIFT);
        }
        else
        {
            // 相手の持ち上げポイントを取得する
            var point = m_LiftObject.transform.Find("LiftPoint");
            float length = m_LiftPoint.transform.position.y - point.position.y;
            m_LiftObject.transform.position += Vector3.up * length;
            //var colliders = liftObj.GetChild(1);
            colliders.transform.position += Vector3.up * length;

            // UIに命令テキストの設定
            SetStartOrderText();
            ChangeAnimation(obj, UndroidAnimationStatus.LIFT);
        }


        // 持ち上げるオブジェクトがプレイヤーの場合
        // プレイヤーを持ち上げるメッセージを送る
        if (m_Player == m_LiftObject)
        {
            print("プレイヤーを持ち上げる");
            // メッセージを送る

        }
    }

    // 上げる座標を返します
    public Vector3 GetLiftPoint() { return m_LiftPoint.position; }

    #region 持ち上げ捜索関数
    // 持ち上げるオブジェクトの捜索
    protected void FindLiftObject(GameObject obj, GameObject actionObj)
    {
        m_IsLift = false;
        //m_isRotate = false;

        // 参照するオブジェクトがある場合
        if (actionObj != null)
        {
            if(actionObj.tag == "StageObject")
            {
                // 見ているものを持つオブジェクトに変更する
                m_LiftObject = actionObj;
                m_LiftMoves[LiftObjectNumber.PLAYER_LIFT_NUMBER](obj, 1.0f);
                m_LiftNumber = LiftObjectNumber.OBJECT_LIFT_NUMBER;
                // 命令承認SEの再生
                SoundManager.Instance.PlaySe("SE_Undroid_Order");
                ChangeAnimation(obj, UndroidAnimationStatus.WALK);
                return;
            } 
        }

        // プレイヤーとの距離を求める
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerLength = m_ObjectChecker.GetLength();
        if (player != null) playerLength = Vector3.Distance(player.transform.position, this.transform.position);

        //if (playerLength >= m_ObjectChecker.GetLength())
        //{
        //    // プレイヤーが範囲外の場合、近くに何も持つものがない場合は、返す
        //    if (m_ObjectChecker.GetStageObjects().Count == 0)
        //    {
        //        print("持ち上げるものがありません");
        //        // 空の状態に遷移
        //        //ChangeOrder(obj, OrderStatus.NULL);
        //        EndOrder(obj);
        //        return;
        //    }
        //}

        // プレイヤーを持つオブジェクトに設定
        //m_LiftObject = player;
        //m_LiftNumber = LiftObjectNumber.PLAYER_LIFT_NUMBER;
        if(m_ObjectChecker.GetStageObjects().Count == 0)
        {
            //print("持ち上げるものがありません");
            // 空の状態に遷移
            //ChangeOrder(obj, OrderStatus.NULL);
            // 命令失敗
            FaildOrder(obj);
            //EndOrder(obj);
            return;
        }

        GameObject liftStageObj =  m_ObjectChecker.GetStageObjects()[0];
        // ステージオブジェクトとの距離を求める
        //var objLength = m_ObjectChecker.GetLength();
        var objLength = Vector3.Distance(liftStageObj.transform.position, this.transform.position);
        for (int i = 1; i != m_ObjectChecker.GetStageObjects().Count; ++i)
        {
            // 相手との距離を求める
            var length = Vector3.Distance(m_ObjectChecker.GetStageObjects()[i].transform.position, this.transform.position);
            if (objLength > length)
            {
                objLength = length;
                liftStageObj = m_ObjectChecker.GetStageObjects()[i];
            }
        }

        // ステージオブジェクトがプレイヤーより近い場合は、ステージオブジェクトを持つ
        if (playerLength >= objLength)
        {
            m_LiftObject = liftStageObj;
            m_LiftNumber = LiftObjectNumber.OBJECT_LIFT_NUMBER;
        }

        if (m_LiftObject == null)
        {
            // 
            m_LiftObject = liftStageObj;
            m_LiftNumber = LiftObjectNumber.OBJECT_LIFT_NUMBER;
        }

        // 持ち上げ処理の前準備
        //m_LiftMoves[m_LiftNumber](obj);
        m_LiftMoves[LiftObjectNumber.PLAYER_LIFT_NUMBER](obj, 1.0f);

        // 命令承認SEの再生
        SoundManager.Instance.PlaySe("SE_Undroid_Order");
        ChangeAnimation(obj, UndroidAnimationStatus.WALK);
    }
    #endregion

    #region 移動関数
    // プレイヤーや敵の位置に移動
    private void PlayerMove(GameObject obj, float deltaTime)
    {
        Worker robot = obj.GetComponent<Worker>();
        // 移動ポイントの更新
        robot.ChangeAgentMovePoint(m_LiftObject.transform.position);
        robot.GetNavMeshAgent().isStopped = false;
    }

    // ステージオブジェクトの持っているポイントに移動
    private void StageObjectMove(GameObject obj, float deltaTime)
    {
        Worker robot = obj.GetComponent<Worker>();

        var pos = m_LiftObject.transform.position;
        pos.y = obj.transform.position.y;
        var robotLength = Vector3.Distance(pos, obj.transform.position);

        // SEの再生
        m_SeDelay = Mathf.Max(m_SeDelay - deltaTime, 0.0f);
        if (m_SeDelay == 0.0f)
        {
            SoundManager.Instance.PlaySe("SE_Undroid_Move");
            m_SeDelay = m_InitSeDelay;
        }

        if (robotLength < 3.0f)
        {
            if (!m_IsSetPoint)
            {
                Transform points = m_LiftObject.transform.Find("Points");
                // 一番近いポイントの位置に移動
                Transform movePoint = null;
                float length = m_ObjectChecker.GetLength();
                for (int i = 0; i != points.childCount; ++i)
                {
                    for (int j = 0; j != points.GetChild(i).childCount; ++j)
                    {
                        Transform point = points.GetChild(i).GetChild(j);
                        float pointLenght = Vector3.Distance(point.position, obj.transform.position);
                        if (length > pointLenght)
                        {
                            movePoint = point;
                            length = pointLenght;
                        }
                    }
                    //Transform point = points.GetChild(i);
                    //float pointLenght = Vector3.Distance(point.position, obj.transform.position);
                    //if (length > pointLenght)
                    //{
                    //    movePoint = point;
                    //    length = pointLenght;
                    //}
                }

                // 移動ポイントの更新
                print(movePoint.name);
                robot.ChangeAgentMovePoint(movePoint.position);
                //robot.GetNavMeshAgent().isStopped = false;

                m_IsSetPoint = true;
            }
        }
        else robot.UpdateAgentPoint();
    }

    // 回転
    private void Rotation(float deltaTime, GameObject obj)
    {
        //if (m_IsRotation) return;

        Vector2 otherPos = new Vector2(m_LiftObject.transform.position.x, m_LiftObject.transform.position.z);
        //otherPos.y = obj.transform.position.y;
        Vector2 otherDir = otherPos - new Vector2(obj.transform.position.x, obj.transform.position.z);
        Vector2 forward = new Vector2(obj.transform.forward.x, obj.transform.forward.z);

        float angle = Mathf.Atan2(otherDir.y - forward.y, otherDir.x - forward.x);
        //float angle = Vector2.Angle(new Vector2(obj.transform.forward.x, obj.transform.forward.z), otherDir);
        //if (angle <= 1.0f)
        //{
        //    //m_IsRotation = true;
        //    return;
        //}

        float angle2 = Vector2.Angle(otherDir, forward);

        print(angle.ToString());
        if (Mathf.Abs(angle2) <= 3.0f)
        {
            // 回転が終了したら、持ち上げリストに追加
            AddLiftObj(obj);
            // エージェントの停止処理
            Worker robot = obj.GetComponent<Worker>();
            robot.AgentStop();
            // 終了処理
            EndOrder(obj);
            return;
        }

        float dir = 1.0f;
        //if(angle2 < )
        //if (angle < 0.0f) dir = -1.0f;
        // 回転
        obj.transform.Rotate(obj.transform.up, m_Undroid.GetRotateSpeed() * dir * deltaTime);
        //obj.transform.Rotate(obj.transform.up, m_TurnSpeed * dir * deltaTime);
    }
    #endregion

    // IKの座標の設定
    private void SetIKTransform(Worker robot)
    {
        Transform handPoints = m_LiftObject.transform.Find("HandPoints");

        if (handPoints == null) return;

        // ポイントとの距離
        float length = 50.0f;
        // 
        Transform point = null;

        for(int i = 0; i != handPoints.childCount; ++i)
        {
            // 長さを比較する
            float len = Vector3.Distance(robot.transform.position, handPoints.GetChild(i).position);

            if (length > len)
            {
                length = len;
                point = handPoints.GetChild(i);
                //robot.SetHandIK(point.GetChild(0), point.GetChild(1));
            }
        }

        robot.SetHandIK(point.GetChild(0), point.GetChild(1));
    }
}
