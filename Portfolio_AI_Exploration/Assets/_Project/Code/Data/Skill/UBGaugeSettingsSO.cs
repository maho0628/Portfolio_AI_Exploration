using UnityEngine;


/// <summary>
/// UBゲージの表示演出に使用する設定を保持するスクリプタブルオブジェクト。
/// ゲージ更新速度や発光演出の設定を管理する。
/// </summary>
[CreateAssetMenu(menuName = "Battle/UI/UBGaugeSettings")]
public class UBGaugeSettingsSO : ScriptableObject
{

    /// <summary>
    /// ゲージの補間アニメーション時間（秒）。
    /// </summary>
    [Header("Gauge Animation")]
    [SerializeField, Min(0f)]
    [Tooltip("ゲージの補間アニメーション時間（秒）")]
    private float fillDuration = 0.25f;

    /// <summary>
    /// 警告演出を開始するTP割合。
    /// </summary>
    [Header("Threshold")]
    [SerializeField, Range(0f, 1f)]
    [Tooltip("警告演出を開始するTP割合")]
    private float warningThreshold = 0.8f;

    /// <summary>
    /// 警告状態の発光演出設定。
    /// </summary>
    [Header("Glow Settings")]
    [SerializeField,Tooltip("警告状態の発光演出設定")]
    private GlowSettings warningGlow;

    /// <summary>
    /// ゲージ最大時の発光演出設定。
    /// </summary>
    [SerializeField,Tooltip("ゲージ最大時の発光演出設定")]
    private GlowSettings maxGlow;


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

    /// <summary>
    /// ゲージの補間アニメーション時間（秒）。
    /// </summary>
    internal float FillDuration => fillDuration;

    /// <summary>
    /// 警告演出を開始するTP割合。
    /// </summary>
    internal float WarningThreshold => warningThreshold;

    /// <summary>
    /// 警告状態の発光演出設定。
    /// </summary>
    internal GlowSettings WarningGlow => warningGlow;

    /// <summary>
    /// ゲージ最大時の発光演出設定。
    /// </summary>
    internal GlowSettings MaxGlow => maxGlow;

    #endregion

}

