using UnityEngine;

public interface IPoolable<T> where T : MonoBehaviour
{
    void OnCreated(UIObjectPool<T> pool);

}

