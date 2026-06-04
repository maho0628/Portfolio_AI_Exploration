using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;



/// <summary>
/// 戦闘AIのテスト用実装。
/// BattleAI の動作確認専用。
/// </summary>
public class BattleAITest : BattleAI
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField]
    private TPTextPool tpTextPool;
    private InputAction demoAction;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("BattleAI Test Awake");
        Debug.Log($"[AI] Initial State = {currentState?.GetType().Name}");

        demoAction = inputActions.FindAction("Skill"); // "Skill" は InputAction 名
    }

    private void OnEnable()
    {
        demoAction?.Enable();
    }


    internal override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.PlayerHit);

    }

    protected override void Update()
    {

        if (demoAction != null && demoAction.IsPressed())
        {
            ReceivePlayerCommand(PlayerCommand.Skill);
        }

      
        base.Update();

    }

    private void OnDisable()
    {
        demoAction?.Disable();
    }

    // 仮入力用（後でPlayerInputControllerに置き換える）

    public override bool IsGaugeFull()
    {
        return base.IsGaugeFull();
    }

    public override void ExecuteSkill()
    {
        base.ExecuteSkill();

        int tpGain =
            SkillState.GetCurrentSkill()
            .tpGainOnHit;

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

    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.PlayerDeath);

    }
    public override bool HasWaitInput()
    {
        return Input.GetKeyDown(KeyCode.W); // これは仮入力のまま
    }
}
