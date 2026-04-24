using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

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
    [SerializeField] private ExplorationRoute route;
    private int currentIndex = 0;
    [Header("Planner Adjustable")]
    [Tooltip("到達判定の余裕距離。プランナーが微調整可能")]
    [SerializeField] private float arrivalThreshold = 0.05f;

    private NavMeshAgent agent;
    private bool hasArrivedHandled = false;

    private bool hasStartedMoving = false;
    private enum MoveState
    {
        Moving,
        Arrived,
        Blocked
    }

    private MoveState state;

    // 直前の状態を保持して状態変化時のみログ出力
    private MoveState previousState;

    private async void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentIndex = ExplorationData.currentRouteIndex;

        agent.Warp(ExplorationData.playerPosition);
        agent.ResetPath(); // ← これ超重要

        await UniTask.Yield(); // ← これ

        if (CurrentTarget != null)
        {
            hasArrivedHandled = false;
            agent.isStopped = false;
            hasStartedMoving = false; // ←追加

            agent.SetDestination(CurrentTarget.position);

            Debug.Log(ExplorationData.currentRouteIndex);

            Debug.Log($"Next Target : {CurrentTarget.name}");
            Debug.Log($"Player Pos  : {transform.position}");
            Debug.Log($"Target Pos  : {CurrentTarget.position}");
        }
    }

    void Update()
    {
        if (CurrentTarget == null) return;

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
            //Debug.Log(
            //    $"[Debug] State changed: {previousState} -> {state}, " +
            //    $"RemainingDistance: {agent.remainingDistance}, " +
            //    $"StoppingDistance: {agent.stoppingDistance}, " +
            //    $"HasPath: {agent.hasPath}, " +
            //    $"IsStopped: {agent.isStopped}"
            //);
            previousState = state;
        }
    }

    //private void MoveNext()
    //{
    //    currentIndex++;
    //    ExplorationData.currentRouteIndex = currentIndex; // ←保存

    //    if (currentIndex >= route.targets.Count)
    //    {
    //        return;
    //    }

    //    agent.isStopped = false;
    //    agent.SetDestination(CurrentTarget.position);
    //    hasArrivedHandled = false;
    //}
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

        // まだ移動開始してないなら判定しない
        if (!hasStartedMoving)
        {
            if (!agent.pathPending && agent.hasPath)
            {
                hasStartedMoving = true;
            }
            return;
        }
        // ---------------------------
        // 経路計算中は Moving とする
        // ---------------------------
        if (agent.pathPending)
        {
            state = MoveState.Moving;
            return;
        }


        if (!agent.pathPending && agent.remainingDistance == Mathf.Infinity)
        {
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
            HandleArrival();

            return;

        }

        // ---------------------------
        // 移動中判定（Moving）
        // ---------------------------
        state = MoveState.Moving;
    }
    private Transform CurrentTarget
    {
        get
        {
            if (route == null || route.targets == null || currentIndex >= route.targets.Count)
                return null;

            return route.targets[currentIndex];
        }

    }
    private void HandleArrival()
    {
        if (SceneTransitionManager.Instance.IsTransitioning) return;
        if (hasArrivedHandled) return;

        hasArrivedHandled = true;

        Debug.Log("State: Arrived - 到達成功");

        if (CurrentTarget != null)
        {
            var interactTarget = CurrentTarget.GetComponent<InteractTarget>();
            if (interactTarget != null)
            {
                HandleArrive(interactTarget.role);
            }
        }

        agent.isStopped = true;
    }
    private void HandleArrive(InteractRole role)
    {
        switch (role)
        {
            case InteractRole.Enemy:
                Debug.Log("Enemy reached → BattleScene");
                ExplorationData.playerPosition = transform.position;
                if (SceneTransitionManager.Instance.IsTransitioning) return;
                SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);
                break;

            case InteractRole.Goal:
                BattleResultData.resultType = ResultType.Goal;
                if (SceneTransitionManager.Instance.IsTransitioning) return;
                Debug.Log("Goal reached → ResultScene");
                SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);
                break;
        }
    }

}
