using System.Collections;
using UnityEngine;

public enum UndroidAnimationStatus {
    IDEL            = 1 << 0,
    WALK            = 1 << 1,
    ATTACK          = 1 << 2,
    ATTACK_OBJECT   = 1 << 3
}
