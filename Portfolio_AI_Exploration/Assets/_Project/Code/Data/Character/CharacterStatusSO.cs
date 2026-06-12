using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// キャラクターごとのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(menuName = "Battle/Character Status")]
public class CharacterStatusSO : ScriptableObject
{
    [Header("Base Status")]
    [SerializeField] private int maxHP;
    [SerializeField] private int physicalAttack;
    [SerializeField] private int magicAttack;
    [SerializeField] private int physicalDefense;
    [SerializeField] private int magicDefense;

    [Header("TP")]
    [SerializeField] private int tpMax;

    [Header("Skills")]
    [SerializeField] private List<SkillSO> skillLoop= new List<SkillSO>();

    public int MaxHP => maxHP;
    public int PhysicalAttack => physicalAttack;
    public int MagicAttack => magicAttack;
    public int PhysicalDefense => physicalDefense;
    public int MagicDefense => magicDefense;
    public int TPMax => tpMax;
    public List<SkillSO> SkillLoop => skillLoop;
}