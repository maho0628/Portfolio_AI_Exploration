using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 必殺技ゲージ（TP）の表示と演出を制御するUIコントローラー。
/// BattleAI の TP変化を監視し、ゲージの更新や発光演出を行う。
/// </summary>
public class UltimateImpactGaugeUIController : MonoBehaviour
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    #region Serialized Fields

    /// <summary>
    /// TPゲージの表示を行う Image コンポーネント。
    /// </summary>
    [SerializeField, Tooltip("TPゲージの表示を行う Image コンポーネント")]
    private Image gaugeFill;

    /// <summary>
    /// TPゲージ演出の設定データ。
    /// </summary>
    [SerializeField, Tooltip("TPゲージ演出の設定データ")]
    private UltimateImpactGaugeSettingsSO gaugeSettingSO;

    /// <summary>
    /// ゲージ発光演出を表示する Image コンポーネント。
    /// </summary>
    [SerializeField, Tooltip("ゲージ発光演出を表示する Image コンポーネント")]
    private Image glowImage;

    #endregion


    // ==================================================
    // Runtime Data
    // ==================================================

    #region Runtime Data

    /// <summary>
    /// このゲージに対応する BattleAI。
    /// </summary>
    private BattleAI owner;

    /// <summary>
    /// 現在再生中の発光アニメーション。
    /// </summary>
    private Tween glowTween;

    #endregion


    // ==================================================
    // UI Lifecycle
    // ==================================================

    #region  UI Lifecycle

    /// <summary>
    /// TPゲージの初期化を行う。
    /// TP変更イベントを購読し、現在の値を表示する。
    /// </summary>
    /// <param name="ai">対応する BattleAI。</param>
    internal void Init(BattleAI ai)
    {
        owner = ai;

        owner.BB.OnTPChanged += UpdateGauge;

        // 初期表示
        UpdateGauge(
            owner.BB.CurrentTP,
            owner.BB.MaxTP
        );
    }

    /// <summary>
    /// イベント購読を解除する。
    /// </summary>
    private void OnDestroy()
    {
        if (owner != null)
        {
            owner.BB.OnTPChanged -= UpdateGauge;
        }
    }

    #endregion


    // ==================================================
    // Gauge Presentation
    // ==================================================

    #region Gauge Presentation

    /// <summary>
    /// TPゲージの表示を更新する。
    /// TP量に応じて発光演出も切り替える。
    /// </summary>
    /// <param name="current">現在のTP。</param>
    /// <param name="max">TPの最大値。</param>
    private void UpdateGauge(int current, int max)
    {
        float normalized = (float)current / max;

        if (normalized >= 1f)
        {
            PlayGlow(gaugeSettingSO.MaxGlow);
        }
        else if (normalized >= gaugeSettingSO.WarningThreshold)
        {
            PlayGlow(gaugeSettingSO.WarningGlow);
        }
        else
        {
            StopGlow();
        }

        gaugeFill.DOFillAmount(normalized, gaugeSettingSO.FillDuration);
    }

    /// <summary>
    /// 発光アニメーションを開始する。
    /// </summary>
    /// <param name="settings">発光演出の設定。
    /// </param>
    private void PlayGlow(GlowSettings settings)
    {
        if (glowTween != null)
        {
            glowTween.Kill();
        }

        glowImage.gameObject.SetActive(true);

        glowTween = glowImage
            .DOFade(settings.Alpha, settings.Duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 発光アニメーションを停止する。
    /// </summary>
    private void StopGlow()
    {
        if (glowTween != null)
        {
            glowTween.Kill();
        }

        Color color = glowImage.color;
        color.a = 0f;
        glowImage.color = color;
    }

    #endregion
}