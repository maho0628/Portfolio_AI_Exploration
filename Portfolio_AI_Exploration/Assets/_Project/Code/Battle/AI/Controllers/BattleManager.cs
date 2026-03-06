using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField] private BattleAI player;
    [SerializeField] private BattleAI enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.SetTarget(enemy);
        enemy.SetTarget(player);
    }

   
}
