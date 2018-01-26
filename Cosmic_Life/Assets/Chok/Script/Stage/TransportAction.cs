using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportAction : StageAction
{
    public override IEnumerator Action(Pausable pause)
    {
        yield return new WaitForSeconds(1.0f);

        FadeMgr.Instance.FadeOut(
            2.0f,
            () => {
                MovePlayer();
                MoveRobot();
                pause.pausing = true;
            }
            );
        yield return new WaitForSeconds(2.0f);
        pause.pausing = false;
        m_isEnd = true;
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(1.0f);
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }
}
