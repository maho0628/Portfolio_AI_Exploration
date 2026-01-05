using System.Collections.Generic;
using UnityEngine;

public class UIObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField] private int preloadCount = 10;

    private readonly List<T> pool = new List<T>();

    private void Awake()
    {
        for (int i = 0; i < preloadCount; i++)
        {
            T instance = Instantiate(prefab, transform);
            instance.gameObject.SetActive(false);
            pool.Add(instance);
        }
    }

    public T Get()
    {
        // 非アクティブなインスタンスを探す
        foreach (var item in pool)
        {
            if (!item.gameObject.activeSelf)
            {
                Prepare(item);
                return item;
            }
        }

        // 空きがないなら新しく作る
        T newItem = Instantiate(prefab, transform);
        pool.Add(newItem);
        Prepare(newItem);
        return newItem;
    }

    private void Prepare(T item)
    {
        item.gameObject.SetActive(true);

        if (item is IPoolable<T> poolAble)
        {
            poolAble.OnCreated(this);
        }

        //if (item is IUIEffectPoolable<T> uiEffectPoolAble)
        //{
        //    uiEffectPoolAble.OnCreated(this);
        //}

    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        DebugManager.Log($"Returning to pool: {typeof(T).Name}, Current count: {pool.Count}");
    }
}
