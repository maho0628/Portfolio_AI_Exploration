/// <summary>
/// 戦闘AIの各状態（Idle / Hold など）の基底クラス。
/// Unity の仕組みから切り離された思考ロジック層として機能する。
/// </summary>
public abstract class BattleState
{
    /// <summary>
    /// このステートを所有する BattleAI。
    /// HP・TP（必殺技ゲージ）・入力・状態遷移は owner を通じて操作する。
    /// </summary>
    protected BattleAI owner;

    /// <summary>
    /// BattleStateBase を初期化する。
    /// </summary>
    /// <param name="owner">このステートを所有する BattleAI。</param>
    protected BattleState(BattleAI owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// AI の思考を 1 フレーム分進める。
    /// 状態遷移の判定や行動決定を行う。
    /// </summary>
    internal abstract void Tick();

    /// <summary>
    /// ステート開始時に呼ばれる。
    /// </summary>
    internal virtual void OnEnter() { }

    /// <summary>
    /// ステート終了時に呼ばれる。
    /// </summary>
    internal virtual void OnExit() { }
}
