using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialAction : StageAction {
    [SerializeField] private CanvasGroup m_canvas;
    [SerializeField] private TextController m_textController;

    public override IEnumerator Action(Pausable pause)
    {
        m_canvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        pause.pausing = true;
        m_canvas.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        m_textController.Init();
        yield return new WaitWhile(() => !m_textController.IsEnd);
        m_canvas.DOFade(0.0f, 0.2f);

        pause.pausing = false;

        m_isEnd = true;
        m_canvas.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);

        yield return null;
    }
}