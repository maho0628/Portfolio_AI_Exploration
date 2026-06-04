using UnityEngine;



[CreateAssetMenu(menuName = "Battle/UI/UBGaugeSettings")]
public class UBGaugeSettingsSO : ScriptableObject
{
    [Header("Gauge Animation")]
    public float fillDuration = 0.25f;

    [Header("Pulse")]
    public float pulseScale = 1.05f;
    public float pulseDuration = 0.4f;

    [Header("Threshold")]
    [Range(0f, 1f)]
    public float warningThreshold = 0.8f;

    [Header("Glow Settings")]
    public GlowSettings warningGlow;
    public GlowSettings maxGlow;
}

[System.Serializable]
public class GlowSettings
{
    [Range(0f, 1f)]
    public float alpha = 0.5f;

    public float duration = 0.5f;
}