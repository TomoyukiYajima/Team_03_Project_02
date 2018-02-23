using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3BossAction : StageAction
{

    public override IEnumerator Action(Pausable pause)
    {
        pause.pausing = true;

        FadeMgr.Instance.FadeOut(
            1.0f,
            () =>
            {
            EraseAllEnemy();
            ChangeCamera();
            SpawnObject();
        }
        );

        yield return new WaitForSeconds(1.0f);

        FadeMgr.Instance.FadeIn(
            1.0f,
            () =>
            {
                ActiveGimmick();
            }
            );

        yield return new WaitForSeconds(3.0f);

        ActiveGimmick();

        yield return new WaitForSeconds(3.0f);

        FadeMgr.Instance.FadeOut(
            0.5f, 
            () => {
                MovePlayer();
                MoveRobot();
                ChangeCamera();
                ResetGimmick();
            });

        yield return new WaitForSeconds(0.5f);


        FadeMgr.Instance.FadeIn(0.5f);
        pause.pausing = false;

        yield return null;
    }
}
