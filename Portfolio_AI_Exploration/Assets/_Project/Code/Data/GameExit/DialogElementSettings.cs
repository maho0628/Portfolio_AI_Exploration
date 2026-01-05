using UnityEngine;

/// <summary>
/// ダイアログ要素の設定
/// </summary>
[System.Serializable]
public class DialogElementSettings
{
    #region ゲームオブジェクトの名前に関連する内部管理用変数　オブジェクトを探すときに使用

    /// <summary>
    /// 文章を打つ場所のオブジェクトの名前
    /// </summary>
    [Header("▼オブジェクト名設定")]

    [SerializeField, Tooltip("文章を打つ場所のオブジェクトの名前")]
    private string messageObjectName = "MessageText";

    [Space(15)]

    /// <summary>
    ///確認時にOKと見なすボタンのオブジェクトの名前
    /// </summary>
    [SerializeField, Tooltip("確認時にOKと見なすボタンのオブジェクトの名前")]
    private string confirmButtonName = "ConfirmButton";

    [Space(15)]

    /// <summary>
    /// 確認時にOKではないと見なすボタンのオブジェクトの名前
    /// </summary>
    [SerializeField, Tooltip("確認時にOKではないと見なすボタンのオブジェクトの名前")]
    private string cancelButtonName = "CancelButton";

    #endregion


    #region 各テキストの内容に関連する内部管理用変数

    /// <summary>
    /// 実際に出す文章の内容
    /// </summary>
    [Space(15)]

    [Header("▼テキスト設定")]

    [SerializeField, Tooltip("実際に出す文章の内容")]
    private string messageText = "ゲームを終了しますか？";

    [Space(15)]

    /// <summary>
    /// 確認時にOKと見なすボタンのテキスト内容
    /// </summary>
    [SerializeField, Tooltip("確認時にOKと見なすボタンのテキスト内容")]
    private string confirmButtonText = "はい";

    [Space(15)]

    /// <summary>
    /// 確認時にOKではないと見なすボタンのテキスト内容
    /// </summary>
    [SerializeField, Tooltip("確認時にOKではないと見なすボタンのテキスト内容")]
    private string cancelButtonText = "いいえ";

    #endregion


    #region 各テキストの色変更に関連する内部管理用変数

    /// <summary>
    /// テキストの色をスクリプタブルオブジェクト内で設定するかどうか
    /// </summary>
    [Space(15)]

    [Header("▼色設定（オプション）")]

    [SerializeField, Tooltip("テキストの色をスクリプタブルオブジェクト内で設定するかどうか")]
    private bool useCustomColors = false;

    /// <summary>
    /// 実際に出す文章のテキストの色
    /// </summary>

    [Space(15)]

    [SerializeField, Tooltip("実際に出す文章のテキストの色")]
    private Color messageTextColor = Color.white;

    [Space(15)]

    /// <summary>
    /// 確認時にOKと見なすボタンのテキストの色
    /// </summary>
    [SerializeField, Tooltip("確認時にOKと見なすボタンのテキストの色")]
    private Color confirmButtonTextColor = Color.green;

    [Space(15)]

    /// <summary>
    /// 確認時にOKではないと見なすボタンのテキストの色
    /// </summary>
    [SerializeField, Tooltip("確認時にOKではないと見なすボタンのテキストの色")]
    private Color cancelButtonTextColor = Color.red;

    #endregion


    #region 各テキストのフォントサイズ変更に関連する内部管理用変数

    /// <summary>
    /// テキストのフォントサイズをスクリプタブルオブジェクト内で設定するかどうか
    /// </summary>
    [Space(15)]

    [Header("▼フォントサイズ設定（オプション）")]

    [SerializeField, Tooltip("テキストのフォントサイズをスクリプタブルオブジェクト内で設定するかどうか")]
    private bool useCustomFontSize = false;

    [Space(15)]

    /// <summary>
    /// 実際に出す文章のテキストのフォントサイズ
    /// </summary>
    [SerializeField, Tooltip("実際に出す文章のテキストのフォントサイズ")]
    private int messageFontSize = 100;

    [Space(15)]

    /// <summary>
    /// 確認時にOKと見なすボタンのテキストのフォントサイズ
    /// </summary>
    [SerializeField, Tooltip("確認時にOKと見なすボタンのテキストのフォントサイズ")]
    private int confirmFontSize = 100;

    [Space(15)]

    /// <summary>
    /// 確認時にOKではないと見なすボタンのテキストのフォントサイズ
    /// </summary>
    [SerializeField, Tooltip("確認時にOKではないと見なすボタンのテキストのフォントサイズ")]
    private int cancelFontSize = 100;


    #endregion


    #region  読み取り専用プロパティ (ゲームオブジェクトの名前の内部管理用変数、オブジェクトを探すときに使用)

    /// <summary>
    /// 文章を打つ場所のオブジェクトの名前の読み取り専用
    /// </summary>
    internal string MessageObjectName
    {
        get { return messageObjectName; }
        set { messageObjectName = value; }
    }

    /// <summary>
    /// 確認時にOKと見なすボタンのオブジェクトの名前の読み取り専用
    /// </summary>
    internal string ConfirmButtonName
    {
        get { return confirmButtonName; }
        set { confirmButtonName = value; }
    }

    /// <summary>
    /// 確認時にOKではないと見なすボタンのオブジェクトの名前の読み取り専用
    /// </summary>
    internal string CancelButtonName
    {
        get { return cancelButtonName; }
        set { cancelButtonName = value; }
    }

    #endregion


    #region  読み取り専用プロパティ (各テキストの内容に関連する内部管理用変数)

    /// <summary>
    /// 実際に出す文章の内容の読み取り専用
    /// </summary>
    internal string MessageText
    {
        get { return messageText; }
        set { messageText = value; }
    }

    /// <summary>
    /// 確認時にOKと見なすボタンのテキスト内容の読み取り専用
    /// </summary>
    internal string ConfirmButtonText
    {
        get { return confirmButtonText; }
        set { confirmButtonText = value; }
    }

    /// <summary>
    ///確認時にOKではないと見なすボタンのテキスト内容の読み取り専用
    /// </summary>
    internal string CancelButtonText
    {
        get { return cancelButtonText; }
        set { cancelButtonText = value; }
    }

    #endregion


    #region  読み取り専用プロパティ (各テキストの色変更に関連する内部管理用変数)

    /// <summary>
    /// テキストの色をスクリプタブルオブジェクト内で設定するかどうか
    /// </summary>
    internal bool UseCustomColors
    {
        get { return useCustomColors; }
        set { useCustomColors = value; }
    }

    /// <summary>
    /// 実際に出す文章のテキストの色の読み取り専用
    /// </summary>
    internal Color MessageTextColor
    {
        get { return messageTextColor; }
        set { messageTextColor = value; }
    }

    /// <summary>
    /// 確認時にOKと見なすボタンのテキストの色の読み取り専用
    /// </summary>
    internal Color ConfirmButtonTextColor
    {
        get { return confirmButtonTextColor; }
        set { confirmButtonTextColor = value; }
    }

    /// <summary>
    /// 確認時にOKではないと見なすボタンのテキストの色の読み取り専用
    /// </summary>
    internal Color CancelButtonTextColor
    {
        get { return cancelButtonTextColor; }
        set { cancelButtonTextColor = value; }
    }

    #endregion


    #region  読み取り専用プロパティ (各テキストのフォントサイズ変更に関連する内部管理用変数)

    /// <summary>
    /// テキストのフォントサイズをスクリプタブルオブジェクト内で設定するかどうかの読み取り専用
    /// </summary>
    internal bool UseCustomFontSize
    {
        get { return useCustomFontSize; }
        set { useCustomFontSize = value; }
    }

    /// <summary>
    /// 実際に出す文章のテキストのフォントサイズの読み取り専用
    /// </summary>
    internal int MessageFontSize
    {
        get { return messageFontSize; }
        set { messageFontSize = value; }
    }

    /// <summary>
    /// 確認時にOKと見なすボタンのテキストのフォントサイズの名前の読み取り専用
    /// </summary>
    internal int ConfirmFontSize
    {
        get { return confirmFontSize; }
        set { confirmFontSize = value; }
    }

    /// <summary>
    /// 確認時にOKではないと見なすボタンのテキストのフォントサイズの読み取り専用
    /// </summary>
    internal int CancelFontSize
    {
        get { return cancelFontSize; }
        set { cancelFontSize = value; }
    }

    #endregion
}