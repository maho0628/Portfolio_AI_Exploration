using UnityEngine;

public interface IPoolable<T>where T : MonoBehaviour
{
    void OnCreated(ObjectPool<T> pool);

    void ReturnToPool();
}

