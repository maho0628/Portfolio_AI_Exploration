using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンに対応するBGM設定の一覧を保持する ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "SceneBGMConfig", menuName = "GameData/SceneBGMConfigTable")]
public class SceneBGMConfigTable : ScriptableObject
{
    #region シーンBGMのリストやディクショナリの内部管理用変数

    /// <summary>
    /// シーンに対応するBGM設定をまとめたリスト
    /// </summary>
    [Header("▼シーンに対応するBGMの一覧")]

    [SerializeField, Tooltip(" シーンに対応するBGM設定をまとめたリスト")]
    private List<SceneBGMConfig> sceneBgmConfigLists = new List<SceneBGMConfig>();

    /// <summary>
    /// シーンに対応するBGMのリストのディクショナリ
    /// </summary>
    private Dictionary<string, BGMName> sceneToBgmIdDict;

    #endregion


    #region ゲッターメソッド

    /// <summary>
    /// sceneNameに対応したBGMIDを返す
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns>string</returns>
    internal BGMName GetSceneBgmConfigName(string sceneName)
    {
        if (sceneToBgmIdDict == null)
        {
            InitializeDictionary();
        }

        sceneToBgmIdDict.TryGetValue(sceneName, out var bgmId);
        return bgmId;
    }

    #endregion


    #region Unity イベント

    private void OnEnable()
    {
        // ScriptableObject 再読み込み時にも対応
        InitializeDictionary();
    }

    #endregion


    #region プライベートメソッド

    /// <summary>
    /// ディクショナリの初期化
    /// </summary>
    private void InitializeDictionary()
    {
        sceneToBgmIdDict = new Dictionary<string, BGMName>();
        foreach (var sceneBgm in sceneBgmConfigLists)
        {
            //コンフィグのシーンの名前とBgmIdが両方とも空欄でないなら
            if (!string.IsNullOrEmpty(sceneBgm.SceneName) && !string.IsNullOrEmpty(sceneBgm.BgmId.ToString()))
            {
                //ディクショナリ内にSceneNameがないなら
                if (!sceneToBgmIdDict.ContainsKey(sceneBgm.SceneName))
                {
                    // ディクショナリ内にSceneNameとBgmIdを追加

                    sceneToBgmIdDict.Add(sceneBgm.SceneName, sceneBgm.BgmId);
                }
                else
                {
                    //エラー出して終了
                    DebugManager.LogWarning($"[SceneBGMConfigTable] シーン '{sceneBgm.SceneName}' は既に登録されています。");
                }
            }
        }
    }

    #endregion


}
