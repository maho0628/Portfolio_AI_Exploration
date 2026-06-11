using UnityEngine;

/// <summary>
/// 攻撃ステートってかここで通常の、Skillをやるべきかも（けど処理的におかしくなるかもだから要検討
/// </summary>
public class AttackState : BattleStateBase
{

    /// <summary>
    /// なぜ仮実装のまま動いてるのよ
    /// タイマーでIdleステートへ遷移か怪しいけど要検討かな
    /// </summary>
    private float timer; //まだ仮実装　


    /// <summary>
    /// 最初にAttackStateのインスタンスを用意するため
    /// ならパブリックじゃないかな（Internalでいいはず
    /// </summary>
    /// <param name="owner"></param>
    public AttackState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// ステートに入った時に呼ばれる
    /// タイマーを０にするだけだからこれだけでいいのかは不明
    /// 
    /// </summary>
    public override void OnEnter()
    {
        timer = 0f;
        Debug.Log("Attack Start");
    }

    /// <summary>
    /// アタックステートのTick
    /// 一定時間でアイドルに戻す
    /// </summary>
    public override void Tick()
    {
        // デルタタイムで加算（将来変更の可能性あり）

        timer += Time.deltaTime;

        //一定時間でアイドルに戻す
        if (timer >= owner.CurrentAction.duration)
        {
            owner.ClearCurrentAction();
            owner.ChangeState(owner.IdleState);
        }
    }

    /// <summary>
    /// ステートから抜ける
    /// </summary>
    public override void OnExit()
    {
        Debug.Log("Attack End");
    }
}
