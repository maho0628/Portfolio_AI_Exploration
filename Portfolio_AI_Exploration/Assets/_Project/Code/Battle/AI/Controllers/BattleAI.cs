using System.Collections.Generic;
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
    /// <summary>
    /// キャラクターのステータス情報。
    /// スキル構成やスキルループ順、各種能力値を保持する。
    /// </summary>
    [SerializeField]
    private CharacterStatusSO status;

    /// <summary>
    /// 被弾エフェクトのスクリプタブルオブジェクト
    /// シェイクやヒットストップなど
    /// </summary>
    [SerializeField]
    private DamageEffectSettingsSO damageEffectSettings;

    /// <summary>
    /// 被弾したときのHP減少をオブジェクトプールで表示するための変数
    /// </summary>
    [SerializeField]
    private DamageTextPool damageTextPool;

    /// <summary>
    /// 必殺技の演出などを呼び出すためのクラスのインスタンス
    /// </summary>
    [SerializeField]
    private UltimatePresentationController ultimatePresentation;

    /// <summary>
    /// 現在どのステートなのか
    /// </summary>
    private BattleStateBase currentState;

    /// <summary>
    /// 各キャラクターのブラックボード。
    /// 現在のHPや スキル命中時に増加するTP（必殺技ゲージ）量、ターゲット情報などを管理する。
    /// </summary>
    private BattleBlackboard blackboard;

    /// <summary>
    /// スキルループの現在位置
    /// </summary>
    private int skillIndex = 0;

    /// <summary>
    /// IdleStateインスタンス
    /// </summary>
    private IdleState idleState;

    /// <summary>
    /// HoldStateインスタンス
    /// </summary>
    private HoldState holdState;

    /// <summary>
    /// SkillStateインスタンス
    /// </summary>
    private SkillState skillState;

    /// <summary>
    /// AIの更新処理を有効にするかどうか
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// 初期化が完了しているか
    /// </summary>
    private bool isInitialized = false;

    /// <summary>
    /// 介入受付中か
    /// </summary>
    private bool isInterventionWindowOpen;

    /// <summary>
    /// 手動介入の要求がまだ残っているか
    /// </summary>
    private bool hasManualInterventionRequest;

    /// <summary>
    /// 必殺技が起動中かどうか
    /// </summary>
    private bool isPlayingUltimate;

    /// <summary>
    /// 死亡処理が実行済みか
    /// </summary>
    private bool isDead;

    /// <summary>
    /// 現在使用予定のスキルのSO
    /// </summary>
    private SkillSO pendingSkill;

    /// <summary>
    ///ブラックボードインスタンスを取得する
    /// </summary>
    internal BattleBlackboard Blackboard => blackboard;

    /// <summary>
    /// 被弾エフェクトのスクリプタブルオブジェクトインスタンスを取得する
    /// </summary>
    protected DamageEffectSettingsSO DamageEffectSettings => damageEffectSettings;

    /// <summary>
    /// IdleState インスタンスを取得する
    /// </summary>
    internal IdleState IdleState => idleState;

    /// <summary>
    /// HoldState インスタンスを取得する
    /// </summary>
    internal HoldState HoldState => holdState;

 

    /// <summary>
    /// SkillState インスタンスを取得する
    /// </summary>
    internal SkillState SkillState => skillState;

    /// <summary>
    /// 現在選択されている行動情報
    /// </summary>
    private BattleAction currentAction;

    /// <summary>
    /// 現在の行動情報を取得する。
    /// 未設定の場合は null。
    /// </summary>
    internal BattleAction CurrentAction => currentAction;

    protected virtual void Awake()
    {
        // Stateは一度だけ生成
        idleState = new IdleState(this);
        holdState = new HoldState(this);
        skillState = new SkillState(this);

        //現在のステートをIdleに決定
        currentState = idleState;

        //初期設定を呼び出し
        Initialize();

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
        if (!isDead && blackboard.IsDead)
        {
            isDead = true;
            OnDeath();
            return;
        }

        //すでに死亡していたら処理しない
        if (isDead)
        {
            return;
        }

        //Tickを進める
        currentState.Tick();
    }

    /// <summary>
    /// AIの初期化を行う
    /// </summary>
    private void Initialize()
    {
        //ブラックボードの初期化
        blackboard = new BattleBlackboard(this, status.MaxHP, status.TPMax);

        //死亡していない、初期化完了に設定
        isDead = false;
        isInitialized = true;

    }

    /// <summary>
    /// 手動介入要求が行われた時に呼ばれる
    /// </summary>
    private void RequestManualSkill()
    {
        hasManualInterventionRequest = true;
    }

    /// <summary>
    /// 次に実行するスキルを設定する。
    /// </summary>
    /// <param name="skill">設定するスキル</param>
    internal void SetPendingSkill(SkillSO skill)
    {
        pendingSkill = skill;
    }

    /// <summary>
    /// 現在使用予定のスキルを取得する
    /// </summary>
    /// <returns>
    /// 現在使用予定のスキル。設定されていない場合は null。
    /// </returns>    
    internal SkillSO GetPendingSkill()
    {
        return pendingSkill;
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
            s => s.SkillCategory == SkillType.Ultimate
        );
    }

    /// <summary>
    /// 手動介入要求が存在する場合にそれを消費し、結果を返す
    /// </summary>
    /// <returns>
    /// true = 要求を消費できた
    /// false = 要求が存在しなかった
    /// </returns>
    internal bool ConsumeManualSkillRequest()
    {
        //手動介入の要求がまだ残っていないのであれば
        if (!hasManualInterventionRequest)
        {
            //falseを返してその後処理しない
            return false;
        }

        //手動介入の要求が残っているので消費
        hasManualInterventionRequest = false;

        return true;
    }

    /// <summary>
    /// 手動介入の受付状態を設定する
    /// </summary>
    /// <param name="isOpen">
    /// true: 手動介入を受け付ける
    /// false: 手動介入を受け付けない
    /// </param>
    internal void SetInterventionWindow(bool isOpen)
    {
        isInterventionWindowOpen = isOpen;
    }

    /// <summary>
    /// 次に実行するスキルを決定し、実行を開始する。
    /// 必殺技を優先して判定し、条件を満たさない場合は通常スキルを選択する。
    /// </summary>
    /// <returns>
    /// true: スキルの開始に成功した
    /// false: 実行可能なスキルが存在しない
    /// </returns>
    internal bool TryDecideSkill()
    {

        //UB再生中ならスキル決定しない
        if (isPlayingUltimate)
        {
            return false;
        }

        //// SkillState実行中は再度スキル決定しないためのガード
        if (currentState == SkillState)
        {
            return false;
        }

        //まずUB条件だけを最優先で確定させる
        bool gaugeFull = IsGaugeFull();

        //ゲージがフルかつそのスキルが必殺技なら
        if (gaugeFull && TryStartUltimate())
        {
            return true;
        }



        SkillSO nextSkill = GetNextNormalSkill();

        if (nextSkill == null)
        {
            return false;
        }

        return StartSkill(nextSkill);
    }

    /// <summary>
    /// 実行するスキルを設定し、HoldStateへ遷移する。
    /// </summary>
    /// <param name="skill">実行するスキル</param>
    /// <returns>
    /// true: スキルの開始に成功した
    /// false: スキルが設定されていない
    /// </returns>
    private bool StartSkill(SkillSO skill)
    {
        if (skill == null)
        {
            return false;
        }

        SetPendingSkill(skill);

        ChangeState(HoldState);

        return true;
    }

    /// <summary>
    /// 必殺技の実行を開始する。
    /// 必殺技演出を再生し、実行するスキルを設定する。
    /// </summary>
    /// <returns>
    /// true: 必殺技の開始に成功した
    /// false: 実行可能な必殺技が存在しない
    /// </returns>
    private bool TryStartUltimate()
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
        return StartSkill(ultimate);

    }

    /// <summary>
    /// スキルループから次に実行する通常スキルを取得する。
    /// 必殺技は対象外とする。
    /// </summary>
    /// <returns>
    /// 次に実行する通常スキル。
    /// 取得できない場合は null。
    /// </returns>
    private SkillSO GetNextNormalSkill()
    {
        SkillSO nextSkill = null;
        int safety = 0;


        while (safety < status.SkillLoop.Count)
        {
            nextSkill = status.SkillLoop[skillIndex];
            skillIndex = (skillIndex + 1) % status.SkillLoop.Count;
            safety++;

            //必殺技では無かったらスキルデータを返す
            if (nextSkill.SkillCategory != SkillType.Ultimate)
            {
                return nextSkill;
            }
        }

        return null;
    }

    /// <summary>
    /// Stateを切り替える唯一の窓口。
    /// State自身は直接他Stateを操作せず、
    /// 必ず owner を通じて遷移を要求する。
    /// </summary>
    internal void ChangeState(BattleStateBase nextState)
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

    /// <summary>
    /// 必殺技の実行ロックを解除する。
    /// </summary>
    internal void ResetUltimateLock()
    {
        isPlayingUltimate = false;
    }

    // ===== 以下は State から呼ばれる「能力」 =====

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


    /// <summary>
    /// 対象にダメージを適用する。
    /// 対象の生存確認、攻撃SEの再生、ダメージ計算を行う。
    /// </summary>
    /// <param name="skill">ダメージ計算に使用するスキル情報</param>
    internal void DealDamage(SkillSO skill)
    {
        //ブラックボードにターゲットが設定されていなければ処理しない
        if (Blackboard.Target == null)
        {
            return;
        }

        //与えようとしているターゲットが死んでいなければ
        if (Blackboard.Target.Blackboard.IsDead)
        {
            return;
        }

        //必殺技なら
        if (skill.SkillCategory == SkillType.Ultimate)
        {
            //必殺技時の攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.UBImpact);
        }
        else
        {
            //必殺技以外のスキルの攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.Attack);
        }

        //ダメージ計算関数を呼ぶ
        int damage = CalculateDamage(skill);

        //ターゲットへのダメージ量が決定したのでブラックボードに渡す
        Blackboard.Target.TakeDamage(damage);
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
        int defense = Blackboard.Target.status.PhysicalDefense;

        //// スキル倍率を適用した、理論上の攻撃力を計算
        float scaled = attack * skill.Multiplier;

        // 防御力を差し引いたダメージを整数に丸める。
        // 計算結果がスキルの最低保証ダメージを下回る場合は、最低保証ダメージを返す。
        return Mathf.Max(
          skill.MinimumDamage,
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

    /// <summary>
    /// ダメージ数値を表示する。
    /// </summary>
    /// <param name="damage">表示するダメージ量</param>

    private void PlayDamageText(int damage)
    {
        if (damageTextPool == null) return;

        var text = damageTextPool.Get();
        text.transform.position = transform.position;
        text.Play(damage);
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
                0f,
                0f
            ),
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
                damageEffectSettings.HitStopDuration
            ),
            ignoreTimeScale: true
        );

        //元の時間に戻して正常の時間の進み方にする
        Time.timeScale = 1f;
    }

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

    /// <summary>
    /// 現在のスキルを実行し、必要な後処理を行う。
    /// </summary>
    internal virtual void ExecuteSkill()
    {
        // スキル命中時に増加するTP（必殺技ゲージ）を加算する
        blackboard.AddTP(
            SkillState.GetCurrentSkill()
            .TPGainOnHit
        );
    }

    /// <summary>
    /// 行動情報が設定されているかを返す
    /// </summary>
    /// <returns>
    /// true: 行動情報が設定されている
    /// false: 行動情報が設定されていない
    /// </returns>
    internal bool HasCurrentAction()
    {
        return currentAction != null;
    }

    /// <summary>
    /// 現在の行動情報を設定する
    /// </summary>
    /// <param name="action">設定する行動情報</param>
    internal void SetCurrentAction(BattleAction action)
    {
        currentAction = action;
    }

    /// <summary>
    /// 現在の行動情報をクリアする
    /// </summary>
    internal void ClearCurrentAction()
    {
        currentAction = null;
    }


    /// <summary>
    /// 現在の行動情報に対応するステートを取得する
    /// </summary>
    /// <returns>
    /// 現在の行動情報に対応するステート。
    /// 行動情報が未設定の場合は IdleState。
    /// </returns>
    internal BattleStateBase GetStateForCurrentAction()
    {
        //現在のアクションが何もなかったらIdle状態にして返す
        if (currentAction == null)
        {
            return IdleState;
        }


        //該当のステートを返す
        return currentAction.actionType switch
        {
            ActionType.Hold => HoldState,
            ActionType.Wait => IdleState,
            // Waitは何もしない＝Idle
            _ => IdleState
        };
    }


    /// <summary>
    /// 初期状態で使用する行動情報を生成する
    /// </summary>
    /// <returns>初期状態で使用する行動情報。</returns>
    internal BattleAction GetDefaultAction()
    {
        return new BattleAction
        {
            actionType = ActionType.Hold,
            duration = 10.0f
        };
    }

    /// <summary>
    /// プレイヤー入力を受け取り、手動介入要求を処理する。
    /// </summary>
    /// <param name="command">受信したプレイヤーコマンド</param>

    protected virtual void ReceivePlayerCommand(PlayerCommand command)
    {
        DebugManager.Log($"{name}'s ReceivePlayerCommand called");
        DebugManager.Log(currentState.GetType().Name);
        switch (command)
        {
            case PlayerCommand.Skill:

                //手動介入に挑戦したのでカウントを増やす
                BattleResultData.interventionCount++;


                //ここまで

                // 成功時のみ予約
                if (isInterventionWindowOpen)
                {
                    BattleResultData.successCount++;

                    //手動介入要求を許可する
                    RequestManualSkill();

                    DebugManager.Log("hasManualInterventionRequest = true");
                }
                break;
        }
    }



}