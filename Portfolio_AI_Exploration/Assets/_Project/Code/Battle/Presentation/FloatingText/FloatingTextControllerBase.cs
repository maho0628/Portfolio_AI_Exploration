using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 浮遊テキスト演出の共通処理を提供する基底クラス。
/// テキスト表示、アニメーション再生、オブジェクトプールへの返却を管理する。
/// </summary>
/// <typeparam name="T">
/// オブジェクトプールで管理する自身の型。
/// </typeparam>
public abstract class FloatingTextControllerBase<T>
    : MonoBehaviour, IPoolable<T> where T : MonoBehaviour
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    #region Serialized Fields

    /// <summary>
    /// 表示するテキストUI。
    /// </summary>
    [Header("UI")]
    [SerializeField, Tooltip("表示する浮遊テキストUI")]
    private TextMeshProUGUI floatingTextUI;

    /// <summary>
    /// フェード演出に使用するCanvasGroup。
    /// </summary>
    [SerializeField, Tooltip("浮遊テキストの透明度を制御するCanvasGroup")]
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 浮遊テキスト演出の設定データ。
    /// </summary>
    [Header("Settings")]
    [SerializeField, Tooltip("浮遊テキストの移動量や表示時間などの設定")]
    private FloatingTextSettingsSO floatingTextSettings;

    #endregion

    // ==================================================
    // Runtime State
    // ==================================================

    #region Runtime State

    /// <summary>
    /// 自身を管理するオブジェクトプール。
    /// </summary>
    private ObjectPool<T> pool;

    /// <summary>
    /// 現在再生中のアニメーションシーケンス。
    /// </summary>
    private Sequence activeSequence;

    #endregion


    // ==================================================
    // Properties
    // ==================================================

    #region Properties

    /// <summary>
    /// 派生クラスから参照するテキストUI。
    /// </summary>
    protected TextMeshProUGUI FloatingTextUI => floatingTextUI;

    #endregion


    // ==================================================
    // Pool Lifecycle
    // ==================================================

    #region Pool Lifecycle

    /// <summary>
    /// オブジェクト生成時にプールを登録する。
    /// </summary>
    /// <param name="pool">
    /// 自身を管理するオブジェクトプール。
    /// </param>
    public void OnCreated(ObjectPool<T> pool)
    {
        this.pool = pool;
    }

    /// <summary>
    /// オブジェクトをプールへ返却する。
    /// </summary>
    public void ReturnToPool()
    {
        activeSequence?.Kill();

        pool.Return(this as T);
    }

    /// <summary>
    /// オブジェクト無効化時に再生中の演出を停止する。
    /// </summary>
    void OnDisable()
    {
        activeSequence?.Kill();
    }

    #endregion


    // ==================================================
    // Animation
    // ==================================================

    #region Animation

    /// <summary>
    /// 浮遊テキストの移動・フェード演出を再生する。
    /// </summary>
    protected void PlayAnimation()
    {
        activeSequence?.Kill();

        canvasGroup.alpha = 1f;

        Vector2 offset = new Vector2(
            Random.Range(
                floatingTextSettings.RandomOffsetMin.x,
                floatingTextSettings.RandomOffsetMax.x
            ),
            Random.Range(
                floatingTextSettings.RandomOffsetMin.y,
                floatingTextSettings.RandomOffsetMax.y
            )
        );

        transform.localPosition = offset;

        activeSequence = DOTween.Sequence();

        activeSequence.Join(
            transform.DOLocalMoveY(
                offset.y + floatingTextSettings.MoveY,
                floatingTextSettings.Duration
            )
        );

        activeSequence.Join(
            canvasGroup.DOFade(
                0f,
                floatingTextSettings.Duration
            )
        );

        activeSequence.OnComplete(ReturnToPool);
    }

    #endregion

}
