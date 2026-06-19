using UnityEngine;

/// <summary>
/// 発光演出の設定を保持するクラス。
/// </summary>
[System.Serializable]
public class GlowSettings
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    /// <summary>
    /// 発光時の透明度。
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    [Tooltip("発光時の透明度")]
    private float alpha = 0.5f;

    /// <summary>
    /// 発光アニメーションの継続時間（秒）。
    /// </summary>
    [SerializeField, Min(0f)]
    [Tooltip("発光アニメーションの継続時間（秒）")]
    private float duration = 0.5f;


    // ==================================================
    //Read Only Properties
    // ==================================================

    #region Read Only Properties

    /// <summary>
    /// 発光時の透明度。
    /// </summary>
    internal float Alpha => alpha;

    /// <summary>
    /// 発光アニメーションの継続時間（秒）。
    /// </summary>
    internal float Duration => duration;

    #endregion

}
