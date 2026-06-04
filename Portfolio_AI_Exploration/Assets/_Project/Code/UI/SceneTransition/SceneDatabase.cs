using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneDatabase", menuName = "Scene/SceneDatabase")]
public class SceneDatabase : ScriptableObject
{
    [SerializeField] private List<SceneData> scenes = new List<SceneData>();

    private Dictionary<string, SceneData> sceneDict;

    [System.Serializable]
    public class SceneData
    {
        public SceneObject scene;
        public SceneObject nextScene;
        public SceneObject previousScene;
    }
    private void EnsureInit()
    {
        if (sceneDict == null || sceneDict.Count == 0)
        {
            Init();


        }
    }
    private void Init()
    {
        sceneDict = new Dictionary<string, SceneData>();

        foreach (var s in scenes)
        {
            if (s.scene == null || string.IsNullOrEmpty(s.scene.SceneName))
                continue;


            if (sceneDict.ContainsKey(s.scene.SceneName))
            {
                Debug.LogError($"Duplicate scene key: {s.scene.SceneName}");
            }
            else
            {
                sceneDict.Add(s.scene.SceneName, s);
            }
        }
    }




    public SceneObject GetScene(string name)
    {
        EnsureInit();
        return sceneDict.TryGetValue(name, out var data) ? data.scene : null;


 
    }

    public SceneObject GetNextScene(string name)
    {
        EnsureInit();
        sceneDict.TryGetValue(name, out var data);

        Debug.LogWarning($"Current: {name}");
        Debug.LogWarning($"Next: {data?.nextScene?.SceneName}");

        return data?.nextScene;
    }

    public SceneObject GetPreviousScene(string name)
    {
        EnsureInit();
        return sceneDict.TryGetValue(name, out var data) ? data.previousScene : null;
    }
}