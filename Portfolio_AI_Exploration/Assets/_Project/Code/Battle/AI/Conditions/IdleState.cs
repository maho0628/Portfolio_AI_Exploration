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

        // まずスキル試行
        bool skillStarted = owner.TryDecideSkill();

        if (skillStarted)
        {
            return;
        }

        // 通常行動
        if (!owner.HasCurrentAction())
        {
            owner.SetCurrentAction(
                owner.GetDefaultAction()
            );

            owner.ChangeState(
                owner.GetStateForCurrentAction()
            );
        }


    }
}
