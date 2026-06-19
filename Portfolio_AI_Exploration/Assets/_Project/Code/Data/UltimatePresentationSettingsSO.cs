using UnityEngine;

/// <summary>
/// 必殺技演出に使用する各種パラメータを管理するScriptableObject。
/// UIフェード・スローモーション・フラッシュ演出などの
/// 視覚演出の調整値を一元管理する。
/// </summary>
[CreateAssetMenu(menuName = "Battle/UltimatePresentationSettings")]
public class UltimatePresentationSettingsSO : ScriptableObject
{
    // ==================================================
    // Dark Panel Settings
    // ==================================================

    #region Dark Panel Settings

    /// <summary>
    /// 暗転時のパネルアルファ値。
    /// 画面をどの程度暗くするかを制御する。
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    private float darkPanelAlpha = 0.7f;

    /// <summary>
    /// 暗転フェードにかかる時間（秒）。
    /// 画面が暗くなるスピードを制御する。
    /// </summary>
    [SerializeField, Min(0f)]
    private float darkFadeDuration = 0.2f;

    #endregion


    // ==================================================
    // Slow Motion Settings
    // ==================================================

    #region Slow Motion Settings

    /// <summary>
    /// スローモーション時の時間倍率。
    /// 0に近いほど強いスロー演出になる。
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    private float slowScale = 0.2f;

    /// <summary>
    /// スローモーション演出の持続時間（秒）。
    /// </summary>
    [SerializeField, Min(0f)]
    private float slowDuration = 0.3f;

    #endregion


    // ==================================================
    // White Flash Settings
    // ==================================================

    #region White Flash Settings

    /// <summary>
    /// 白フラッシュ演出のフェードアウト時間（秒）。
    /// 必殺技発動時の強い発光演出の長さを制御する。
    /// </summary>
    [SerializeField, Min(0f)]
    private float whiteFlashDuration = 0.1f;

    #endregion


    // ==================================================
    // Properties
    // ==================================================

    #region Properties

    /// <summary>
    /// 暗転パネルのアルファ値。
    /// </summary>
    internal float DarkPanelAlpha => darkPanelAlpha;

    /// <summary>
    /// 暗転フェード時間。
    /// </summary>
    internal float DarkFadeDuration => darkFadeDuration;

    /// <summary>
    /// スローモーション倍率。
    /// </summary>
    internal float SlowScale => slowScale;

    /// <summary>
    /// スローモーション持続時間。
    /// </summary>
    internal float SlowDuration => slowDuration;

    /// <summary>
    /// 白フラッシュの持続時間。
    /// </summary>
    internal float WhiteFlashDuration => whiteFlashDuration;

    #endregion

}