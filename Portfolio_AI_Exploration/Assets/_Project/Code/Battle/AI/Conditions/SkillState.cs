using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class SkillState : BattleStateBase
{
    private bool executed;

    public SkillState(BattleAI owner) : base(owner) { }


    public override void OnEnter()
    {
        executed = false;   

        Debug.Log("Skill Start");
        owner.ExecuteSkill();
        executed = true;
    }

    public override void Tick()
    {
        if (executed)
        {
            Debug.Log("Skill End");
            owner.ChangeState(owner.IdleState);
        }
    }
}