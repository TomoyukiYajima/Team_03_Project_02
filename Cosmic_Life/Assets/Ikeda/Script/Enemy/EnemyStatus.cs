using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    //巡回
    RoundState,
    //巡回しない
    NonRoundState,
    //追跡中
    Chasing,
    //追跡中(見失っている)
    ChasingButLosed,
    //攻撃
    Attack,
    //元の位置に戻る状態
    ReturnPosition,
    //Targetの方向を向く状態
    TurnState,
    //援護状態
    SupportState,
    //暴走状態
    RunawayState,
    //聞こえた状態
    AudibleState,
    //ショック状態
    ShockState,
    //死亡時(何もしない状態)
    DeadState,

    None
}
