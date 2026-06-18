using DG.Tweening;
using UnityEngine;

/// <summary>
/// 敵キャラクター用の BattleAI。
/// 被弾時のノックバック演出やSE再生など、敵固有の処理を追加する。
/// </summary>
public class EnemyBattleAI : BattleAI
{
    /// <summary>
    /// 被弾演出を再生する。
    /// 基本演出に加えて、敵固有のノックバック演出を行う。
    /// </summary>
    protected override void PlayDamageReaction()
    {
        base.PlayDamageReaction();

        var enemySettings = DamageEffectSettings as EnemyDamageEffectSettingsSO;
        if (enemySettings == null)
        {
            DebugManager.LogError( "EnemyBattleAI requires EnemyDamageEffectSettingsSO.");
            return;
        }

        //ノックバック演出を行う
        transform
         .DOMoveX(
             transform.position.x - enemySettings.KnockbackDistance,
             enemySettings.KnockbackDuration)
         .SetEase(enemySettings.KnockbackEase)
         .SetLoops(enemySettings.KnockbackLoops, LoopType.Yoyo);
    }

    /// <summary>
    /// 被弾時の処理を行う。
    /// 基本処理に加えて、敵専用の被弾SEを再生する。
    /// </summary>
    /// <param name="amount">受けるダメージ量</param>
    protected override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.EnemyHit);

    }

    /// <summary>
    /// 死亡時の処理を行う。
    /// 基本処理に加えて、敵専用の死亡SEを再生する。
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.EnemyDeath);
    }

}
