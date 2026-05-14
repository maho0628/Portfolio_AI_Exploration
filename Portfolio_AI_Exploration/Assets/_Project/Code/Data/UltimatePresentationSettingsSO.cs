using UnityEngine;

[CreateAssetMenu(
    menuName = "Battle/UltimatePresentationSettings"
)]
public class UltimatePresentationSettingsSO
    : ScriptableObject
{
    public float darkFadeDuration = 0.2f;

    public float slowScale = 0.2f;

    public float slowDuration = 0.3f;

    public float whiteFlashDuration = 0.1f;
}