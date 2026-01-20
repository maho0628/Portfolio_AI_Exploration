using UnityEngine;

public class InvestigateState : BattleStateBase
{
    public InvestigateState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        // 仮：攻撃完了後Idleへ
        owner.ChangeState(owner.IdleState);
    }
}