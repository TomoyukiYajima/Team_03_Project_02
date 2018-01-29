using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType {
    Phase_Move,
    Phase_Lift,
    Phase_TakeDown,
    Phase_Attack,
    Phase_Password
}


public class TutorialAction : StageAction {

    [SerializeField] private TutorialType m_phase;

    public override IEnumerator Action(Pausable pause)
    {
        switch (m_phase)
        {
            case TutorialType.Phase_Move:break;
            case TutorialType.Phase_Lift:break;
            case TutorialType.Phase_TakeDown:break;
            case TutorialType.Phase_Attack:break;
            case TutorialType.Phase_Password:break;
        }
        yield return null;
    }

    
}
