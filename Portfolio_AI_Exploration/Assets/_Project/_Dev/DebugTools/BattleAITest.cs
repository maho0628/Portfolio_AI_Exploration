using UnityEngine;
using UnityEngine.InputSystem;



/// <summary>
/// 戦闘AIのテスト用実装。
/// BattleAI の動作確認専用。
/// </summary>
public class BattleAITest : BattleAI
{
    [SerializeField] private InputActionAsset inputActions;
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

    protected override void Update()
    {
        if (demoAction != null && demoAction.IsPressed())
        {
            ReceivePlayerCommand(PlayerCommand.Skill);
        }
        
        //TODO:キャラステータスのSO作成次第書き換え
        
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            TakeDamage(1000);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Blackboard.AddTP(300);
            Debug.Log($"TP: {Blackboard.CurrentTP}/{Blackboard.MaxTP}");
            Debug.Log($"GaugeFull: {IsGaugeFull()}");

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

   
    public override bool HasWaitInput()
    {
        return Input.GetKeyDown(KeyCode.W); // これは仮入力のまま
    }
}
