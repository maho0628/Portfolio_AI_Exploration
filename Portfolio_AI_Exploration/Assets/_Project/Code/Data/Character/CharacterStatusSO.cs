using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// キャラクターごとのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(menuName = "Battle/Character Status")]
public class CharacterStatusSO : ScriptableObject
{
    /// <summary>
    /// 最大HP
    /// </summary>
    [Header("基本ステータス")]
    [SerializeField,Tooltip("最大HP")] 
    private int maxHP;

    /// <summary>
    /// 物理攻撃力
    /// </summary>
    [SerializeField,Tooltip("物理攻撃力")] 
    private int physicalAttack;

    /// <summary>
    /// 物理防御力
    /// </summary>
    [SerializeField,Tooltip("物理防御力")] 
    private int physicalDefense;

    /// <summary>
    /// 行動で増加するゲージの最大値
    /// </summary>
    [SerializeField,Tooltip("行動で増加するゲージの最大値")] 
    private int tpMax;


    /// <summary>
    /// 各キャラが持っているスキルの一覧および順番のリスト
    /// </summary>
    [Header("スキル情報")]
    [SerializeField,Tooltip("各キャラが持っているスキルの一覧および順番のリスト")] 
    private List<SkillSO> skillLoop= new List<SkillSO>();

    /// <summary>
    /// 最大HP
    /// </summary>
    internal int MaxHP => maxHP;

    /// <summary>
    /// 物理攻撃力
    /// </summary>
    internal int PhysicalAttack => physicalAttack;

    /// <summary>
    /// 物理防御力
    /// </summary>
    internal int PhysicalDefense => physicalDefense;

    /// <summary>
    /// 行動で増加するゲージの最大値
    /// </summary>
    internal int TPMax => tpMax;

    /// <summary>
    /// 各キャラが持っているスキルの一覧および順番のリスト
    /// </summary>
    internal List<SkillSO> SkillLoop => skillLoop;
}