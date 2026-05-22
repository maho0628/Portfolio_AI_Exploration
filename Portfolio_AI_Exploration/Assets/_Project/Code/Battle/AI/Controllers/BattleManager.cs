using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleAI player;
    [SerializeField] private BattleAI enemy;
    private bool battleEnded;
    [SerializeField] private HPUIController hpUI;
    private GameState currentState = GameState.Battle;
    private bool isBattleStarted = false;
    [SerializeField] private UBGaugeUIController playerUBGauge;

    void Start()
    {
        AudioManager.Instance.FadeInBGM();

        AudioManager.Instance.PlayBGMIfNotPlaying(BGMName.Battle);
        battleEnded = false;
        hpUI.Init(player, enemy);
        BattleResultData.Reset();
        player.SetTarget(enemy);
        enemy.SetTarget(player);
        playerUBGauge.Init(player); 
    }

    void Update()
    {
        if (!isBattleStarted) return;

        if (currentState == GameState.Battle)
        {
            CheckBattleEnd();
        }

      
    }

    public void StartBattle()
    {
        isBattleStarted = true;
        player.ActivateAI();
        enemy.ActivateAI();
    }

    void CheckBattleEnd()
    {
        if (battleEnded)
        {
            return;

        }
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
            Debug.Log("❌ すでに終了済み");
            return;
        }

        battleEnded = true;

        BattleResultData.resultType = resultType;

        SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);

    }

}