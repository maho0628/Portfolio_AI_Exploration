using UnityEngine;

/// <summary>
/// スキルの設定情報を保持する ScriptableObject。
/// </summary>
[CreateAssetMenu(menuName = "Battle/Skill")]
public class SkillSO : ScriptableObject
{
    /// <summary>
    /// スキルのカテゴリ。
    /// </summary>
    [SerializeField, Tooltip("スキルのカテゴリ。")]
    private SkillType skillCategory;

    /// <summary>
    /// スキルの威力倍率（1.0 = 100%）。
    /// </summary>
    [SerializeField, Tooltip("スキルの威力倍率（1.0 = 100%）。")]
    [Range(0f, 20f)]
    private float multiplier;

    /// <summary>
    /// スキル命中時に増加するTP（必殺技ゲージ）量。
    /// </summary>
    [SerializeField, Tooltip("スキル命中時に増加するTP（必殺技ゲージ）量。")]
    private int tpGainOnHit;

    /// <summary>
    /// スキル発動までの待機時間。
    /// </summary>
    [SerializeField, Tooltip("スキル発動までの待機時間。")]
    private float castTime;

    /// <summary>
    /// スキルの最低保証ダメージ。
    /// </summary>
    [SerializeField, Tooltip("スキルの最低保証ダメージ。")]
    private int minimumDamage = 1;


    #region 読み取り専用プロパティ

    /// <summary>
    /// スキルのカテゴリ。
    /// </summary>
    internal SkillType SkillCategory => skillCategory;

    /// <summary>
    /// スキルの威力倍率（1.0 = 100%）。
    /// </summary>
    internal float Multiplier => multiplier;

    /// <summary>
    /// スキル命中時に増加するTP（必殺技ゲージ）量。
    /// </summary>
    internal int TPGainOnHit => tpGainOnHit;

    /// <summary>
    /// スキル発動までの待機時間。
    /// </summary>
    internal float CastTime => castTime;

    /// <summary>
    /// スキルの最低保証ダメージ。
    /// </summary>
    internal int MinimumDamage => minimumDamage;

    #endregion

}
