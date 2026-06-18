using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour を対象とした汎用オブジェクトプール。
/// 非アクティブなインスタンスを再利用し、生成コストを削減する。
/// </summary>
/// <typeparam name="T">
/// プール対象となる MonoBehaviour の型。
/// </typeparam>
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    #region Serialized Fields

    /// <summary>
    /// プールで生成・再利用するプレハブ。
    /// </summary>
    [SerializeField, Tooltip("プールで生成・再利用するプレハブ")]
    private T prefab;

    /// <summary>
    /// 起動時に事前生成するインスタンス数。
    /// </summary>
    [SerializeField, Min(0), Tooltip("起動時に事前生成するインスタンス数")]
    private int preloadCount = 10;

    #endregion


    // ==================================================
    // Runtime Data
    // ==================================================

    #region Runtime Data

    /// <summary>
    /// プールで管理しているインスタンス一覧。
    /// </summary>
    private readonly List<T> pool = new();

    #endregion


    // ==================================================
    // Unity Lifecycle
    // ==================================================

    #region Unity Lifecycle

    private void Awake()
    {
        for (int i = 0; i < preloadCount; i++)
        {
            T instance = Instantiate( prefab,transform  );

            instance.gameObject.SetActive(false);

            pool.Add(instance);
        }
    }

    #endregion


    // ==================================================
    // Pool Operations
    // ==================================================

    #region Pool Operations

    /// <summary>
    /// プールから利用可能なインスタンスを取得する。
    /// 利用可能なオブジェクトが存在しない場合は新規生成する。
    /// </summary>
    /// <returns>
    /// 使用可能なインスタンス。
    /// </returns>
    internal T Get()
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeSelf)
            {
                Prepare(item);
                return item;
            }
        }

        T newItem = Instantiate(prefab,transform);

        pool.Add(newItem);

        Prepare(newItem);

        return newItem;
    }

    /// <summary>
    /// プールから取得したオブジェクトを使用可能な状態にする。
    /// </summary>
    /// <param name="item">
    /// 初期化対象のインスタンス。
    /// </param>
    private void Prepare(T item)
    {
        item.gameObject.SetActive(true);

        if (item is IPoolable<T> poolable)
        {
            poolable.OnCreated(this);
        }
    }

    /// <summary>
    /// オブジェクトをプールへ返却する。
    /// </summary>
    /// <param name="item">
    /// 返却対象のインスタンス。
    /// </param>
    internal void Return(T item)
    {
        item.gameObject.SetActive(false);
    }

    #endregion
}