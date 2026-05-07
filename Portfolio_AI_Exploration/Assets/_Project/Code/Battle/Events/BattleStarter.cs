using UnityEngine;

using Cysharp.Threading.Tasks;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private BattleStartSequence sequence;

    private async void Start()
    {
        if (sequence != null)
        {
            await sequence.Play(gameObject);
        }
    }
}