using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/*
 * SimpleMoveToGoal
 *
 * 探索パートにおける「NavMesh を使った最低限の移動挙動」を確認するための検証用クラス。
 *
 * 目的：
 * - NavMeshAgent に目的地を与えると、正しく移動するか
 * - remainingDistance / stoppingDistance を使った到達判定が機能するか
 *
 * 本クラスは以下を目的としている：
 * - 探索AIの最終実装ではなく、挙動理解・確認用
 * - 後に StateMachine / BehaviorTree に組み込む前提のプロトタイプ
 *
 * 注意：
 * - このクラス自体は本番で直接使用しない想定
 * - 将来的には「移動状態（Walk）」や「到達イベント」のロジックに分解される
 */

public class SimpleMoveToGoal : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Planner Adjustable")]
    [Tooltip("到達判定の余裕距離。プランナーが微調整可能")]
    [SerializeField] private float arrivalThreshold = 0.05f;

    private NavMeshAgent agent;

    private enum MoveState
    {
        Moving,
        Arrived,
        Blocked
    }

    private MoveState state;

    // 直前の状態を保持して状態変化時のみログ出力
    private MoveState previousState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;

        Debug.Log($"isOnNavMesh: {agent.isOnNavMesh}");
        Debug.Log($"hasPath: {agent.hasPath}");
        Debug.Log($"pathStatus: {agent.pathStatus}");

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        UpdateState();

        // ---------------------------
        // 状態に応じた処理
        // ---------------------------
        switch (state)
        {
            case MoveState.Moving:
                // 移動中処理
                Debug.Log("State: Moving - 移動中");
                break;

            case MoveState.Arrived:
                // 到達成功時の処理
                Debug.Log("State: Arrived - 到達成功");
                agent.isStopped = true; // 仮処理、本番では StateMachine で制御
                break;

            case MoveState.Blocked:
                // 到達不能時の処理
                Debug.Log("State: Blocked - 到達不能");
                agent.isStopped = true; // 仮処理、本番では StateMachine で制御
                break;
        }

        // ---------------------------
        // デバッグ用ログ（状態変化時のみ出力）
        // ---------------------------
        if (state != previousState)
        {
            Debug.Log(
                $"[Debug] State changed: {previousState} -> {state}, " +
                $"RemainingDistance: {agent.remainingDistance}, " +
                $"StoppingDistance: {agent.stoppingDistance}, " +
                $"HasPath: {agent.hasPath}, " +
                $"IsStopped: {agent.isStopped}"
            );
            previousState = state;
        }
    }

    /*
     * UpdateState
     *
     * NavMeshAgent の状態を MoveState に変換
     *
     * pathPending:
     * - NavMeshAgent がまだ経路計算中かどうか
     * - true  = 計算中、false = 経路確定済み
     *
     * remainingDistance / stoppingDistance:
     * - NavMesh上で「目的地まで残っている距離」をチェック
     * - remainingDistance が 0 になった場合は到達済み
     *
     * 注意：
     * - remainingDistance だけで判定しない
     *   → 経路計算中は 0 になる場合がある
     * - この到達判定は、移動→Idle、移動→Investigate、移動→次の行動
     *   といった「状態遷移のトリガー」として使用
     * - isStopped = true は仮処理、本番では StateMachine / BehaviorTree で制御
     */
    private void UpdateState()
    {
        // ---------------------------
        // 経路計算中は Moving とする
        // ---------------------------
        if (agent.pathPending)
        {
            state = MoveState.Moving;
            return;
        }

        // ---------------------------
        // 到達不能判定（Blocked）
        // 経路計算後に hasPath が false の場合のみ
        // ---------------------------
        if (!agent.hasPath)
        {
            state = MoveState.Blocked;
            return;
        }

        // ---------------------------
        // 到達成功判定（Arrived）
        // remainingDistance が stoppingDistance + threshold 以下
        // ---------------------------
        if (agent.remainingDistance <= agent.stoppingDistance + arrivalThreshold)
        {
            state = MoveState.Arrived;
            // 到達した対象が敵なら戦闘シーンへ
            if (target != null)
            {
                var interactTarget = target.GetComponent<InteractTarget>();
                if (interactTarget != null)
                {
                    HandleArrive(interactTarget.role);
                }
            }

            agent.isStopped = true; // 仮処理

            return;
        }

        // ---------------------------
        // 移動中判定（Moving）
        // ---------------------------
        state = MoveState.Moving;
    }
    private void HandleArrive(InteractRole role)
    {
        switch (role)
        {
            case InteractRole.Enemy:
                Debug.Log("Enemy reached → BattleScene");
                SceneManager.LoadScene("BattleScene");
                break;

            case InteractRole.Goal:
                Debug.Log("Goal reached → ResultScene");
                SceneManager.LoadScene("ResultScene");
                break;
        }
    }
}
