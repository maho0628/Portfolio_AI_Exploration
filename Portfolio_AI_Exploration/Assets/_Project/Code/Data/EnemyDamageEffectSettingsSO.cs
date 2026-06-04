using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Battle/Effect/EnemyDamageEffectSettings"
)]
public class EnemyDamageEffectSettingsSO
    : DamageEffectSettingsSO
{
    [Header("Knockback")]
    public float knockbackDistance = 0.8f;

    public float knockbackDuration = 0.08f;

    public Ease knockbackEase = Ease.OutQuad;

    public int knockbackLoops = 2;
}