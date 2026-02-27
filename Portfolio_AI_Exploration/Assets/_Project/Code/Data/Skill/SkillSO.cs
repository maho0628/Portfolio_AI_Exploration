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

    [Header("Interrupt")]
    public bool canBeInterrupted;
    public bool canInterruptOthers;
}
