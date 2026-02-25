using UnityEngine;

public class SkillState : BattleStateBase
{
    //TODO:　実行するかどうかのフラグいらないかどうかの判断　いる場合はTickのIdle処理の条件を見直す
    //private bool executed;
    private SkillSO currentSkill;

    private float timer;

    public SkillState(BattleAI owner) : base(owner) { }

    public void SetSkill(SkillSO skill)
    {
        currentSkill = skill;
    }

    public SkillSO GetCurrentSkill()
    {
        return currentSkill;
    }
    public override void OnEnter()
    {
        if (currentSkill.skillType == SkillType.Ultimate)
        {
            owner.Blackboard.ResetTP();
        }
        //executed = false;
        Debug.Log($"Skill Start: {currentSkill.skillType}");
        timer = 0.0f;
        Debug.Log("Skill Start");
        owner.ExecuteSkill();
        //executed = true;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= currentSkill.duration)
        {
            owner.ChangeState(owner.IdleState);
        }
    }
}