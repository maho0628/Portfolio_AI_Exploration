using UnityEngine;

public class IdleState : BattleStateBase
{
    public IdleState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        if (owner.IsGaugeFull())
        {
            owner.ChangeState(owner.HoldState);
        }
    }
}
