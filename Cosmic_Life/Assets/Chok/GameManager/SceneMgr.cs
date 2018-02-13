using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LogoScene,
    Title,
    Naming,
    Tutorial,
    Stage1,
    StageSample2,
    FinalStage,
    Credit,
    MikeWarn,
    Null,
}

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    // 現在のシーン
    [SerializeField]
    private SceneType m_currentScene = SceneType.Title;
    // 遷移完了か
    private bool m_isEnd = true;
    // フェードしたか
    private bool m_isFade = false;
    // フェード時間の長さ
    private float m_duration = 0.5f;
    // 遷移の作業
    private AsyncOperation m_async;

    [SerializeField]
    private Flash m_Flash;

    public bool IsEnd { get { return m_isEnd; } private set { } }

    /// <summary>
    /// Fadeしてから遷移
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void SceneTransition(SceneType name, float duration = 0.5f)
    {
        if (!m_isEnd) return;
        m_isFade = true;
        m_isEnd = false;
        FadeMgr.Instance.FadeOut(duration, () => { m_isFade = false; });
        StartCoroutine(transitionAsync(name, duration));
    }

    public void SceneTransitionSimple(SceneType name,float duration = 0.5f)
    {
        if (!m_isEnd) return;
        m_isFade = true;
        m_isEnd = false;
        FadeMgr.Instance.FadeOut(duration, () => { m_isFade = false; });
        StartCoroutine(transition(name, duration));
    }

    IEnumerator transition(SceneType name,float duration)
    {
        yield return new WaitWhile(() => m_isFade);

        yield return SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);

        UnLoadScene(m_currentScene);
        m_currentScene = name;

        if (duration != 0)
        {
            FadeMgr.Instance.FadeIn(duration, () =>
            {
                Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
            });
        }
        else
        {
            Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
        }

        m_isEnd = true;
    }

    IEnumerator transitionAsync(SceneType name, float duration)
    {
        FadeMgr.Instance.EnableAnimation(true);

        m_Flash.SetFalse();
        
        yield return new WaitWhile(() => m_isFade);

        ChangeScene.Instance.CloseDoor();
        yield return new WaitForSeconds(2.0f);

        yield return SceneManager.LoadSceneAsync("Null", LoadSceneMode.Additive);

        #region Unused
        //m_async = SceneManager.LoadSceneAsync("Null", LoadSceneMode.Additive);
        //// 読み込みが終わっても表示しない
        //m_async.allowSceneActivation = true;

        //while (!m_async.isDone)
        //{
        //    //FadeMgr.Instance.FillBar(m_async.progress /0.9f);
        //    //Debug.Log(m_async.progress);
        //    yield return null;
        //}

        //// 読み込みが完了しているSceneを表示
        //m_async.allowSceneActivation = true;
#endregion

        UnLoadScene(m_currentScene);

        yield return SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);

        UnLoadScene(SceneType.Null);
        m_currentScene = name;

        ChangeScene.Instance.OpenDoor();
        yield return new WaitForSeconds(2.5f);


        if (duration != 0)
        {
            FadeMgr.Instance.FadeIn(duration, () =>
            {
                FadeMgr.Instance.EnableAnimation(false);
                //FadeMgr.Instance.countText.enabled = false;
                //FadeMgr.Instance.animeObj.SetActive(false);
                //FadeMgr.Instance.SetCounter(0);
                //MyDebug.Log(name.ToString() + "_Scene : LoadComplete!!");
            });
        }
        else
        {
            //FadeMgr.Instance.countText.enabled = false;
            //FadeMgr.Instance.animeObj.SetActive(false);
            //FadeMgr.Instance.SetCounter(0);
            //MyDebug.Log(name.ToString() + "_Scene : LoadComplete!!");
        }
        m_Flash.ResetFlash();
        m_isEnd = true;
    }

    // 現在読み込んでいるシーンType
    public SceneType GetCurrentSceneType()
    {
        return m_currentScene;
    }

    public void UnLoadScene(SceneType name)
    {
        Debug.Log(name.ToString() + "_Scene : UnLoad!!");
        SceneManager.UnloadSceneAsync(name.ToString());
    }
}
