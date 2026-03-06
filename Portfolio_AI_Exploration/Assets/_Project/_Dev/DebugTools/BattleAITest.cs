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

        // Refactor:スペースキーが反応しづらく検証しづらいので一時的にコメントアウトほかの挙動確認でき次第（インプットマネージャー製作次第修正予定
        //if (demoAction != null && demoAction.IsPressed())
        //{
        //    ReceivePlayerCommand(PlayerCommand.Skill);
        //}
        
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            ReceivePlayerCommand(PlayerCommand.Skill);
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
