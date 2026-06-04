using UnityEngine;

public class AttackState : BattleStateBase
{
    private float timer; //まだ仮実装　


    public AttackState(BattleAI owner) : base(owner) { }

    public override void OnEnter()
    {
        timer = 0f;
        Debug.Log("Attack Start");
    }

    public override void Tick()
    {
        // デルタタイムで加算（将来変更の可能性あり）

        timer += Time.deltaTime;

        if (timer >= owner.CurrentAction.duration)
        {
            owner.ClearCurrentAction();
            owner.ChangeState(owner.IdleState);
        }
    }

    public override void OnExit()
    {
        Debug.Log("Attack End");
    }
}
