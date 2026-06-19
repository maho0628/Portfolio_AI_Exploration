using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// キャラクターの基礎ステータスと使用スキルを定義する。
/// 戦闘中に参照する初期データとして使用する。
/// </summary>
[CreateAssetMenu(menuName = "Battle/Character Status")]
public class CharacterStatusSO : ScriptableObject
{
    /// <summary>
    /// キャラクターの最大HP
    /// </summary>
    [Header("基本ステータス")]
    [SerializeField, Tooltip("キャラクターの最大HP")]
    private int maxHP;

    /// <summary>
    /// キャラクターの物理攻撃力
    /// </summary>
    [SerializeField, Tooltip("キャラクターの物理攻撃力")]
    private int physicalAttack;

    /// <summary>
    /// キャラクターの物理防御力
    /// </summary>
    [SerializeField, Tooltip("キャラクターの物理防御力")]
    private int physicalDefense;

    /// <summary>
    /// スキル命中時に増加するTP（必殺技ゲージ）量の最大値
    /// </summary>
    [SerializeField, Tooltip("スキル命中時に増加するTP（必殺技ゲージ）量の最大値")]
    private int tpMax;


    /// <summary>
    /// キャラクターが使用するスキルの一覧と実行順序
    /// </summary>
    [Header("スキル情報")]
    [SerializeField, Tooltip("キャラクターが使用するスキルの一覧と実行順序")]
    private List<SkillSO> skillLoop = new List<SkillSO>();


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

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
    ///スキル命中時に増加するTP（必殺技ゲージ）量の最大値
    /// </summary>
    internal int TPMax => tpMax;

    /// <summary>
    /// キャラクターが使用するスキルの一覧と実行順序
    /// </summary>
    internal IReadOnlyList<SkillSO> SkillLoop => skillLoop;

    #endregion

}