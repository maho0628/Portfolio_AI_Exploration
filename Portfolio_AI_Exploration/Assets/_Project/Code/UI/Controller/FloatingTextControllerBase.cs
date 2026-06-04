using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class FloatingTextControllerBase<T>
    : MonoBehaviour,
      IPoolable<T>
    where T : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    protected TextMeshProUGUI textUI;

    [SerializeField]
    protected CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField]
    protected FloatingTextSettingsSO settings;

    protected ObjectPool<T> pool;

    protected Sequence activeSequence;

    public void OnCreated(ObjectPool<T> pool)
    {
        this.pool = pool;
    }

    protected void PlayAnimation()
    {
        activeSequence?.Kill();

        canvasGroup.alpha = 1f;

        Vector2 offset = new Vector2(
            Random.Range(
                settings.randomOffsetMin.x,
                settings.randomOffsetMax.x
            ),
            Random.Range(
                settings.randomOffsetMin.y,
                settings.randomOffsetMax.y
            )
        );

        transform.localPosition = offset;

        activeSequence = DOTween.Sequence();

        activeSequence.Join(
            transform.DOLocalMoveY(
                offset.y + settings.moveY,
                settings.duration
            )
        );

        activeSequence.Join(
            canvasGroup.DOFade(
                0f,
                settings.duration
            )
        );

        activeSequence.OnComplete(ReturnToPool);
    }

    public virtual void ReturnToPool()
    {
        activeSequence?.Kill();

        pool.Return(this as T);
    }

    protected virtual void OnDisable()
    {
        activeSequence?.Kill();
    }
}
