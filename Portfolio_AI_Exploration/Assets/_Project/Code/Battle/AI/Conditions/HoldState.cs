using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class HoldState : BattleStateBase
{
    private float timer;

    public HoldState(BattleAI owner) : base(owner) { }

    public override void OnEnter()
    {
        timer = 0f;
        Debug.Log($"[Hold] Start (duration={owner.CurrentAction?.duration})");
    }

    public override void Tick()
    {

        if (owner.ConsumeSkillInput())
        {

            Debug.Log("[Hold] Interrupt: SkillInput");
            owner.ClearCurrentAction();
            owner.ChangeState(owner.IdleState);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= owner.CurrentAction.duration)
        {
            Debug.Log("[Hold] Timeout -> Idle");
            owner.ClearCurrentAction();
            owner.ChangeState(owner.IdleState);
        }
        
    }


    public override void OnExit()
    {
        Debug.Log("Hold End");
    }
}
