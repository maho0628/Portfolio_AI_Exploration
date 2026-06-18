using UnityEngine;

/// <summary>
/// オブジェクトプールで管理されるコンポーネントの共通インターフェース。
/// プール生成時の初期化処理と、プールへの返却処理を定義する。
/// </summary>
/// <typeparam name="T">
/// プール対象となる MonoBehaviour の型。
/// </typeparam>
public interface IPoolable<T> where T : MonoBehaviour
{
    /// <summary>
    /// オブジェクト生成時に呼び出される。
    /// 所属するオブジェクトプールへの参照を保持する。
    /// </summary>
    /// <param name="pool">
    /// このオブジェクトを管理するプール。
    /// </param>
    void OnCreated(ObjectPool<T> pool);

    /// <summary>
    /// オブジェクトをプールへ返却する。
    /// 必要に応じて再利用のための後処理を行う。
    /// </summary>
    void ReturnToPool();

}
