using UnityEngine;

using Cysharp.Threading.Tasks;
using DG.Tweening;

public static class TweenUtil
{
    public static UniTask Play(Tween tween)
    {
        if (tween == null)
            return UniTask.CompletedTask;

        return tween.AsyncWaitForCompletion().AsUniTask();
    }

    public static UniTask Play(Sequence seq)
    {
        if (seq == null)
            return UniTask.CompletedTask;

        return seq.AsyncWaitForCompletion().AsUniTask();
    }
}
