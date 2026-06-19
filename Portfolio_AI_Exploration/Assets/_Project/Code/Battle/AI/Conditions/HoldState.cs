using UnityEngine;

/// <summary>
/// スキル実行前の待機状態を管理するステート。
/// プレイヤーからの手動介入を受け付け、実行するスキルを確定する。
/// 待機時間の経過後、SkillExecutionStateへ遷移する。
/// </summary>
public class SkillPreparationState : BattleState
{
    /// <summary>
    /// スキル発動までの経過時間。
    /// </summary>
    private float skillCastTimer;

    /// <summary>
    /// SkillPreparationState を初期化する。
    /// </summary>
    /// <param name="owner">このステートを所有する BattleAI。</param>
    internal SkillPreparationState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// 待機時間を初期化し、手動介入の受付を開始する。
    /// </summary>
    internal override void OnEnter()
    {
        skillCastTimer = 0f;
        owner.SetManualInterventionEnabled(true);
    }

    /// <summary>
    /// 手動介入要求を処理し、待機時間の経過後に実行予定のスキルを SkillExecutionState に引き継ぐ。
    /// 手動介入が成功した場合は、実行予定のスキルを必殺技に更新する。
    /// </summary>
    internal override void Tick()
    {
        if (owner.ConsumeManualInterventionRequest() && owner.IsGaugeFull())
        {
            owner.SetNextSkill(owner.GetUltimateSkill());
        }

        skillCastTimer += Time.deltaTime;

        if (skillCastTimer >= owner.GetNextSkill().CastTime)
        {
            owner.SkillExecutionState.SetSkill(owner.GetNextSkill());

            owner.SwitchState(owner.SkillExecutionState);
        }

    }

    /// <summary>
    /// 手動介入の受付を終了する。
    /// </summary>
    internal override void OnExit()
    {

        owner.SetManualInterventionEnabled(false);

    }
}
