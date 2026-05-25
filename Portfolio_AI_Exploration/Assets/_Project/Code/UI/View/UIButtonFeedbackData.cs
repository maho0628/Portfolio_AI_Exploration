using UnityEngine;


[CreateAssetMenu(menuName = "UI/Button Feedback Data")]
public class UIButtonFeedbackData : ScriptableObject
{
    public float clickScale = 0.92f;
    public float hoverScale = 1.08f;
    public float normalScale = 1f;

    public float clickDuration = 0.08f;
    public float hoverDuration = 0.15f;
}
