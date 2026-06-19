using UnityEngine;

/// <summary>
/// スキルを実行するステート。
/// スキルの発動、ダメージ適用、TP（必殺技ゲージ）の消費を行い、
/// 実行後に IdleState へ遷移する。
/// </summary>
public class SkillExecutionState : BattleState
{
    /// <summary>
    /// 現在実行されているスキル。
    /// </summary>
    private SkillSO currentSkill;

    /// <summary>
    /// SkillExecutionState を初期化する。
    /// </summary>
    /// <param name="owner">このステートを所有する BattleAI。</param>
    internal SkillExecutionState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// 実行するスキルを設定する。
    /// </summary>
    /// <param name="skill">実行対象のスキル</param>
    internal void SetSkill(SkillSO skill)
    {
        currentSkill = skill;
    }

    /// <summary>
    /// 現在設定されているスキルを取得する。
    /// </summary>
    /// <returns>実行対象のスキル</returns>
    internal SkillSO GetCurrentSkill()
    {
        return currentSkill;
    }

    /// <summary>
    /// ステート開始時にスキルを実行する。
    /// 必殺技の場合はP（必殺技ゲージ）を消費し、ダメージを適用する。
    /// </summary>
    internal override void OnEnter()
    {
        if (currentSkill == null)
        {
            owner.SwitchState(owner.IdleState);
            return;
        }

        // 必殺技ならTP消費
        if (currentSkill.SkillCategory == SkillType.Ultimate)
        {
            owner.BB.ResetTP();
        }

        owner.RunSkill();

        // ダメージ適用
        owner.DealDamage(currentSkill);
   
    }

    /// <summary>
    /// スキル実行後の後処理を行う。
    /// 必殺技の再実行制限を解除し、IdleStateへ遷移する。
    /// </summary>
    internal override void Tick()
    {

        //必殺技をもう一度打てるようにフラグをリセット
        owner.ResetUltimateLock();

        owner.SwitchState(owner.IdleState);

    }

}