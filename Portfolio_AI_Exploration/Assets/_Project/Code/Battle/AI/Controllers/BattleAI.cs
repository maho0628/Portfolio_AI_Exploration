
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

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
    [SerializeField] protected CharacterStatusSO status;
    public List<SkillSO> skills = new List<SkillSO>();

    [SerializeField] private DamageEffectSettingsSO damageEffectSettings;
    protected BattleStateBase currentState;
    protected BattleBlackboard blackboard;
    public BattleBlackboard Blackboard => blackboard;
    private int skillIndex = 0;
    [SerializeField] int MinimumDamage = 1;
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
    private bool isActive = false;
    public SkillState SkillState => skillState;
    protected bool isInitialized = false;

    [SerializeField]
    private DamageTextPool damageTextPool;

    [SerializeField]
    private UltimatePresentationController ultimatePresentation;
    /// skill入力バッファフラグ
    /// </summary>
    private bool skillInputBuffered;

    private bool isDead;
    protected virtual void Awake()
    {
        // Stateは一度だけ生成（使い捨てしない）
        idleState = new IdleState(this);
        holdState = new HoldState(this);
        attackState = new AttackState(this);
        skillState = new SkillState(this);
        currentState = idleState;
        Initialize();
        skills = status.SkillLoop.ToList();
        //Debug.Log($"HP:{blackboard.CurrentHP} TP:{blackboard.CurrentTP}");


        //foreach (var skill in skills)
        //{
        //    Debug.Log($"Skill:{skill.name} Type:{skill.skillType}");
        //}
    }

    public void Initialize()
    {
        blackboard = new BattleBlackboard(this,
            status.MaxHP,
            status.TPMax
        );
        Debug.LogWarning(status.MaxHP);
        Debug.LogWarning(status.TPMax);

        isDead = false;
        isInitialized = true;

        currentAction = new BattleAction
        {
            actionType = ActionType.Hold,
            duration = 0.5f
        };

    }

    protected virtual void Update()
    {

        if (!isInitialized)
            return;

        if (!isActive)
            return;
        if (!isDead && blackboard.IsDead)
        {
            isDead = true;
            OnDeath();
            return; // ← ここ重要（死後は何もしない）
        }

        if (isDead)
            return;

        currentState.Tick();
    }

    public bool TryDecideSkill()
    {
        Debug.Log($"{name} TryDecideSkill");
        if (currentState == SkillState)
            return false;

        //まずUB条件だけを最優先で確定させる
        bool hasManualInput = skillInputBuffered;
        bool gaugeFull = IsGaugeFull();

        //Debug.Log($"[UB CHECK] TP:{Blackboard.CurrentTP}/{Blackboard.MaxTP} manual:{hasManualInput} full:{gaugeFull}");

        if (gaugeFull)
        {
            var ultimate = skills.FirstOrDefault(s => s.skillType == SkillType.Ultimate);
            if (ultimate != null)
            {
                // 手動があれば消費（なくても自動で出すならここでOK）
                if (hasManualInput)
                    skillInputBuffered = false;
                ultimatePresentation.Play().Forget();
                SkillState.SetSkill(ultimate);
                Debug.Log($"{name} Change To SkillState");
                ChangeState(SkillState);
                return true; // ← ここ超重要。以降は絶対通さない
            }
        }

        // ---- ここから下は通常スキルだけ ----

        SkillSO nextSkill = null;

        int safety = 0;
        do
        {
            nextSkill = skills[skillIndex];
            skillIndex = (skillIndex + 1) % skills.Count;
            safety++;
        }
        while (nextSkill.skillType == SkillType.Ultimate && safety < skills.Count);

        if (nextSkill == null)
            return false;

        SkillState.SetSkill(nextSkill);
        ChangeState(SkillState);

        return true;

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
        return blackboard.CurrentTP >= status.TPMax;
    }

    public void SetTarget(BattleAI target)
    {
        blackboard.SetTarget(target);
    }

    public void DealDamage(SkillSO skill)
    {
        if (Blackboard.Target == null) return;
        if (Blackboard.Target.Blackboard.IsDead) return;

        int attack = status.PhysicalAttack;
        int defense = Blackboard.Target.status.PhysicalDefense;

        int scaled = attack * skill.power / 100;

        //Refactor: ここシリアライズフィールドで書いてるけど将来データ駆動
        int damage = Mathf.Max(MinimumDamage, scaled - defense);
        Debug.Log($"{name} deals {damage} damage");

        Blackboard.Target.TakeDamage(damage);

    }

    private void TakeDamage(int amount)
    {
        blackboard.TakeDamage(amount);
        PlayDamageText(amount);

        PlayDamageReaction();
    }
    private void PlayDamageText(int damage)
    {
        if (damageTextPool == null)
            return;

        var text = damageTextPool.Get();

        text.transform.position = transform.position;

        text.Play(damage);
    }
    protected virtual void OnDeath()
    {
        Debug.Log($"{name} is dead.");
        Debug.Log($"IsDead{blackboard.IsDead}");
        Debug.Log($"OnDeath called by {this}");
    }

    public virtual bool HasWaitInput()
    {
        return false;
    }
    public void ActivateAI()
    {
        isActive = true;
    }
    public virtual void ExecuteSkill()
    {
        blackboard.AddTP(
          SkillState.GetCurrentSkill()
          .tpGainOnHit
      );

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

    private void PlayDamageReaction()
    {
        transform.DOShakePosition(
          damageEffectSettings.shakeDuration,
        new Vector3(
            damageEffectSettings.shakeStrength,
            0f,
            0f
        ),
        damageEffectSettings.shakeVibrato
  );
        PlayHitStop().Forget();
    }



    private async UniTaskVoid PlayHitStop()
    {
        Time.timeScale = damageEffectSettings.hitStopScale;

        await UniTask.Delay(
            System.TimeSpan.FromSeconds(
                damageEffectSettings.hitStopDuration
            ),
            ignoreTimeScale: true
        );
        Time.timeScale = 1f;

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
    public virtual void ReceivePlayerCommand(PlayerCommand command)
    {

        Debug.Log($"{name}'s ReceivePlayerCommand called");
        switch (command)
        {
            case PlayerCommand.Skill:
                BattleResultData.interventionCount++;


                if (currentState == idleState)
                {
                    BattleResultData.successCount++;
                }
                skillInputBuffered = true;
                Debug.Log("skillInputBuffered = true");
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
            duration = 10.0f
        };
    }

}
