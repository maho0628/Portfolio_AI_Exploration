using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class UltimatePresentationController : MonoBehaviour
{
    [SerializeField] private Image darkPanel;
    [SerializeField] private Image whiteFlash;
    [SerializeField] private UltimatePresentationSettingsSO settings;
    [SerializeField] private Animator animator;

    private bool isPlaying;
    private static int slowRefCount;


    public async UniTask Play()
    {
        if (isPlaying) return;
        isPlaying = true;

        darkPanel.gameObject.SetActive(true);

        Color dark = darkPanel.color;
        dark.a = 0f;
        darkPanel.color = dark;

        await darkPanel
            .DOFade(0.7f, settings.darkFadeDuration)
            .ToUniTask();

        if (animator != null)
        {
            animator.SetTrigger("Ultimate");
        }
        if (slowRefCount == 0)
        {
            Time.timeScale = settings.slowScale;
        }

        slowRefCount++;

        await UniTask.Delay(
            System.TimeSpan.FromSeconds(settings.slowDuration),
            ignoreTimeScale: true
        );

        slowRefCount--;

        if (slowRefCount <= 0)
        {
            Time.timeScale = 1f;
            slowRefCount = 0;
        }

        whiteFlash.gameObject.SetActive(true);

        Color flash = whiteFlash.color;
        flash.a = 1f;
        whiteFlash.color = flash;

        await whiteFlash
            .DOFade(0f, settings.whiteFlashDuration)
            .ToUniTask();

        darkPanel.gameObject.SetActive(false);
        whiteFlash.gameObject.SetActive(false);

        isPlaying = false;
    }
}