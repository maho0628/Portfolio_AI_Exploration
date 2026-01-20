using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

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

    // 外部参照用（Stateから使う）
    public IdleState IdleState => idleState;
    public HoldState HoldState => holdState;

    public AttackState AttackState => attackState;  

    protected virtual void Awake()
    {
        // Stateは一度だけ生成（使い捨てしない）
        idleState = new IdleState(this);
        holdState = new HoldState(this);
        attackState = new AttackState(this);    
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
        Debug.Log($"State Change: {currentState.GetType().Name} -> {nextState.GetType().Name}");

        currentState = nextState;
    }

    // ===== 以下は State から呼ばれる「能力」 =====

    public virtual bool IsGaugeFull()
    {
        // 仮実装
        return false;
    }

    public virtual bool HasSkillInput()
    {
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
}
