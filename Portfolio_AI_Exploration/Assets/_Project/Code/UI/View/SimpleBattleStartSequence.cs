using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[CreateAssetMenu(menuName = "Battle/StartSequence/Simple")]
public class SimpleBattleStartSequence : BattleStartSequence
{
    [Header("Text")]
    [SerializeField] private string message = "BATTLE START";

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private float waitDuration = 0.5f;
    [SerializeField] private float moveY = 200f;
    [SerializeField] private float startScale = 0.5f;

    public override async UniTask Play(GameObject context)
    {
        var presenter = Object.FindAnyObjectByType<BattleStartPresenter>();

        if (presenter != null)
        {
            await presenter.PlayAsync(
                message,
                fadeDuration,
                scaleDuration,
                waitDuration,
                moveY,
                startScale
            );
        }

        var manager =FindAnyObjectByType<BattleManager>();

        if (manager != null)
        {
            manager.StartBattle();
        }
    }
}