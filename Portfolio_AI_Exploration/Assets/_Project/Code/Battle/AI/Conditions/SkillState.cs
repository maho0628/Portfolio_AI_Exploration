using UnityEngine;

/// <summary>
/// スキル実行ステート
/// OnEnterでスキルを実行し、
/// 次フレームでIdleへ戻る
/// </summary>
public class SkillState : BattleStateBase
{
    /// <summary>
    /// 現在のskill
    /// </summary>
    //TODO:　実行するかどうかのフラグいらないかどうかの判断　いる場合はTickのIdle処理の条件を見直す
    private SkillSO currentSkill;




    /// <summary>
    /// 最初にSkillStateのインスタンスを用意するため
    /// ならパブリックじゃないかな（Internalでいいはず
    /// </summary>
    /// <param name="owner"></param>
    public SkillState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// 実行するスキルを設定する
    /// </summary>
    /// <param name="skill"></param>
    public void SetSkill(SkillSO skill)
    {
        currentSkill = skill;
    }

    /// <summary>
    /// 現在どのスキルを持ってるのかを取得するための関数
    /// </summary>
    /// <returns></returns>
    public SkillSO GetCurrentSkill()
    {
        return currentSkill;
    }

    /// <summary>
    /// ステートに入った直後
    /// スキルの実行とダメージ処理
    /// UBだったらTPを０にする
    ///
    /// </summary>
    public override void OnEnter()
    {
        DebugManager.Log($"Skill Enter {Time.time}");
        if (currentSkill == null)
        {
            owner.ChangeState(owner.IdleState);
            return;
        }

        // UBならTP消費
        if (currentSkill.SkillCategory == SkillType.Ultimate)
        {
            owner.Blackboard.ResetTP();
        }

        // スキル実行
        owner.ExecuteSkill();

        // ダメージ適用
        owner.DealDamage(currentSkill);
   

    }

    /// <summary>
    /// スキルステートのTick
    /// スキルが実行され終わっていた場合はUB再生中のフラグを元に戻してIdleに遷移
    /// </summary>
    public override void Tick()
    {



        //UBをもう一度打てるようにフラグをリセット
        owner.ResetUltimateLock();

        //Idleに戻す
        owner.ChangeState(owner.IdleState);

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}