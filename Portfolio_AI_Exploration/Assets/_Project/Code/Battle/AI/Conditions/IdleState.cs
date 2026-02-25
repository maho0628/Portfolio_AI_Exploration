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
        owner.TryDecideSkill();

        if (owner.ConsumeSkillInput())
        {
            owner.ChangeState(owner.SkillState);
            return;
        }
        if (!owner.HasCurrentAction())
        {
            owner.SetCurrentAction(owner.GetDefaultAction());
            owner.ChangeState(owner.GetStateForCurrentAction());
        }

    }
}
