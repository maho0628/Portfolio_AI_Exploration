using UnityEngine;

using Cysharp.Threading.Tasks;

public abstract class BattleStartSequence : ScriptableObject
{
    public abstract UniTask Play(GameObject context);
}