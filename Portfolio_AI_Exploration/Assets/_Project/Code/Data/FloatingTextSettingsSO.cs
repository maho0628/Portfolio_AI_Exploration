using UnityEngine;

[CreateAssetMenu(
    menuName = "Battle/UI/FloatingTextSettingsSO"
)]
public class FloatingTextSettingsSO
    : ScriptableObject
{
    [Header("Move")]
    public float moveY = 80f;

    public float duration = 0.5f;

    [Header("Spawn Offset")]
    public Vector2 randomOffsetMin;

    public Vector2 randomOffsetMax;
}
