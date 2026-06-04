using UnityEngine;

using UnityEditor;


public class RemoveMissingScripts
{
    [MenuItem("Tools/Remove Missing Scripts From Selected (Deep)")]
    static void Remove()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Transform[] children = go.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                if (count > 0)
                    Debug.Log(child.name + ": Removed " + count + " missing scripts");
            }
        }
    }
}


