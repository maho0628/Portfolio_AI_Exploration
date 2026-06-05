using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/// <summary>
/// Input System のアクションを一元管理するマネージャー
/// ・ActionMap / Action を enum ベースで安全に取得
/// ・初期化時にキャッシュして高速化
/// ・InputActionAsset への直接アクセスを隠蔽する
/// </summary>
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private InputActionAsset inputActionAsset;

    private readonly Dictionary<ActionMapType, InputActionMap> actionMaps = new();

    private readonly Dictionary<(ActionMapType, InputActionType), Action<InputAction.CallbackContext>> bindings = new();

    /// <summary>
    /// InputActionAsset を初期化し、ActionMap をキャッシュする
    /// </summary>
    public void Initialize(InputActionAsset asset)
    {
        inputActionAsset = asset ?? throw new ArgumentNullException(nameof(asset));

        actionMaps.Clear();

        foreach (ActionMapType mapType in Enum.GetValues(typeof(ActionMapType)))
        {
            var map = inputActionAsset.FindActionMap(mapType.ToString());

            if (map != null)
                actionMaps[mapType] = map;
        
            else
            {
                Debug.LogWarning($"[InputManager] ActionMap not found: {mapType}");
            }
        }
    }

    public void Bind(ActionMapType mapType, InputActionType actionType, Action<InputAction.CallbackContext> callback)
    {
        var action = GetActionInternal(mapType, actionType);

        if (action == null)
        {
            Debug.LogWarning($"Bind failed: {mapType}/{actionType}");
            return;
        }

        action.performed += callback;
        action.Enable();

        bindings[(mapType, actionType)] = callback;
    }

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
    /// 指定した ActionMap / Action から InputAction を取得する
    /// </summary>
    private InputAction GetActionInternal(ActionMapType mapType, InputActionType actionType)
    {
        if (!actionMaps.TryGetValue(mapType, out var map))
            return null;

        return map.FindAction(actionType.ToString());
    }

}