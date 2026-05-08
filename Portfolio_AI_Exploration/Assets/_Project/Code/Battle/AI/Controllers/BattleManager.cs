using UnityEngine.SceneManagement;
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
        battleEnded = false;

        Debug.Log(player);
        Debug.Log(enemy);
        Debug.Log(hpUI);

        Debug.Log($"[Battle Start] Player HP: {player.Blackboard.CurrentHP}");
        Debug.Log($"[Battle Start] Enemy  HP: {enemy.Blackboard.CurrentHP}");
      
      
        hpUI.Init(player, enemy);


        BattleResultData.Reset();
        Debug.Log(BattleResultData.interventionCount.ToString());
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




        Debug.Log("✅ 初回EndBattle通過");
        battleEnded = true;

        BattleResultData.resultType = resultType;

        Debug.Log($"Battle Finished : {resultType}");


        Debug.Log("👉 遷移呼び出し前");

        Debug.LogWarning($"Instance: {SceneTransitionManager.Instance}");

        var manager = SceneTransitionManager.Instance;
        Debug.Log("② Instance取得");
        Debug.Log($"manager null? {manager == null}");


        Debug.LogWarning($"Instance: {SceneTransitionManager.Instance}");
        Debug.LogWarning($"Scene: {SceneTransitionManager.Instance.gameObject.scene.name}");


        SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);



        Debug.Log("👉 遷移呼び出し後");
    }






}