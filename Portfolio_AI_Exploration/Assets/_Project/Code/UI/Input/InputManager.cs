using UnityEngine;

using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private InputActionAsset inputActionAsset;

    public void Initialize(InputActionAsset asset)
    {
        inputActionAsset = asset;
    }

    public InputAction GetAction(
      ActionMapType mapType,
      InputActionType actionType)
    {
        return inputActionAsset
            .FindActionMap(mapType.ToString())
            ?.FindAction(actionType.ToString());
    }
}
