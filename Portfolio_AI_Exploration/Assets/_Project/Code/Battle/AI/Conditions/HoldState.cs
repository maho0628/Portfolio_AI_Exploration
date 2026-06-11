using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// ホールドステート　ここで介入成功かどうかを判断する必要がある
/// </summary>
public class HoldState : BattleStateBase
{
    /// <summary>
    /// なぜ仮実装のまま動いてるのよ
    /// タイマーでIdleステートへ遷移か怪しいけど要検討かな
    /// </summary>
    private float timer;

    /// <summary>
    /// 最初にHoldStateのインスタンスを用意するため
    /// ならパブリックじゃないかな（Internalでいいはず
    /// </summary>
    /// <param name="owner"></param>
    public HoldState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// ステートに入った時に呼ばれる
    /// タイマーを０にするだけだからこれだけでいいのかは不明
    /// </summary>
    public override void OnEnter()
    {
        timer = 0f;
        Debug.Log($"[Hold] Start (duration={owner.CurrentAction?.duration})");
    }

    /// <summary>
    /// 介入成功時に手動介入要求を認めてIdleに戻す
    /// </summary>
    public override void Tick()
    {

        // 介入成功時に手動介入要求を認めたら
        // Holdを即終了してIdleへ戻す
        if (owner.ConsumeManualSkillRequest())
        {
            Debug.Log("[Hold] Interrupt: ManualSkill");



            owner.ClearCurrentAction();

            owner.ChangeState(owner.IdleState);

            return;
        }

        timer += Time.deltaTime;

        //一定時間経過したらアイドルに戻す
        //// この処理自体は必要そう
        // ただ HoldState の責務が今後増えるなら
        // タイムアウトだけで終わる設計かは要検証

        if (timer >= owner.CurrentAction.duration)
        {
            Debug.Log("[Hold] Timeout -> Idle");
            owner.ClearCurrentAction();
            owner.ChangeState(owner.IdleState);
        }
        
    }

    /// <summary>
    /// 介入成功時に手動介入要求を認めたら
    /// </summary>
    public override void OnExit()
    {
        Debug.Log("Hold End");
    }
}
