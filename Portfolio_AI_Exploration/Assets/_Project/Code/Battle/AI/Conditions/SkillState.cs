using UnityEngine;

public class SkillState : BattleStateBase
{
    //TODO:　実行するかどうかのフラグいらないかどうかの判断　いる場合はTickのIdle処理の条件を見直す
    //private bool executed;
    private SkillSO currentSkill;

    private float timer;
    private bool executed;
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
        executed = false;

        timer = 0f;
        if (currentSkill.skillType == SkillType.Ultimate)
        {
            owner.Blackboard.ResetTP();
        }
        //executed = false;
        //Debug.Log($"Skill Start: {currentSkill.skillType}");
        Debug.Log($"{owner.name} Skill Start");

        //executed = true;
    }

    public override void Tick()
    {
        if (owner.Blackboard.IsDead) return;

        timer += Time.deltaTime;

        float hitTiming =
    currentSkill.duration *
    currentSkill.InterventionTiming;

        float window =
            currentSkill.InterventionWindow;

        bool inWindow =
            timer >= hitTiming - window &&
            timer <= hitTiming + window;

        owner.SetInterventionWindow(inWindow);
        // 途中で攻撃発生
        if (!executed && timer >= currentSkill.duration * 0.5f)
        {
            executed = true;

            owner.ExecuteSkill();

            owner.DealDamage(currentSkill);
        }

        // 終了
        if (timer >= currentSkill.duration)
        {
            owner.ResetUltimateLock();

            owner.ChangeState(owner.IdleState);
        }
    }
}