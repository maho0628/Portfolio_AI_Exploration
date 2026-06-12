using System;
using UnityEngine;

/// <summary>
/// 各skillの詳細情報のスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(menuName = "Battle/Skill")]
public class SkillSO : ScriptableObject
{
    /// <summary>
    /// スキルのタイプ　
    /// </summary>
    [SerializeField, Tooltip("スキルのタイプ")]
    private SkillType skillCategory;


    /// <summary>
    /// 威力倍率
    /// </summary>
    [SerializeField, Tooltip("威力倍率")]
    private int multiplier;


    /// <summary>
    /// 行動で増加するゲージ
    /// </summary>
    [SerializeField, Tooltip("行動で増加するゲージ")]
    private int tpGainOnHit;


    /// <summary>
    /// スキル発動までの待機時間
    /// </summary>
    [SerializeField, Tooltip("スキル発動までの待機時間")]

    private float castTime;






    #region  読み取り専用プロパティ (スキルの内部管理用変数)

    /// <summary>
    /// スキルのタイプ
    /// </summary>
    internal SkillType SkillCategory => skillCategory;


    /// <summary>
    /// 威力倍率
    /// </summary>
    internal int MultiPlier => multiplier;

    /// <summary>
    /// 行動で増加するゲージ
    /// </summary>
    internal int TPGainOnHit => tpGainOnHit;


    internal float CastTime => castTime;    

    #endregion

}
