using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

public class UBGaugeUIController : MonoBehaviour
{
    [SerializeField] private Image gaugeFill;

    [SerializeField] private UBGaugeSettingsSO settings;
    [SerializeField] private Image glowImage;

    private BattleAI owner;


    private Tween glowTween;

    public void Init(BattleAI ai)
    {
        owner = ai;

        owner.Blackboard.OnTPChanged += UpdateGauge;

        // 初期表示
        UpdateGauge(
            owner.Blackboard.CurrentTP,
            owner.Blackboard.MaxTP
        );
    }

    private void UpdateGauge(int current, int max)
    {
        float normalized = (float)current / max;

        if (normalized >= 1f)
        {
            PlayGlow(settings.maxGlow);
        }
        else if (normalized >= settings.warningThreshold)
        {
            PlayGlow(settings.warningGlow);
        }
        else
        {
            StopGlow();
        }
        gaugeFill.DOFillAmount(normalized,settings.fillDuration);
    }


    private void PlayGlow(GlowSettings settings)
    {
        if (glowTween != null)
        {
            glowTween.Kill();
        }

        glowImage.gameObject.SetActive(true);

        glowTween = glowImage
            .DOFade(settings.alpha, settings.duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

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

    private void OnDestroy()
    {
        if (owner != null)
        {
            owner.Blackboard.OnTPChanged -= UpdateGauge;
        }
    }
}