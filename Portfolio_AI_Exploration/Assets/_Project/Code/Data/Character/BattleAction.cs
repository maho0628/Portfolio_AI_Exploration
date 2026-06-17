using System;

[Serializable]
public class BattleAction
{
    public ActionType actionType;
}

public enum ActionType
{
    Hold,
    Wait
}
