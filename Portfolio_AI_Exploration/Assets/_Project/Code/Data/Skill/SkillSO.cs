using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Skill")]
public class SkillSO : ScriptableObject
{
    public SkillType skillType;



    [Header("Power")]
    public int power;          // 威力倍率（例：100 = 100%）
    public DamageType damageType;
    [Header("Target")]
    public TargetType targetType;
    public int tpGainOnHit;
    public float duration;
    public string animationName;
    [SerializeField]
    private float interventionWindow = 0.3f;

    public float InterventionWindow => interventionWindow;

    [SerializeField]
    [Range(0f, 1f)]
    private float interventionTiming = 0.5f;

    public float InterventionTiming => interventionTiming;
    [Header("Interrupt")]
    public bool canBeInterrupted;
    public bool canInterruptOthers;
}
