
using UnityEngine;

/// <summary>
/// ダメージ数値の表示とアニメーション再生を行うコントローラー。
/// FloatingTextControllerBase を継承し、ダメージ表示用のテキストを制御する。
/// </summary>
public class DamageTextController
    : FloatingTextControllerBase<DamageTextController>
{
    /// <summary>
    /// ダメージ数値を表示し、アニメーションを再生する。
    /// </summary>
    /// <param name="damage">
    /// 表示するダメージ量。
    /// </param>
    internal void PlayDamage(int damage)
    {
        gameObject.SetActive(true);

        FloatingTextUI.text = damage.ToString();

        PlayAnimation();
    }
}