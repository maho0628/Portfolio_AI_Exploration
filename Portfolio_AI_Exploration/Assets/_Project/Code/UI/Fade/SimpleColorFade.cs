using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SimpleColorFade : FadeControllerBase
{
    [SerializeField] private Image fadeImage;

    public override async UniTask FadeOutAsync(FadeSettings settings)
    {
        await FadeCoroutineAsync(settings.startAlpha, settings.endAlpha, settings.FadeSpeed);
    }

    public override async UniTask FadeInAsync(FadeSettings settings)
    {
        await FadeCoroutineAsync(settings.startAlpha, settings.endAlpha, settings.FadeSpeed);
    }

    private async UniTask FadeCoroutineAsync(float start, float end, float speed)
    {
        Debug.LogWarning($"fadeImage: {fadeImage}");
        Debug.LogWarning($"active: {fadeImage.gameObject.activeInHierarchy}");
        Color color = fadeImage.color;
        fadeImage.gameObject.SetActive(true);

        float t = 0f;

        while (t < 1f)
        {
            float delta = Time.unscaledDeltaTime;
            if (delta <= 0f) delta = 0.016f;

            t += delta * speed;
            color.a = Mathf.Lerp(start, end, t);
            fadeImage.color = color;

            await UniTask.Yield();
        }

        // 最終値保証
        color.a = end;
        fadeImage.color = color;

        if (Mathf.Approximately(end, 0f))
            fadeImage.gameObject.SetActive(false);
    }
}
