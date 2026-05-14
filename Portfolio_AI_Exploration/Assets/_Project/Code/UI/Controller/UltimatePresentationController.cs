using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
public class UltimatePresentationController
    : MonoBehaviour
{
    [SerializeField]
    private Image darkPanel;

    [SerializeField]
    private Image whiteFlash;

    [SerializeField]
    private UltimatePresentationSettingsSO settings;

    public async UniTask Play()
    {
        darkPanel.gameObject.SetActive(true);

        Color dark = darkPanel.color;
        dark.a = 0f;
        darkPanel.color = dark;

        await darkPanel
            .DOFade(
                0.7f,
                settings.darkFadeDuration
            )
            .ToUniTask();

        Time.timeScale = settings.slowScale;

        await UniTask.Delay(
            System.TimeSpan.FromSeconds(
                settings.slowDuration
            ),
            ignoreTimeScale: true
        );

        Time.timeScale = 1f;

        whiteFlash.gameObject.SetActive(true);

        Color flash = whiteFlash.color;
        flash.a = 1f;
        whiteFlash.color = flash;

        await whiteFlash
            .DOFade(
                0f,
                settings.whiteFlashDuration
            )
            .ToUniTask();

        darkPanel.gameObject.SetActive(false);
        whiteFlash.gameObject.SetActive(false);
    }
}