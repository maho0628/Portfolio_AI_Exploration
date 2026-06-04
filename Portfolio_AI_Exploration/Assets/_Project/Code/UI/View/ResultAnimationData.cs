using UnityEngine;

using DG.Tweening;

[CreateAssetMenu(menuName = "Game/Result Animation Data")]
public class ResultAnimationData : ScriptableObject
{

    [Header("RESULT")]
    public float resultFadeDuration = 0.1f;
    public float resultScaleUp = 1.08f;
    public float resultScaleBack = 0.98f;
    public float resultScaleNormal = 1f;
    public Ease resultEase = Ease.OutBack;

    [Header("INTERVAL")]
    public float betweenDelayShort = 0.05f;
    public float betweenDelayMid = 0.08f;
    public float betweenDelayLong = 0.12f;

    [Header("SUCCESS RATE")]
    public float successFadeDuration = 0.28f;
    public float successMoveOffsetY = 12f;
    public float successScaleUp = 1.06f;
    public float successScaleDuration = 0.08f;
    public Ease successEase = Ease.OutCubic;

    [Header("INTERVENTION")]
    public float interventionFadeDuration = 0.25f;
    public float interventionMoveOffsetY = 12f;
    public Ease interventionEase = Ease.OutCubic;

    [Header("Retry Hide")]
    public float retryFadeDelay = 0.7f;
    public float retryFadeDuration = 0.4f;
    public float retryShrinkScale = 0.9f;
    public Ease retryFadeEase = Ease.OutQuad;


}
