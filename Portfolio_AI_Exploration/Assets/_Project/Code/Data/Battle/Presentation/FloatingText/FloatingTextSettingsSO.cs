using UnityEngine;

/// <summary>
/// 浮遊テキスト演出の設定データを保持するスクリプタブルオブジェクト。
/// テキストの移動量、表示時間、生成位置のランダムオフセットを設定する。
/// </summary>
[CreateAssetMenu(
menuName = "Battle/UI/FloatingTextSettingsSO"
)]
public class FloatingTextSettingsSO : ScriptableObject
{

    /// <summary>
    /// 表示開始位置から上方向へ移動する距離。
    /// </summary>
    [Header("Move")]
    [SerializeField, Min(0f), Tooltip("表示開始位置から上方向へ移動する距離")]
    private float moveY = 80f;

    /// <summary>
    /// 浮遊テキスト演出の再生時間（秒）。
    /// </summary>
    [SerializeField, Min(0.01f), Tooltip("浮遊テキスト演出の再生時間（秒）")]
    private float duration = 0.5f;

    /// <summary>
    /// 表示位置のランダムオフセット最小値。
    /// </summary>
    [Header("Spawn Offset")]
    [SerializeField, Tooltip("表示位置のランダムオフセット最小値")]
    private Vector2 randomOffsetMin;

    /// <summary>
    /// 表示位置のランダムオフセット最大値。
    /// </summary>
    [SerializeField, Tooltip("表示位置のランダムオフセット最大値")]
    private Vector2 randomOffsetMax;


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

    /// <summary>
    /// 表示開始位置から上方向へ移動する距離。
    /// </summary>
    internal float MoveY => moveY;

    /// <summary>
    /// 浮遊テキスト演出の再生時間（秒）。
    /// </summary>
    internal float Duration => duration;

    /// <summary>
    /// 表示位置のランダムオフセット最小値。
    /// </summary>
    internal Vector2 RandomOffsetMin => randomOffsetMin;

    /// <summary>
    /// 表示位置のランダムオフセット最大値。
    /// </summary>
    internal Vector2 RandomOffsetMax => randomOffsetMax;

    #endregion
}
