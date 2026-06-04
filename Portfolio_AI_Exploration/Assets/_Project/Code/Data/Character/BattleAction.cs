using System;

[Serializable]
public class BattleAction
{
    public ActionType actionType;
    public float duration;
}

public enum ActionType
{
    Attack,
    Hold,
    Wait
}
