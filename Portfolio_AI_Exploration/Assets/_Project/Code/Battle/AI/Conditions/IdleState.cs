using UnityEngine;

/// <summary>
/// 何もしない処理状態ステート
/// </summary>
public class IdleState : BattleStateBase
{
    public IdleState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// ここ中に処理書いてないからリファクタリング後に使わなさそうなら消してもいいかも
    /// 
    /// </summary>
    public override void OnEnter()
    {
    }

    /// <summary>
    /// スキル発動を試行する。
    /// TryDecideSkill() が成功した場合は
    /// SkillStateへ遷移する。
    ///
    /// TODO:
    /// TryDecideSkill() が高頻度で成功するため
    /// HoldStateへ遷移する機会が少ない可能性あり。
    /// 実際の遷移フローを確認する。
    /// </summary>
    public override void Tick()
    {

        //キャラクター自体が死んでいたら処理しない
        if (owner.Blackboard.IsDead)
        {
            return;
        }

        // まずスキル試行と同時にほぼスキルステートに移行する
        bool skillStarted = owner.TryDecideSkill();

        //スキルが決まった後は下の処理を動かさない
        if (skillStarted)
        {
            return;
        }

        // スキルが決定していない時通常行動（これさ！boolを反転させてるの変じゃないかな？（HasCurrentActionの中身変えていいかもね）
        if (!owner.HasCurrentAction())
        {
            //ホールドステート用のアクションに移行
            owner.SetCurrentAction(
                owner.GetDefaultAction()
            );

            //ホールドステートに移行
            owner.ChangeState(
                owner.GetStateForCurrentAction()
            );
        }


    }
}
