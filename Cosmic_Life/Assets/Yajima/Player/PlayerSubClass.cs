using System;

[Serializable]
public class PlayerStatus
{
    public int hp;
    public int st;

    public PlayerStatus(int hp, int st)
    {
        this.hp = hp;
        this.st = st;
    }
}

public enum PlayerState
{
    NULL = 1 << 0,
    IDLE = 1 << 1,
    ATTACK = 1 << 2,
    HOLD = 1 << 3,
}

