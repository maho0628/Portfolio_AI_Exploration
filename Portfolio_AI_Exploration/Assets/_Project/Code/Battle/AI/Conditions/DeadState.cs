using UnityEngine;

public class DeadState : BattleStateBase
{
    public DeadState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        // 仮：攻撃完了後Idleへ
        owner.ChangeState(owner.IdleState);
    }
}