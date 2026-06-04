using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageTextController
    : FloatingTextControllerBase<DamageTextController>
{
    public void Play(int damage)
    {
        gameObject.SetActive(true);

        textUI.text = damage.ToString();

        PlayAnimation();
    }
}