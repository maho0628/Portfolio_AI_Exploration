using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleAI player;
    [SerializeField] private BattleAI enemy;

    private bool battleEnded;

    private GameState currentState = GameState.Battle;

    void Start()
    {
        battleEnded = false;
        Debug.Log($"[Battle Start] Player HP: {player.Blackboard.CurrentHP}");
        Debug.Log($"[Battle Start] Enemy  HP: {enemy.Blackboard.CurrentHP}");
    
    player.Initialize();
        enemy.Initialize();
        BattleResultData.Reset();
        Debug.Log(BattleResultData.interventionCount.ToString());
        player.SetTarget(enemy);
        enemy.SetTarget(player);
  
    }

    void Update()
    {
        if (currentState == GameState.Battle)
        {
            CheckBattleEnd();
        }
    }

    void CheckBattleEnd()
    {
        if (battleEnded)
        {
            return;

        }
        Debug.Log($"[Check] PlayerDead: {player.Blackboard.IsDead} / EnemyDead: {enemy.Blackboard.IsDead}");
        if (enemy.Blackboard.IsDead)
        {
            EndBattle(ResultType.Victory);
        }
        else if (player.Blackboard.IsDead)
        {
            EndBattle(ResultType.Defeat);
        }
    }

    void EndBattle(ResultType resultType)
    {
        if (battleEnded)
        {
            return;
        }

        battleEnded = true;

        BattleResultData.resultType = resultType;

        Debug.Log($"Battle Finished : {resultType}");

        SceneManager.LoadScene("ResultScene");
    }
}