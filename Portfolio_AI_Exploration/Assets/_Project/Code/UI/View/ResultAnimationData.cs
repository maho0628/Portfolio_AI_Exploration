using UnityEngine;

using DG.Tweening;

[CreateAssetMenu(menuName = "Game/Result Animation Data")]
public class ResultAnimationData : ScriptableObject
{
    [Header("Retry Hide")]
    public float retryFadeDelay = 0.7f;
    public float retryFadeDuration = 0.4f;
    public float retryShrinkScale = 0.9f;
    public Ease retryFadeEase = Ease.OutQuad;

   
}
