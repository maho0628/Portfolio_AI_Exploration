using UnityEngine;
using UnityEngine.InputSystem;

public class BattleAITest : BattleAI
{
    [SerializeField] private TPTextPool tpTextPool;

    protected override void Awake()
    {
        base.Awake();

        DebugManager.Log("BattleAI Test Awake");
        DebugManager.Log($"[AI] Initial State = {currentState?.GetType().Name}");
    }

    private void Start()
    {
        InputManager.Instance.Bind(
            ActionMapType.Battle,
            InputActionType.Ultimate,
            OnUltimatePerformed
        );
    }

    private void OnUltimatePerformed(InputAction.CallbackContext _)
    {
        ReceivePlayerCommand(PlayerCommand.Skill);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        InputManager.Instance.Unbind(
            ActionMapType.Battle,
            InputActionType.Ultimate
        );
    }

}