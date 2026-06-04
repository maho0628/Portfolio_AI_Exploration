using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/MoveSettings")]
public class ExplorationMoveSettingsSO : ScriptableObject
{
    [Header("Arrival")]
    public float arrivalThreshold = 0.05f;

    [Header("Footstep")]
    public float footstepInterval = 0.4f;

    [SerializeField]
    public float footstepMoveThreshold = 0.01f;
}
