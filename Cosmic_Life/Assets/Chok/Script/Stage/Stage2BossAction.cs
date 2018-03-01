using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BossAction : StageAction
{
    public override IEnumerator Action(Pausable pause)
    {
        FadeMgr.Instance.FadeOut(
            1.0f,
            () => {
                MovePlayer();
                MoveRobot();
                EraseAllEnemy();
                ChangeCamera();
            }
            );
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(
            1.0f,
            () => ResetGimmick()
            );
        yield return new WaitForSeconds(4.0f);
        FadeMgr.Instance.FadeOut(
            1.0f,
            () => {
                ChangeCamera();
                SpawnObject();
            }
            );
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(1.0f);
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeOut(
            1.0f,
            () => {
                ChangeCamera();
                m_isEnd = true;
            }
            );
        yield return new WaitForSeconds(1.0f);
        FadeMgr.Instance.FadeIn(1.0f);

        Destroy(this.gameObject, 0.5f);

        yield return null;
    }
}
