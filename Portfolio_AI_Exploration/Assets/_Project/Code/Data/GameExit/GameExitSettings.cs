using UnityEngine;

/// <summary>
/// ゲーム終了処理の設定を管理するスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "GameExitSettings", menuName = "Game Settings/Game Exit Settings")]
public class GameExitSettings : ScriptableObject
{
    #region ダイアログに関連する内部管理用変数　

    /// <summary>
    /// ダイアログを見せるかどうか
    /// </summary>
    [Header("▼確認ダイアログ設定")]

    [SerializeField, Tooltip("ダイアログを見せるかどうか")]
    private bool showConfirmDialog = true;

    [Space(15)]

    /// <summary>
    /// ダイアログの詳細設定
    /// </summary>
    [SerializeField, Tooltip("ダイアログの詳細設定")]
    private DialogElementSettings dialogSettings = new DialogElementSettings();

    [Space(15)]

    /// <summary>
    /// 入れるダイアログのオブジェクトプレハブ
    /// </summary>
    [SerializeField, Tooltip("入れるダイアログのオブジェクトプレハブ")]
    private GameObject confirmDialogPrefab;

    #endregion


    #region  読み取り専用プロパティ (ダイアログに関連する内部管理用変数)

    /// <summary>
    /// ダイアログを見せるかどうかの読み取り専用
    /// </summary>
    internal bool ShowConfirmDialog
    {
        get { return showConfirmDialog; }
        set { showConfirmDialog = value; }
    }

    /// <summary>
    /// ダイアログの詳細設定の読み取り専用
    /// </summary>
    internal DialogElementSettings DialogSettings
    {
        get { return dialogSettings; }
        set { dialogSettings = value; }
    }

    /// <summary>
    /// 入れるダイアログのオブジェクトプレハブの読み取り専用
    /// </summary>
    internal GameObject ConfirmDialogPrefab => confirmDialogPrefab;

    #endregion



    private void OnValidate()
    {

        if (dialogSettings == null)
        {
            dialogSettings = new DialogElementSettings();
        }

    }
}