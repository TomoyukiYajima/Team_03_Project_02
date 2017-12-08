using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class OrderDictionary : Serialize.TableBase<string, OrderStatus, OrderPair> { }

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class OrderPair : Serialize.KeyAndValue<string, OrderStatus>
{
    public OrderPair(string key, OrderStatus value) : base(key, value) { }
}

[System.Serializable]
public class DirectionDictionary : Serialize.TableBase<string, OrderDirection, DirectionPair> { }

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class DirectionPair : Serialize.KeyAndValue<string, OrderDirection>
{
    public DirectionPair(string key, OrderDirection value) : base(key, value) { }
}
public class SpeechManager : SingletonBehaviour<SpeechManager>
{
    [SerializeField] private string m_path;
    [SerializeField] private string m_unlockFile;
    [SerializeField] private OrderDictionary m_orderDictionary;
    [SerializeField] private DirectionDictionary m_directionrDictionary;

    private KeywordRecognizer m_orderRecognizer;
    private KeywordRecognizer m_unlockRecognizer;

    private Dictionary<string,List<string>> m_orderKeyword = new Dictionary<string, List<string>>();

    //#if !UNITY_EDITOR
    void Start()
    {
        List<string> keywords = new List<string>();
        foreach (var list in m_orderDictionary.GetTable())
        {
            var textAsset = Resources.Load(m_path + list.Key) as TextAsset;
            string[] split = textAsset.text.Split(',');

            List<string> keywordList = new List<string>();

            for (int i = 0; i < split.Length; ++i)
            {
                keywordList.Add(split[i]);
                Debug.Log(split[i]);
            }
            m_orderKeyword.Add(list.Key, keywordList);
            keywords.AddRange(keywordList);
        }

        // キーワードを格納
        m_orderRecognizer = new KeywordRecognizer(keywords.ToArray());
        m_orderRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_orderRecognizer.Start();

        List<string> m_unlockKeyword = new List<string>();

        {
            var textAsset = Resources.Load(m_path + m_unlockFile) as TextAsset;
            string[] split = textAsset.text.Split(char.Parse(","));

            for (int i = 0; i < split.Length; ++i)
            {
                m_unlockKeyword.Add(split[i]);
                Debug.Log(split[i]);
            }
        }

        m_unlockRecognizer = new KeywordRecognizer(m_unlockKeyword.ToArray());
        m_unlockRecognizer.OnPhraseRecognized += OnUnlockPhrase;
        m_unlockRecognizer.Start();
        Debug.Log("キーワード読み取り完了");

        //m_robot = GameObject.FindGameObjectWithTag("Robot");
    }

    // キーワードを読み取ったら実行するメソッド
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        // オーダー初期化
        OrderStatus orderType = OrderStatus.NULL;
        OrderDirection orderDir = OrderDirection.NULL;

        // キーワードがどのリストに入ったをチェック
        foreach (var list in m_orderKeyword)
        {
            foreach (var order in list.Value)
            {
                if (args.text != order) continue;
                m_orderDictionary.GetTable().TryGetValue(list.Key, out orderType);

                if (orderType != OrderStatus.LOOK) break;
                // 方向チェック
                foreach (var dir in m_directionrDictionary.GetTable())
                {
                    if (!args.text.Contains(dir.Key)) continue;
                    orderDir = dir.Value;
                    break;
                }
                break;
            }
        }

        if (orderType == OrderStatus.NULL) return;

        SendOrder(orderType, orderDir);
    }

    private void OnUnlockPhrase(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        UnlockDoor(args.text);
    }

    private void SendOrder(OrderStatus order, OrderDirection dir)
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

            if (dir == OrderDirection.NULL)
            {
                ExecuteEvents.Execute<IOrderEvent>(
                    robot,
                    null,
                    (receive, y) => receive.onOrder(order));
            }
            else
            {
                ExecuteEvents.Execute<IOrderEvent>(
                    robot,
                    null,
                    (receive, y) => receive.onOrder(order, dir));
            }
        }

        var enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemyList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IEnemyEvent>(enemy))
            {
                Debug.Log("IEnemyEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IEnemyEvent>(
                enemy,
                null,
                (receive, y) => receive.onHear());
        }
    }

    private void UnlockDoor(String password)
    {
        var doorList = GameObject.FindGameObjectsWithTag("LockedDoor");

        foreach (var door in doorList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IGimmickEvent>(door))
            {
                Debug.Log("IEnemyEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IGimmickEvent>(
                door,
                null,
                (receive, y) => receive.onActivate(password));
        }
    }


    private void OnApplicationQuit()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        if (m_orderRecognizer != null && m_orderRecognizer.IsRunning)
        {
            m_orderRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_orderRecognizer.Dispose();
        }
        if (m_unlockRecognizer != null && m_unlockRecognizer.IsRunning)
        {
            m_unlockRecognizer.OnPhraseRecognized -= OnUnlockPhrase;
            m_unlockRecognizer.Dispose();
        }
    }
//#endif
}
