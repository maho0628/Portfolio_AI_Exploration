public class HoldState : BattleStateBase
{
    public HoldState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        if (owner.HasSkillInput())
        {
            owner.ExecuteSkill();
            owner.ChangeState(owner.IdleState);
        }
        else if (owner.HasWaitInput())
        {
            owner.ChangeState(owner.IdleState);
        }
    }
}
