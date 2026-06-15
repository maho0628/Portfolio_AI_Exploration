using DG.Tweening;
using UnityEngine;


public class EnemyBattleAI : BattleAI
{
    protected override void Update()
    {
        base.Update();

        if (Blackboard.IsDead)
            return;


    }



    internal override void PlayDamageReaction()
    {
        base.PlayDamageReaction();

        var enemySettings =
          damageEffectSettings
          as EnemyDamageEffectSettingsSO;

        transform.DOMoveX(
    transform.position.x
        - enemySettings.knockbackDistance,

    enemySettings.knockbackDuration
)
.SetEase(enemySettings.knockbackEase)
.SetLoops(
    enemySettings.knockbackLoops,
    LoopType.Yoyo
);
    }

    internal override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.EnemyHit);

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.EnemyDeath);    
    }
    public override void ReceivePlayerCommand(PlayerCommand command)
    {
        DebugManager.LogError("Enemy received input!?");
    }
    public override BattleAction GetDefaultAction()
    {
        return new BattleAction
        {
            actionType = ActionType.Attack,
            duration = 1.5f
        };
    }
}
