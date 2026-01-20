using UnityEngine;

public class SkillState : BattleStateBase
{
    public SkillState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        // 仮：攻撃完了後Idleへ
        owner.ChangeState(owner.IdleState);
    }
}