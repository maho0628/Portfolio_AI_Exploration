/// <summary>
/// 戦闘AIの各状態（Idle / Hold など）の基底クラス。
/// Unityの仕組みから切り離された「思考ロジック層」。
/// </summary>
public abstract class BattleStateBase
{
    /// <summary>
    /// このStateを所有している戦闘AI本体。
    /// HP・ゲージ・入力・遷移要求などは必ず owner 経由で行う。
    /// </summary>
    protected BattleAI owner;

    /// <summary>
    /// バトルAIを渡す
    /// </summary>
    /// <param name="owner"></param>
    protected BattleStateBase(BattleAI owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// AIの思考を1フレーム分進める。
    /// 行動実行ではなく「判断・遷移判定」を行う。
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// ステートに入るとき
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// ステートから抜けるとき（てかステートに抜ける時に処理挟んでないからバグった説）
    /// </summary>
    public virtual void OnExit() { }
}
