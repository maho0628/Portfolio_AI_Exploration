using DG.Tweening;
using UnityEngine;

/// <summary>
/// 敵キャラクターの被弾演出設定を保持するスクリプタブルオブジェクト。
/// DamageEffectSettingsSO を継承し、ノックバック演出の設定を追加する。
/// </summary>
[CreateAssetMenu( menuName = "Battle/Effect/EnemyDamageEffectSettings")]
public class EnemyDamageEffectSettingsSO : DamageEffectSettingsSO
{
    /// <summary>
    /// ノックバック時の移動距離。
    /// </summary>
    [Header("Knockback")]
    [SerializeField, Min(0f), Tooltip("ノックバック時の移動距離")]
    private float knockbackDistance = 0.8f;

    /// <summary>
    /// ノックバック演出の継続時間（秒）。
    /// </summary>
    [SerializeField, Min(0f), Tooltip("ノックバック演出の継続時間（秒）")]
    private float knockbackDuration = 0.08f;

    /// <summary>
    /// ノックバック演出のイージング設定。
    /// </summary>
    [SerializeField, Tooltip("ノックバック演出のイージング設定")]
    private Ease knockbackEase = Ease.OutQuad;

    /// <summary>
    /// ノックバック演出の往復回数。
    /// </summary>
    [SerializeField, Min(1), Tooltip("ノックバック演出の往復回数")]
    private int knockbackLoops = 2;


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

    /// <summary>
    /// ノックバック時の移動距離。
    /// </summary>
    internal float KnockbackDistance => knockbackDistance;

    /// <summary>
    /// ノックバック演出の継続時間（秒）。
    /// </summary>
    internal float KnockbackDuration => knockbackDuration;

    /// <summary>
    /// ノックバック演出のイージング設定。
    /// </summary>
    internal Ease KnockbackEase => knockbackEase;

    /// <summary>
    /// ノックバック演出の往復回数。
    /// </summary>
    internal int KnockbackLoops => knockbackLoops;

    #endregion

}


