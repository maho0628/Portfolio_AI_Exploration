using System.Linq;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// 戦闘AIの本体クラス。
/// Unity（Update / 入力 / HP / UI）と
/// 思考ロジック（State）をつなぐ役割。
/// </summary>
public abstract class BattleAI : MonoBehaviour
{
    // ==================================================
    // Serialized Fields
    // ==================================================

    #region Serialized Fields

    /// <summary>
    /// キャラクターのステータスとスキル構成。
    /// スキル構成やスキルループ順、各種能力値を保持する。
    /// </summary>
    [SerializeField,Tooltip("キャラクターのステータスとスキル構成")]
    private CharacterStatusSO status;

    /// <summary>
    /// 被弾エフェクトのスクリプタブルオブジェクト
    /// シェイクやヒットストップなど
    /// </summary>
    [SerializeField,Tooltip("被弾エフェクトのスクリプタブルオブジェクト")]
    private DamageEffectSettingsSO damageEffectSettings;

    /// <summary>
    /// 被弾したときのHP減少をオブジェクトプールで表示するための変数
    /// </summary>
    [SerializeField,Tooltip("ダメージ数値表示用のオブジェクトプール")]
    private DamageTextPool damageTextPool;

    /// <summary>
    /// 必殺技の演出などを呼び出すためのクラスのインスタンス
    /// </summary>
    [SerializeField,Tooltip("必殺技演出を制御するコントローラー")]
    private UltimatePresentationController ultimatePresentation;

    #endregion


    // ==================================================
    // Runtime State
    // ==================================================

    #region Runtime State

    /// <summary>
    /// AIの更新処理を有効にするかどうか
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// 初期化が完了しているか
    /// </summary>
    private bool isInitialized = false;

    /// <summary>
    /// 死亡処理が実行済みか
    /// </summary>
    private bool deathHandled;

    #endregion


    // ==================================================
    // State Machine
    // ==================================================

    #region State Machine

    /// <summary>
    /// 現在どのステートなのか
    /// </summary>
    private BattleState currentState;

    /// <summary>
    /// IdleStateインスタンス
    /// </summary>
    private IdleState idleState;

    /// <summary>
    /// SkillPreparationStateインスタンス
    /// </summary>
    private SkillPreparationState skillPreparationState;

    /// <summary>
    /// SkillExecutionStateインスタンス
    /// </summary>
    private SkillExecutionState skillExecutionState;

    #endregion


    // ==================================================
    // Combat Core
    // ==================================================

    #region Combat Core

    /// <summary>
    /// 各キャラクターのブラックボード。
    /// 現在のHPや スキル命中時に増加するTP（必殺技ゲージ）量、ターゲット情報などを管理する。
    /// </summary>
    private BattleBlackboard blackboard;

    #endregion


    // ==================================================
    // Skill Flow
    // ==================================================

    #region Skill Flow

    /// <summary>
    /// スキルループの現在位置
    /// </summary>
    private int skillCycleIndex = 0;

    /// <summary>
    /// 現在使用予定のスキルのSO
    /// </summary>
    private SkillSO nextSkill;

    #endregion


    // ==================================================
    // Input / Intervention
    // ==================================================

    #region Input / Intervention

    /// <summary>
    /// 介入受付中か
    /// </summary>
    private bool isManualInterventionEnabled;

    /// <summary>
    /// 手動介入の要求がまだ残っているか
    /// </summary>
    private bool hasInterventionRequest;

    #endregion


    // ==================================================
    // Ultimate Management
    // ==================================================

    #region Ultimate Management

    /// <summary>
    /// 必殺技が起動中かどうか
    /// </summary>
    private bool isPlayingUltimate;

    #endregion


    // ==================================================
    // Properties - Data
    // ==================================================

    #region Properties - Data

    /// <summary>
    ///ブラックボードインスタンスを取得する
    /// </summary>
    internal BattleBlackboard BB => blackboard;
    /// <summary>
    /// 被弾エフェクトのスクリプタブルオブジェクトインスタンスを取得する
    /// </summary>
    protected DamageEffectSettingsSO DamageEffectSettings => damageEffectSettings;

    #endregion


    // ==================================================
    // Properties - State Machine
    // ==================================================

    #region Properties - State Machine

    /// <summary>
    /// IdleState インスタンスを取得する
    /// </summary>
    internal IdleState IdleState => idleState;

    /// <summary>
    /// SkillPreparationState インスタンスを取得する
    /// </summary>
    internal SkillPreparationState SkillPreparationState => skillPreparationState;

    /// <summary>
    /// SkillExecutionState インスタンスを取得する
    /// </summary>
    internal SkillExecutionState SkillExecutionState => skillExecutionState;

    #endregion


    // ==================================================
    // Unity Lifecycle (Entry Point)
    // ==================================================

    #region Unity Lifecycle (Entry Point)

    void Awake()
    {
        //インスタンス生成を開始
        Setup();

        //現在のステートをIdleに決定
        currentState = idleState;

        //初期設定を呼び出し
        InitializeRuntime();
    }

    void Update()
    {
        //初期化されていなければ終了
        if (!isInitialized)
        {
            return;
        }

        // AIが有効化されていない場合は処理しない
        if (!isActive)
        {
            return;
        }

        //  死亡処理の多重実行防止
        if (!deathHandled && blackboard.IsDead)
        {
            deathHandled = true;
            OnDeath();
            return;
        }

        //すでに死亡していたら処理しない
        if (deathHandled)
        {
            return;
        }

        //Tickを進める
        currentState.Tick();
    }

    #endregion


    // ==================================================
    // Initialization (Setup)
    // ==================================================

    #region Initialization (Setup)

    /// <summary>
    /// BattleAIの依存オブジェクトを構築する
    /// State / Blackboard などの初期インスタンス生成を行う
    /// </summary>
    private void Setup()
    {
        //ブラックボードの初期化
        blackboard = new BattleBlackboard( status.MaxHP, status.TPMax);

        idleState = new IdleState(this);
        skillPreparationState = new SkillPreparationState(this);
        skillExecutionState = new SkillExecutionState(this);
    }

    #endregion


    // ==================================================
    //  Initialization (Runtime)
    // ==================================================

    #region Initialization (Runtime)

    /// <summary>
    /// BattleAIの実行状態を初期化する
    /// 初期化完了フラグと死亡状態を設定する
    /// </summary>
    private void InitializeRuntime()
    {
        deathHandled = false;
        isInitialized = true;
    }

    #endregion


    // ==================================================
    //   AI Lifecycle (Runtime Control)
    // ==================================================

    #region AI Lifecycle (Runtime Control)

    /// <summary>
    /// 死亡時の演出を行うための関数
    /// 派生クラスでキャラクターごとにSEを鳴らしている
    /// </summary>
    protected virtual void OnDeath()
    {
        DebugManager.Log($"{name} is dead.");
        DebugManager.Log($"IsDead{blackboard.IsDead}");
        DebugManager.Log($"OnDeath called by {this}");
    }

    /// <summary>
    /// AIの更新処理を開始する。
    /// </summary>
    public void ActivateAI()
    {
        isActive = true;
    }

    #endregion


    // ==================================================
    //  State Management
    // ==================================================

    #region   State Management

    /// <summary>
    /// Stateを切り替える唯一の窓口。
    /// State自身は直接他Stateを操作せず、
    /// 必ず owner を通じて遷移を要求する。
    /// </summary>
    internal void SwitchState(BattleState nextState)
    {
        if (nextState == null)
        {
            DebugManager.LogError("ChangeState called with null");
            return;
        }

        if (currentState == nextState)
        {
            return;
        }

        DebugManager.Log($"State Change: {currentState.GetType().Name} -> {nextState.GetType().Name}");

        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }

    #endregion


    // ==================================================
    // Skill Decision（判断）
    // ==================================================

    #region Skill Decision（判断）

    /// <summary>
    /// 次に実行するスキルを決定し、実行を開始する。
    /// 必殺技を優先して判定し、条件を満たさない場合は通常スキルを選択する。
    /// </summary>
    /// <returns>
    /// true: スキルの開始に成功した
    /// false: 実行可能なスキルが存在しない
    /// </returns>
    internal bool TryDecideNextAction()
    {
        //必殺技再生中ならスキル決定しない
        if (isPlayingUltimate)
        {
            return false;
        }

        //// SkillExecutionState実行中は再度スキル決定しないためのガード
        if (currentState == SkillExecutionState)
        {
            return false;
        }

        //まず必殺技条件だけを最優先で確定させる
        bool gaugeFull = IsGaugeFull();

        //ゲージがフルかつそのスキルが必殺技なら
        if (gaugeFull && TryActivateUltimate())
        {
            return true;
        }

        SkillSO nextSkill = SelectNextNormalSkill();

        if (nextSkill == null)
        {
            return false;
        }

        return StartSkillExecution(nextSkill);
    }

    /// <summary>
    /// 必殺技の実行を開始する。
    /// 必殺技演出を再生し、実行するスキルを設定する。
    /// </summary>
    /// <returns>
    /// true: 必殺技の開始に成功した
    /// false: 実行可能な必殺技が存在しない
    /// </returns>
    private bool TryActivateUltimate()
    {
        //必殺技スキルを取得する
        var ultimate = GetUltimateSkill();

        if (ultimate == null)
        {
            return false;
        }

        //必殺技の起動中と判定する
        isPlayingUltimate = true;

        //必殺技の演出をスタートする
        ultimatePresentation.Play().Forget();

        //必殺技開始をするかどうかを判定
        return StartSkillExecution(ultimate);
    }

    /// <summary>
    /// スキルループから必殺技を取得する。
    /// </summary>
    /// <returns>
    /// 必殺技のスキル情報。
    /// 存在しない場合は null。
    /// </returns>
    internal SkillSO GetUltimateSkill()
    {
        return status.SkillLoop.FirstOrDefault(
           s => s.SkillCategory == SkillType.Ultimate);
    }

    /// <summary>
    /// 実行するスキルを設定し、SkillPreparationStateへ遷移する。
    /// </summary>
    /// <param name="skill">実行するスキル</param>
    /// <returns>
    /// true: スキルの開始に成功した
    /// false: スキルが設定されていない
    /// </returns>
    private bool StartSkillExecution(SkillSO skill)
    {
        if (skill == null)
        {
            return false;
        }

        SetNextSkill(skill);

        SwitchState(SkillPreparationState);

        return true;
    }

    /// <summary>
    /// 次に実行するスキルを設定する。
    /// </summary>
    /// <param name="skill">設定するスキル</param>
    internal void SetNextSkill(SkillSO skill)
    {
        nextSkill = skill;
    }

    /// <summary>
    /// スキルループから次に実行する通常スキルを取得する。
    /// 必殺技は対象外とする。
    /// </summary>
    /// <returns>
    /// 次に実行する通常スキル。
    /// 取得できない場合は null。
    /// </returns>
    private SkillSO SelectNextNormalSkill()
    {
        SkillSO nextSkill = null;

        int safety = 0;

        while (safety < status.SkillLoop.Count)
        {
            nextSkill = status.SkillLoop[skillCycleIndex];
            skillCycleIndex = (skillCycleIndex + 1) % status.SkillLoop.Count;
            safety++;

            //必殺技では無かったらスキルデータを返す
            if (nextSkill.SkillCategory != SkillType.Ultimate)
            {
                return nextSkill;
            }
        }

        return null;
    }

    #endregion


    // ==================================================
    // Skill Execution (実行）
    // ==================================================

    #region Skill Execution (実行）

    /// <summary>
    /// 現在使用予定のスキルを取得する
    /// </summary>
    /// <returns>
    /// 現在使用予定のスキル。設定されていない場合は null。
    /// </returns>    
    internal SkillSO GetNextSkill()
    {
        return nextSkill;
    }

    /// <summary>
    /// 現在のスキルを実行し、必要な後処理を行う。
    /// </summary>
    internal virtual void RunSkill()
    {
        // スキル命中時に増加するTP（必殺技ゲージ）を加算する
        blackboard.AddTP(SkillExecutionState.GetCurrentSkill().TPGainOnHit);
    }

    #endregion


    // ==================================================
    // ManualIntervention
    // ==================================================

    #region ManualIntervention

    /// <summary>
    /// 手動介入要求が行われた時に呼ばれる
    /// </summary>
    private void RequestManualIntervention()
    {
        hasInterventionRequest = true;
    }

    /// <summary>
    /// 手動介入要求が存在する場合にそれを消費し、結果を返す
    /// </summary>
    /// <returns>
    /// true = 要求を消費できた
    /// false = 要求が存在しなかった
    /// </returns>
    internal bool ConsumeManualInterventionRequest()
    {
        //手動介入の要求がまだ残っていないのであれば
        if (!hasInterventionRequest)
        {
            //falseを返してその後処理しない
            return false;
        }

        //手動介入の要求が残っているので消費
        hasInterventionRequest = false;

        return true;
    }

    /// <summary>
    /// 手動介入の受付状態を設定する
    /// </summary>
    /// <param name="isOpen">
    /// true: 手動介入を受け付ける
    /// false: 手動介入を受け付けない
    /// </param>
    internal void SetManualInterventionEnabled(bool isOpen)
    {
        isManualInterventionEnabled = isOpen;
    }

    /// <summary>
    /// プレイヤー入力を受け取り、手動介入要求を処理する。
    /// </summary>
    /// <param name="command">受信したプレイヤーコマンド</param>

    protected virtual void HandleManualInterventionInput(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.Skill:

                //手動介入に挑戦したのでカウントを増やす
                BattleResultData.interventionCount++;

                // 成功時のみ予約
                if (isManualInterventionEnabled)
                {
                    BattleResultData.successCount++;

                    //手動介入要求を許可する
                    RequestManualIntervention();

                    DebugManager.Log("hasManualInterventionRequest = true");
                }
                break;
        }
    }

    #endregion


    // ==================================================
    // Combat（戦闘処理）
    // ==================================================

    #region Combat（戦闘処理）

    /// <summary>
    /// 対象にダメージを適用する。
    /// 対象の生存確認、攻撃SEの再生、ダメージ計算を行う。
    /// </summary>
    /// <param name="skill">ダメージ計算に使用するスキル情報</param>
    internal void DealDamage(SkillSO skill)
    {
        //ブラックボードにターゲットが設定されていなければ処理しない
        if (BB.CurrentTarget == null)
        {
            return;
        }

        //与えようとしているターゲットが死んでいなければ
        if (BB.CurrentTarget.BB.IsDead)
        {
            return;
        }

        //必殺技なら
        if (skill.SkillCategory == SkillType.Ultimate)
        {
            //必殺技時の攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.UltimateImpact);
        }
        else
        {
            //必殺技以外のスキルの攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.Attack);
        }

        //ダメージ計算関数を呼ぶ
        int damage = CalculateDamage(skill);

        //ターゲットへのダメージ量が決定したのでブラックボードに渡す
        BB.CurrentTarget.TakeDamage(damage);
    }

    /// <summary>
    /// スキルダメージを計算する。
    /// 攻撃力にスキル倍率を適用し、対象の防御力を差し引く。
    /// 計算結果が最低保証ダメージを下回る場合は、最低保証ダメージを返す。
    /// </summary>
    /// <param name="skill">ダメージ計算に使用するスキル情報</param>
    private int CalculateDamage(SkillSO skill)
    {
        int attack = status.PhysicalAttack;
        int defense = BB.CurrentTarget.status.PhysicalDefense;

        //// スキル倍率を適用した、理論上の攻撃力を計算
        float scaled = attack * skill.Multiplier;

        // 防御力を差し引いたダメージを整数に丸める。
        // 計算結果がスキルの最低保証ダメージを下回る場合は、最低保証ダメージを返す。
        return Mathf.Max(skill.MinimumDamage,
            Mathf.RoundToInt(scaled - defense)
        );
    }

    /// <summary>
    /// ダメージを受け、HPを減少させる。
    /// 被弾演出とダメージ表示も実行する。
    /// </summary>
    /// <param name="amount">受けるダメージ量</param>
    protected virtual void TakeDamage(int amount)
    {
        //ブラックボードにダメージの値を渡す
        blackboard.TakeDamage(amount);

        //ダメージのエフェクトを出す
        PlayDamageText(amount);

        //ダメージが与えられた時の演出を行う
        PlayDamageReaction();
    }

    #endregion


    // ==================================================
    //   Ultimate Management
    // ==================================================

    #region Ultimate Management

    /// <summary>
    /// 必殺技の実行ロックを解除する。
    /// </summary>
    internal void ResetUltimateLock()
    {
        isPlayingUltimate = false;
    }

    /// <summary>
    /// 必殺技ゲージ（TP）が最大値に達しているかを返す。
    /// </summary>
    /// <returns>
    /// true: TPが最大値以上
    /// false: TPが最大値未満
    /// </returns>
    internal bool IsGaugeFull()
    {
        return blackboard.CurrentTP >= status.TPMax;
    }

    #endregion


    // ==================================================
    // Presentation（演出）
    // ==================================================

    #region Presentation（演出）

    /// <summary>
    /// ダメージ数値を表示する。
    /// </summary>
    /// <param name="damage">表示するダメージ量</param>

    private void PlayDamageText(int damage)
    {
        if (damageTextPool == null) return;

        var text = damageTextPool.Get();
        text.transform.position = transform.position;
        text.PlayDamage(damage);
    }

    /// <summary>
    /// ヒットストップや画面シェイクをする関数
    /// </summary>
    protected virtual void PlayDamageReaction()
    {
        //画面シェイクする
        transform.DOShakePosition(
            damageEffectSettings.ShakeDuration,
            new Vector3(
                damageEffectSettings.ShakeStrength,
                0f, 0f ),
            damageEffectSettings.ShakeVibrato
        );

        //ヒットストップ関数を呼ぶ
        PlayHitStop().Forget();
    }

    /// <summary>
    /// ヒットストップを再生する。
    /// 指定時間だけ Time.timeScale を変更し、その後元に戻す。
    /// </summary>
    private async UniTaskVoid PlayHitStop()
    {
        //タイムスケールをヒットストップ用の時間に設定する
        Time.timeScale = damageEffectSettings.HitStopScale;

        //タイムスケールを無視した状態でヒットストップ用の待ち時間分待つ
        await UniTask.Delay(
            System.TimeSpan.FromSeconds(
                damageEffectSettings.HitStopDuration),
            ignoreTimeScale: true
        );

        //元の時間に戻して正常の時間の進み方にする
        Time.timeScale = 1f;
    }

    #endregion

}