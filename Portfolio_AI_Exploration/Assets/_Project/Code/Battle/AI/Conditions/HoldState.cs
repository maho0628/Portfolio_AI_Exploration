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
        owner.SetInterventionWindow(true);
    }

    /// <summary>
    /// 介入成功時に手動介入要求を認めてIdleに戻す
    /// </summary>
    public override void Tick()
    {

        if (owner.ConsumeManualSkillRequest()
            && owner.IsGaugeFull())
        {
            owner.SetPendingSkill(
                owner.GetUltimateSkill()
            );
        }
     

        timer += Time.deltaTime;

        if (timer >= owner.GetPendingSkill().CastTime)
        {

            Debug.Log($"pendingSkill = {owner.GetPendingSkill()}");

            owner.SkillState.SetSkill(
                owner.GetPendingSkill());

            owner.ChangeState(owner.SkillState);
        }

        
    }

    /// <summary>
    /// 介入成功時に手動介入要求を認めたら
    /// </summary>
    public override void OnExit()
    {
        Debug.Log("Hold End");
        owner.SetInterventionWindow(false);

    }
}
