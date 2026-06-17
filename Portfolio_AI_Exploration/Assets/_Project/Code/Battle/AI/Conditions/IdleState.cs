using UnityEngine;

/// <summary>
/// 待機状態を表すステート。
/// スキル発動を優先して判定し、実行できない場合は通常行動を開始する。
/// </summary>
public class IdleState : BattleStateBase
{
    /// <summary>
    /// IdleState を初期化する。
    /// </summary>
    /// <param name="owner">このステートを所有する BattleAI。</param>
    internal IdleState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// スキル発動を優先して判定する。
    /// スキルを実行できない場合は、デフォルト行動を設定して通常行動を開始する。
    /// </summary>
    internal override void Tick()
    {
        // キャラクターが死亡している場合は処理しない
        if (owner.Blackboard.IsDead)
        {
            return;
        }

        // スキル発動を試行する
        bool skillStarted = owner.TryDecideSkill();

        // スキルが開始された場合は処理を終了する
        if (skillStarted)
        {
            return;
        }

        // 行動が設定されていない場合は通常行動を開始する
        if (!owner.HasCurrentAction())
        {
            // HoldState 用の行動を設定する
            owner.SetCurrentAction(owner.GetDefaultAction());

            // 対応するステートへ遷移する
            owner.ChangeState(owner.GetStateForCurrentAction());
        }

    }
}
