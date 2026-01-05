using UnityEngine;

/// <summary>
/// シーンごとのBGMの設定データ
/// </summary>
[System.Serializable]
public class SceneBGMConfig
{
    #region シーンBGMの設定の内部管理用変数

    /// <summary>
    /// 対象のシーン名
    /// </summary>
    [SerializeField, Tooltip("対象のシーン名")]
    private string sceneName;

    [Space(15)]

    /// <summary>
    /// 再生するBGMのID
    /// </summary>
    [SerializeField, Tooltip("再生するBGMのID")]
    private BGMName bgmId;

    #endregion


    #region 読み取り専用プロパティ(シーンBGMの設定の内部管理用変数)

    /// <summary>
    /// 対象のシーン名の読み取り専用
    /// </summary>
    internal string SceneName => sceneName;

    /// <summary>
    /// 再生するBGMのIDの読み取り専用
    /// </summary>
    internal BGMName BgmId => bgmId;

    #endregion
}