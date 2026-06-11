using UnityEngine;

/// <summary>
/// 死亡時のステートがあるから死亡時の関数ここでいいかもな
/// </summary>
public class DeadState : BattleStateBase
{
    /// <summary>
    /// 最初にDeadStateのインスタンスを用意するため
    /// ならパブリックじゃないかな（Internalでいいはず    
    ///これ参照してないな　死亡時の演出系ここのTickで書いてもいいかもな
    /// <param name="owner"></param>
    /// </summary>
    public DeadState(BattleAI owner) : base(owner) { }

    public override void Tick()
    {
        // 仮：攻撃完了後Idleへ
        owner.ChangeState(owner.IdleState);
    }
}