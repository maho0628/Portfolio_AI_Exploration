using UnityEngine;

public class InteractTarget : MonoBehaviour
{
    public InteractRole role;
}

public enum InteractRole
{
    Enemy,
    Goal
}