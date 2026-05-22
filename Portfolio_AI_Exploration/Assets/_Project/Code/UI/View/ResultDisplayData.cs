using UnityEngine;

[CreateAssetMenu(menuName = "Game/Result Display Data")]
public class ResultDisplayData : ScriptableObject
{
    public ResultType resultType;

    public string resultText;
    public Color textColor;



    [Header("Button")]
    public string resultButtonText;
}