using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;


public enum PlayerCommand
{
    Skill,
    Wait
}

/// <summary>
/// 戦闘AIの本体クラス。
/// Unity（Update / 入力 / HP / UI）と
/// 思考ロジック（State）をつなぐ役割。
/// </summary>
public abstract class BattleAI : MonoBehaviour
{
    protected BattleStateBase currentState;

    // Stateインスタンス
    protected IdleState idleState;
    protected HoldState holdState;
    protected AttackState attackState;
    protected SkillState skillState;

    //行動キュー　のちに行動ループを自由に変更できるようにするための下準備　
    protected BattleAction currentAction;

    public BattleAction CurrentAction => currentAction;

    // 外部参照用（Stateから使う）
    public IdleState IdleState => idleState;
    public HoldState HoldState => holdState;

    public AttackState AttackState => attackState;

    public SkillState SkillState => skillState;

    /// <summary>
    /// skill入力バッファフラグ
    /// </summary>
    private bool skillInputBuffered;


    protected virtual void Awake()
    {
        // Stateは一度だけ生成（使い捨てしない）
        idleState = new IdleState(this);
        holdState = new HoldState(this);
        attackState = new AttackState(this);
        skillState = new SkillState(this);      
        currentState = idleState;
    }

    protected virtual void Update()
    {

        currentState.Tick();
    }

    /// <summary>
    /// Stateを切り替える唯一の窓口。
    /// State自身は直接他Stateを操作せず、
    /// 必ず owner を通じて遷移を要求する。
    /// </summary>
    public virtual void ChangeState(BattleStateBase nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("ChangeState called with null");
            return;
        }

        if (currentState == nextState)
        {
            return;
        }
        

        Debug.Log($"State Change: {currentState.GetType().Name} -> {nextState.GetType().Name}");

        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }

    // ===== 以下は State から呼ばれる「能力」 =====

    public virtual bool IsGaugeFull()
    {
        // 仮実装
        return false;
    }



    public virtual bool HasWaitInput()
    {
        return false;
    }

    public void ExecuteSkill()
    {
        Debug.Log("Skill Executed");
    }

    public bool HasCurrentAction()
    {
        return currentAction != null;
    }

    public void SetCurrentAction(BattleAction action)
    {
        currentAction = action;
    }

    public void ClearCurrentAction()
    {
        currentAction = null;
    }


    public BattleStateBase GetStateForCurrentAction()
    {
        if (currentAction == null)
        {
            return IdleState;

        }

        Debug.Log($"[AI] Action:{currentAction?.actionType}");

        return currentAction.actionType switch
        {
            ActionType.Attack => AttackState,
            ActionType.Hold => HoldState,
            ActionType.Wait => IdleState, // Waitは何もしない＝IdleでOK
            _ => IdleState
        };

    }
    public void ReceivePlayerCommand(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.Skill:
                skillInputBuffered = true;
                break;
        }
    }

    public bool ConsumeSkillInput()
    {
        if (!skillInputBuffered) return false;
        skillInputBuffered = false;
        return true;
    }
    public virtual BattleAction GetDefaultAction()
    {
        return new BattleAction
        {
            actionType = ActionType.Hold,
            duration =10.0f
        };
    }

}
