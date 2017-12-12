using System.Collections;
using UnityEngine;

public enum UndroidAnimationStatus {
    IDEL            = 1 << 0,
    WALK            = 1 << 1,
    TURN            = 1 << 2,
    LIFT            = 1 << 3,
    PUT             = 1 << 4,
    ATTACK          = 1 << 5,
    ATTACK_OBJECT   = 1 << 6
}
