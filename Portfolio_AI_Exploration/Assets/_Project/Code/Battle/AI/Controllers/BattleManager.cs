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

        if (enemy.Blackboard.IsDead)
        {
            battleEnded = true;
            EndBattle(true);
        }
        else if (player.Blackboard.IsDead)
        {
            battleEnded = true;
            EndBattle(false);
        }
    }

    void EndBattle(bool playerWin)
    {
        if (battleEnded == false)
        {
            battleEnded = true;

        }

        BattleResultData.playerWin = playerWin;

        Debug.Log("Battle Finished");
        Debug.Log("Player Win: " + playerWin);

        SceneManager.LoadScene("ResultScene");
    }
}