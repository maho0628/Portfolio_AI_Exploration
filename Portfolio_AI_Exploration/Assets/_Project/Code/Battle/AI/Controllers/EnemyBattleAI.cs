using UnityEngine;


public class EnemyBattleAI : BattleAI
{
    protected override void Update()
    {
        base.Update();

        if (Blackboard.IsDead)
            return;

     
    }

    public override bool HasWaitInput()
    {
        return false; // 敵は待たない
    }

    public override void ReceivePlayerCommand(PlayerCommand command)
    {
        Debug.LogError("Enemy received input!?");
    }
    public override BattleAction GetDefaultAction()
    {
        return new BattleAction
        {
            actionType = ActionType.Attack,
            duration = 1.5f
        };
    }
}