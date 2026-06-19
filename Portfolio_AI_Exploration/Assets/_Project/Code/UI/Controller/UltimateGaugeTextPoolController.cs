using UnityEngine;

/// <summary>
/// TP獲得量の表示とアニメーション再生を行うコントローラー。
/// FloatingTextControllerBase を継承し、TP表示用のテキストを制御する。
/// </summary>
public class UltimateGaugeTextPoolController
    : FloatingTextControllerBase<UltimateGaugeTextPoolController>
{
    /// <summary>
    /// TP獲得量を表示し、アニメーションを再生する。
    /// </summary>
    /// <param name="tp">
    /// 表示するTP獲得量。
    /// </param>
    internal void PlayTP(int tp)
    {
        gameObject.SetActive(true);

        FloatingTextUI.text = tp.ToString();

        PlayAnimation();
    }
}