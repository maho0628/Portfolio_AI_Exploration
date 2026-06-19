using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleStartPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;

    public async UniTask PlayAsync(
      string message,
      float fadeDuration,
      float scaleDuration,
      float waitDuration,
      float moveY,
      float startScale)
    {
        // ===== 初期化 =====
        textTransform.gameObject.SetActive(true);

        text.text = message;

        canvasGroup.alpha = 0;

        textTransform.localScale = Vector3.one * startScale;

        textTransform.anchoredPosition = Vector2.zero;

        // ===== アニメーション =====
        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, fadeDuration));

        seq.Join(
            textTransform.DOScale(1f, scaleDuration)
        );

        seq.AppendInterval(waitDuration);

        seq.Append(
            canvasGroup.DOFade(0f, fadeDuration)
        );

        seq.Join(
            textTransform.DOAnchorPosY(moveY, fadeDuration)
        );

        await seq.AsyncWaitForCompletion();

        // ===== 終了処理 =====
        textTransform.gameObject.SetActive(false);
    }
}