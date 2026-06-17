using DG.Tweening;
using UnityEngine;

/// <summary>
/// 敵キャラクター用の BattleAI。
/// </summary>
public class EnemyBattleAI : BattleAI
{
    /// <summary>
    /// ヒットストップや画面シェイクをする関数
    /// エネミー特有の演出になるように調整
    /// </summary>
    protected override void PlayDamageReaction()
    {
        base.PlayDamageReaction();

        var enemySettings = DamageEffectSettings as EnemyDamageEffectSettingsSO;

        
        transform.DOMoveX(transform.position.x - enemySettings.KnockbackDistance,
    enemySettings.KnockbackDuration).SetEase(enemySettings.KnockbackEase)
.SetLoops(enemySettings.KnockbackLoops, LoopType.Yoyo);
    }

    /// <summary>
    /// ダメージを受け、HPを減少させる。
    /// 被弾演出とダメージ表示も実行する。
    /// この派生クラスでは追加でエネミー被弾時のSEを鳴らす
    /// </summary>
    /// <param name="amount">受けるダメージ量</param>
    protected override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.EnemyHit);

    }

    /// <summary>
    /// 死亡時の演出を行うための関数
    /// エネミー死亡時のSEを鳴らす
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.EnemyDeath);
    }

    

}
