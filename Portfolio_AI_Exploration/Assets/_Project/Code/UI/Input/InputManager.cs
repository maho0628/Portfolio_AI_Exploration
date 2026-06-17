using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input System のアクションを一元管理するマネージャー
/// 
/// ・InputActionAsset から ActionMap / Action を enum で安全に参照
/// ・初期化時にキャッシュして高速アクセス
/// ・InputActionAsset への直接依存を避け、InputSystemの構造変更に強い設計にする
/// ・イベントバインド / アンバインドを一元管理
/// </summary>
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    /// <summary>
    /// InputAction を定義しているアセット
    /// </summary>
    private InputActionAsset inputActionAsset;

    /// <summary>
    /// ActionMap を enum ベースでキャッシュした辞書
    /// （例：Player / UI など）
    /// </summary>
    private readonly Dictionary<ActionMapType, InputActionMap> actionMaps = new();

    /// <summary>
    /// バインド済みの入力イベントを管理する辞書
    /// 
    /// キー：(ActionMap, Action)
    /// 値：登録されたコールバック
    /// </summary>
    private readonly Dictionary<(ActionMapType, InputActionType), Action<InputAction.CallbackContext>> bindings = new();

    /// <summary>
    /// InputManager を初期化し、ActionMap をキャッシュする
    /// 
    ///ゲーム起動時や Input 設定変更時に一度呼ぶ
    /// </summary>
    public void Initialize(InputActionAsset asset)
    {
        inputActionAsset = asset ?? throw new ArgumentNullException(nameof(asset));

        actionMaps.Clear();

        // enum に定義された ActionMap をすべて検索してキャッシュ
        foreach (ActionMapType mapType in Enum.GetValues(typeof(ActionMapType)))
        {
            var map = inputActionAsset.FindActionMap(mapType.ToString());

            if (map != null)
            {
                actionMaps[mapType] = map;
            }
            else
            {
                Debug.LogWarning($"[InputManager] ActionMap not found: {mapType}");
            }
        }
    }

    /// <summary>
    /// 指定した ActionMap / Action にコールバックを登録する
    /// </summary>
    public void Bind(ActionMapType mapType, InputActionType actionType, Action<InputAction.CallbackContext> callback)
    {
        var action = GetActionInternal(mapType, actionType);

        if (action == null)
        {
            Debug.LogWarning($"Bind failed: {mapType}/{actionType}");
            return;
        }

        // 入力発火時に呼ばれるイベントを登録
        action.performed += callback;

        // 念のため有効化（未Enableだと動かないため）
        action.Enable();

        // 後で解除できるように保持
        bindings[(mapType, actionType)] = callback;
    }

    /// <summary>
    /// 指定した ActionMap / Action のバインドを解除する
    /// </summary>
    public void Unbind(ActionMapType mapType, InputActionType actionType)
    {
        var key = (mapType, actionType);

        if (!bindings.TryGetValue(key, out var callback))
            return;

        var action = GetActionInternal(mapType, actionType);

        if (action != null)
        {
            action.performed -= callback;
        }

        bindings.Remove(key);
    }

    /// <summary>
    /// ActionMap / Action から InputAction を取得する内部処理
    /// </summary>
    private InputAction GetActionInternal(ActionMapType mapType, InputActionType actionType)
    {
        if (!actionMaps.TryGetValue(mapType, out var map))
            return null;

        return map.FindAction(actionType.ToString());
    }
}