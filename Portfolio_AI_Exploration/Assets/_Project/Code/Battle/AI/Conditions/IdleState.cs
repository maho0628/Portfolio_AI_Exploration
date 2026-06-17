using UnityEngine;

/// <summary>
/// 待機状態を表すステート。
/// 次に実行可能なスキルを優先的に判定し、
/// 条件を満たさない場合はHoldStateへ遷移する。
/// </summary>
public class IdleState : BattleStateBase
{
    /// <summary>
    /// IdleState を初期化する。
    /// </summary>
    /// <param name="owner">このステートを所有する BattleAI。</param>
    internal IdleState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// 行動開始前の待機フェーズ。
    /// スキル実行条件を評価し、可能であればスキルを開始する。
    /// 実行できない場合はHoldStateへ遷移する。
    internal override void Tick()
    {
        // キャラクターが死亡している場合は処理しない
        if (owner.Blackboard.IsDead)
        {
            return;
        }

        // スキル実行を試みる（成功した場合はそのまま遷移先で処理が進む）
        bool skillStarted = owner.TryDecideSkill();

        if (skillStarted)
        {
            return;
        }

        // スキルが開始されなかった場合は通常待機へ
        owner.ChangeState(owner.HoldState);

    }
}
