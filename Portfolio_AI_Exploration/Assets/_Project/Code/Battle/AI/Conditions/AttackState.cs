using UnityEngine;

public class AttackState : BattleStateBase
{
    public AttackState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        // 仮：攻撃完了後Idleへ
        owner.ChangeState(owner.IdleState);
    }
}
