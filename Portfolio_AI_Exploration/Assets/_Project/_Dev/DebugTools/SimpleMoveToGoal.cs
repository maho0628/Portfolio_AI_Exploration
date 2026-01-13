using UnityEngine;
using UnityEngine.AI;


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

    private NavMeshAgent agent;

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
        Debug.Log(
    $"remaining: {agent.remainingDistance}, " +
    $"stopping: {agent.stoppingDistance}, " +
    $"hasPath: {agent.hasPath}, " +
    $"isStopped: {agent.isStopped}"
);
        // NavMeshAgent が目的地に「到達したか」を判定するための定番チェック
        // Update() や 移動中の処理内で使う想定

        // pathPending:
        // NavMeshAgent がまだ経路計算中かどうか
        // true  = まだルートを計算している途中
        // false = ルートが確定して、実際に移動している or 到達済み
        if (!agent.pathPending &&

            // remainingDistance:
            // NavMesh上で「目的地まで残っている距離」
            // stoppingDistance:
            // ここまで近づいたら「到達した」とみなす距離
            agent.remainingDistance <= agent.stoppingDistance)
        {
            // ここに来た = 「経路計算が終わっていて、目的地に十分近い」

            // 今回は簡易的に移動を停止しているが、
            // 本番では State を Idle に変えたり、
            // 到達イベントを発火させることが多い
            agent.isStopped = true;
        }
    }

    /*
なぜ remainingDistance だけで判定しないのか？

NavMeshAgent は、
経路計算中（pathPending == true）の間、
remainingDistance が 0 になることがある。

そのため
「remainingDistance <= stoppingDistance」だけを見ると
まだ移動が始まっていないのに
"到達した" と誤判定してしまう可能性がある。

→ 必ず pathPending == false とセットで使う。
*/

    /*
この到達判定は、
・移動 → 待機（Idle）
・移動 → 調査（Investigate）
・移動 → 次の行動
といった「状態遷移のトリガー」として使う。

isStopped = true 自体は仮の処理で、
最終的には StateMachine や BehaviorTree 側で制御する。
*/

}

