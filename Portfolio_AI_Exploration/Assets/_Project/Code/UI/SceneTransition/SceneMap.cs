using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneMap", menuName = "Scene/SceneMap")]
public class SceneMap : ScriptableObject
{
    public enum SceneKey
    {
        Title,
        Exploration,
        Battle,
        Result
    }

    [System.Serializable]
    public class Entry
    {
        public SceneKey key;
        public SceneObject scene;
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    private Dictionary<SceneKey, SceneObject> dict;

    private void Init()
    {
        dict = new Dictionary<SceneKey, SceneObject>();

        foreach (var e in entries)
        {
            if (e.scene == null || string.IsNullOrEmpty(e.scene.SceneName))
                continue;

            if (dict.ContainsKey(e.key))
            {
                Debug.LogError($"Duplicate SceneKey: {e.key}");
            }
            else
            {
                dict.Add(e.key, e.scene);
            }
        }
    }

    public SceneObject Get(SceneKey key)
    {
        if (dict == null) Init();

        if (dict.TryGetValue(key, out var scene))
            return scene;

        Debug.LogError($"SceneMap: Key not found → {key}");
        return null;
    }
}