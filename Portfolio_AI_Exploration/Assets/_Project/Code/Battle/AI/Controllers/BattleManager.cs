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