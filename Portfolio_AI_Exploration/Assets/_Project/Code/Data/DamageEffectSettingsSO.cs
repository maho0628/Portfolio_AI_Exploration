using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Effect/DamageEffectSettings")]
public class DamageEffectSettingsSO : ScriptableObject
{
    [Header("Shake")]
    public float shakeDuration = 0.15f;

    public float shakeStrength = 0.15f;

    public int shakeVibrato = 20;


    //Time.timeScale = 0にしてしまうとDOTween、Untask、Update、全部止まる

    [Header("Hit Stop")]
    public float hitStopScale = 0.05f;

    public float hitStopDuration = 0.03f;
}