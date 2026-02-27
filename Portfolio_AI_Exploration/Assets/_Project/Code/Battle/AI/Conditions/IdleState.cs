using UnityEngine;

public class IdleState : BattleStateBase
{
    public IdleState(BattleAI owner) : base(owner) { }

    public override void OnEnter()
    {
    }
    public override void Tick()
    {

        if (owner.Blackboard.IsDead)
        {
            return;
        }

        if (!owner.HasCurrentAction())
        {
            owner.TryDecideSkill();
            return;

        }
        owner.SetCurrentAction(owner.GetDefaultAction());
        owner.ChangeState(owner.GetStateForCurrentAction());


    }
}
