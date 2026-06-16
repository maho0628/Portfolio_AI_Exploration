using UnityEngine;
using UnityEngine.InputSystem;

public class BattleAITest : BattleAI
{
    [SerializeField] private TPTextPool tpTextPool;

    protected override void Awake()
    {
        base.Awake();

        DebugManager.Log("BattleAI Test Awake");
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

    internal override void ExecuteSkill()
    {
        base.ExecuteSkill();

        int tpGain =
            SkillState.GetCurrentSkill()
            .TPGainOnHit;

        ShowTPGain(tpGain);
    }

    private void ShowTPGain(int amount)
    {

        if (tpTextPool == null)
            return;

        var text = tpTextPool.Get();

        text.transform.position = transform.position;

        text.Play(amount);
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

    protected override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.PlayerHit);

    }

    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.PlayerDeath);

    }

}