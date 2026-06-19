using UnityEngine;

/// <summary>
/// 被弾時の演出設定を保持するスクリプタブルオブジェクト。
/// 画面シェイクとヒットストップの各種パラメータを管理する。
/// </summary>
[CreateAssetMenu(menuName = "Battle/Effect/DamageEffectSettings")]
public class DamageEffectSettingsSO : ScriptableObject
{
    /// <summary>
    /// シェイク演出の継続時間（秒）。
    /// </summary>
    [Header("Shake")]
    [SerializeField, Min(0f), Tooltip("シェイク演出の継続時間（秒）。")]
    private float shakeDuration = 0.15f;

    /// <summary>
    /// シェイク演出の揺れ幅。
    /// </summary>
    [SerializeField, Min(0f), Tooltip("シェイク演出の揺れ幅。")]
    private float shakeStrength = 0.15f;

    /// <summary>
    /// シェイク演出の振動回数。
    /// </summary>
    [SerializeField, Min(0), Tooltip("シェイク演出の振動回数。")]
    private int shakeVibrato = 20;


    [Header("Hit Stop")]

    /// ヒットストップ中の Time.timeScale 値。
    /// </summary>
    [SerializeField, Range(0f, 1f), Tooltip("ヒットストップ中の Time.timeScale 値。")]
    private float hitStopScale = 0.05f;

    /// <summary>
    /// ヒットストップの継続時間（秒）。
    /// </summary>
    [SerializeField, Min(0f), Tooltip("ヒットストップの継続時間（秒）。")]
    private float hitStopDuration = 0.03f;


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

    /// <summary>
    /// シェイク演出の継続時間取得する。
    /// </summary>
    internal float ShakeDuration => shakeDuration;

    /// <summary>
    /// シェイク演出の揺れ幅を取得する。
    /// </summary>
    internal float ShakeStrength => shakeStrength;

    /// <summary>
    /// シェイク演出の振動回数を取得する。
    /// </summary>
    internal int ShakeVibrato => shakeVibrato;

    /// <summary>
    /// ヒットストップ中の Time.timeScale 値を取得する。
    /// </summary>
    internal float HitStopScale => hitStopScale;

    /// <summary>
    ///  ヒットストップの継続時間（秒）を取得する。
    /// </summary>
    internal float HitStopDuration => hitStopDuration;

    #endregion

}