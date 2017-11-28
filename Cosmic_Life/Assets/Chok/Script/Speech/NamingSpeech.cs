using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;

public class NamingSpeech : MonoBehaviour
{

    private KeywordRecognizer m_namingRecognizer;

    // Use this for initialization
    void Start()
    {

        string[] names = { /*PlayerPrefs.GetString("RobotName")*/ "ロボット" };

        m_namingRecognizer = new KeywordRecognizer(names);
        m_namingRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_namingRecognizer.Start();

    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        Debug.Log("名前認識");

        var robot = GameObject.FindGameObjectWithTag("Robot");

        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(robot))
        {
            Debug.Log("IOrderEvent未実装");
            return;
        }

        ExecuteEvents.Execute<IOrderEvent>(
            robot,
            null,
            (receive, y) => receive.onOrder(OrderStatus.FOLLOW));
    }

    private void OnApplicationQuit()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        if (m_namingRecognizer != null && m_namingRecognizer.IsRunning)
        {
            m_namingRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_namingRecognizer.Stop();
        }
    }
}
