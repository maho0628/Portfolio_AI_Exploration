using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// 必殺技演出の再生を制御するコントローラー。
/// カットイン・スローモーション・フラッシュなどの演出を統括し、
/// BattleAI の必殺技フローと連携して再生される。
///
/// 複数キャラ同時再生の制御やTimeScale管理など、
/// ゲーム全体に影響する演出制御も担当する。
/// </summary>
public class UltimatePresentationController : MonoBehaviour
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    #region Serialized Fields

    /// <summary>
    /// 暗転演出に使用するUIパネル。
    /// 画面を徐々に暗くするフェード演出に使用される。
    /// </summary>
    [SerializeField, Tooltip("暗転用パネル")]
    private Image darkPanel;

    /// <summary>
    /// 白フラッシュ演出用UI。
    /// 必殺技発動時の強い視覚的インパクトを表現する。
    /// </summary>
    [SerializeField, Tooltip("白フラッシュ演出")]
    private Image whiteFlash;

    /// <summary>
    /// 必殺技演出に関する設定データ。
    /// フェード時間やスロー倍率などを外部から調整可能にする。
    /// </summary>
    [SerializeField, Tooltip("必殺技演出設定データ")]
    private UltimatePresentationSettingsSO settings;

    /// <summary>
    /// 必殺技発動時のアニメーション制御用Animator。
    /// キャラのカットイン演出などに使用される。
    /// </summary>
    [SerializeField, Tooltip("必殺技演出用アニメーター")]
    private Animator animator;

    #endregion


    // ==================================================
    // Runtime State
    // ==================================================

    #region Runtime State

    /// <summary>
    /// 現在必殺技演出が再生中かどうかを示すフラグ。
    /// 複数キャラの同時発動を防ぐためのグローバル制御。
    /// </summary>
    private static bool globalPlaying;

    /// <summary>
    /// スローモーション処理の参照カウント。
    /// 複数演出が重なってもTimeScaleの競合を防ぐために使用する。
    /// </summary>
    private static int slowRefCount;

    /// <summary>
    /// 透明状態を表すアルファ値（0）
    /// </summary>
    private const float TransparentAlpha = 0f;

    /// <summary>
    /// 完全表示状態を表すアルファ値（1）
    /// </summary>
    private const float FullAlpha = 1f;

    /// <summary>
    /// 通常の時間スケール（Time.timeScale = 1）
    /// </summary>
    private const float NormalTimeScale = 1f;

    #endregion


    // ==================================================
    // Presentation Flow
    // ==================================================

    #region Presentation Flow

    /// <summary>
    /// 必殺技演出を再生するメイン処理。
    /// 暗転 → スロー演出 → アニメーション → フラッシュの順に実行される。
    ///
    /// 再生中は globalPlaying により多重実行が防止される。
    /// </summary>
    /// <returns>演出完了まで待機する非同期タスク</returns>
    internal async UniTask Play()
    {
        if (globalPlaying)
        {
            return;
        }

        globalPlaying = true;

        // ------------------------------
        // Dark Panel Fade In
        // ------------------------------
        darkPanel.gameObject.SetActive(true);

        Color dark = darkPanel.color;
        dark.a = TransparentAlpha;
        darkPanel.color = dark;

        await darkPanel
            .DOFade(settings.DarkPanelAlpha, settings.DarkFadeDuration)
            .ToUniTask();

        // ------------------------------
        // Animator Trigger
        // ------------------------------
        animator?.SetTrigger("Ultimate");

        // ------------------------------
        // Slow Motion Start
        // ------------------------------
        if (slowRefCount == 0)
        {
            Time.timeScale = settings.SlowScale;
        }

        slowRefCount++;

        await UniTask.Delay(
            System.TimeSpan.FromSeconds(settings.SlowDuration),
            ignoreTimeScale: true
        );

        slowRefCount--;

        if (slowRefCount <= 0)
        {
            Time.timeScale = NormalTimeScale;
            slowRefCount = 0;
        }

        // ------------------------------
        // White Flash Effect
        // ------------------------------
        whiteFlash.gameObject.SetActive(true);

        Color flash = whiteFlash.color;
        flash.a = FullAlpha;
        whiteFlash.color = flash;

        await whiteFlash
            .DOFade(TransparentAlpha, settings.WhiteFlashDuration)
            .ToUniTask();

        // ------------------------------
        // Cleanup
        // ------------------------------
        darkPanel.gameObject.SetActive(false);
        whiteFlash.gameObject.SetActive(false);

        globalPlaying = false;
    }

    #endregion
}