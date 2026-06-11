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
    /// <summary>
    /// 各キャラクターのステータス（どのスキルを持っているかまたそのスキルループの順番もここで決定
    /// </summary>
    [SerializeField]
    protected CharacterStatusSO status;

    /// <summary>
    /// CharacterStatusSO内のスキルループを配列からリストにしてる？まあ実質バトルAIで使えるようにするがこの変数の意味敵に正しそう
    /// </summary>
    private List<SkillSO> skills = new List<SkillSO>();

    /// <summary>
    /// 被弾エフェクトのスクリプタブルオブジェクト
    /// シェイクしたりヒットストップしたり
    /// </summary>
    [SerializeField]
    protected DamageEffectSettingsSO damageEffectSettings;

    /// <summary>
    /// 被弾したときのHP減少をオブジェクトプールで表示するための変数
    /// </summary>
    [SerializeField]
    private DamageTextPool damageTextPool;

    /// <summary>
    /// UBの演出とかを呼び出すために自作クラス型の変数を宣言してる
    /// </summary>
    [SerializeField]
    private UltimatePresentationController ultimatePresentation;

    /// <summary>
    /// 最低限何ダメージを与えることができるか（そもそもこれさSkillSOが持つべきじゃね？
    /// </summary>

    [SerializeField]

    int MinimumDamage = 1;

    /// <summary>
    /// 現在どのステートなのか（Idle～アタックまで切り替わりまくる場所
    /// </summary>
    protected BattleStateBase currentState;

    /// <summary>
    /// 各キャラクターのブラックボード
    /// ここで内部的に現在のHPやTPの増減を管理？
    /// </summary>
    protected BattleBlackboard blackboard;

    /// <summary>
    ///ブラックボードを外部で参照できるようにするための変数（上の変数が、private変数でいいやん）
    /// </summary>
    public BattleBlackboard Blackboard => blackboard;

    /// <summary>
    /// 今どのスキルなのかをリストのIndexで判断するため
    /// </summary>
    private int skillIndex = 0;


    // Stateインスタンス
    protected IdleState idleState;
    protected HoldState holdState;
    protected AttackState attackState;
    protected SkillState skillState;

    //行動キュー　のちに行動ループを自由に変更できるようにするための下準備　
    protected BattleAction currentAction;

    /// <summary>
    /// これなもともとは上にも書いてるけどどの順番でステートが変わるかも変更可能にしようとしてて、そのためにインスペクターで確認できるようにの意図で作ったけど今ならいらない節あるんよな（結局できてないし）
    /// 
    /// </summary>
    public BattleAction CurrentAction => currentAction;

    // 外部参照用（Stateから使う）また上の変数、privateじゃないやん
    public IdleState IdleState => idleState;
    public HoldState HoldState => holdState;
    public AttackState AttackState => attackState;
    public SkillState SkillState => skillState;

    /// <summary>
    /// この変数なんだ？多分死亡時にAIが動いてるかどうかを判定かな？（バトルマネージャーで参照しとるし
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// AIそのものの初期化？
    /// </summary>
    protected bool isInitialized = false;

    // 介入受付中か

    private bool canIntervention;
    /// skill入力バッファフラグ
    /// </summary>
    private bool manualSkillRequested;

    /// <summary>
    /// 必殺技が起動中かどうか
    /// </summary>
    private bool isPlayingUltimate;

    /// <summary>
    /// AIを持ってるオブジェクトが死んだかどうか
    /// </summary>
    private bool isDead;

    protected virtual void Awake()
    {
        // Stateは一度だけ生成（使い捨てしない）
        idleState = new IdleState(this);
        holdState = new HoldState(this);
        attackState = new AttackState(this);
        skillState = new SkillState(this);

        //現在のステートをIdleに決定？その後ホールドに遷移するよね？？？変じゃない？
        //TODO: Idle代入は不要では？
        currentState = idleState;

        currentState = holdState;
        //初期設定を呼び出し
        Initialize();

        //スキルループを配列からリストに？（そもそもスキルループ側リストで宣言すればいいやん

        skills = status.SkillLoop.ToList();


        //デバッグログ一旦は残すけど後で削除すること

        //Debug.Log($"HP:{blackboard.CurrentHP} TP:{blackboard.CurrentTP}");

        //foreach (var skill in skills)
        //{
        //    Debug.Log($"Skill:{skill.name} Type:{skill.skillType}");
        //}
    }

    public void Initialize()
    {
        //ブラックボードの初期化
        blackboard = new BattleBlackboard(this, status.MaxHP, status.TPMax);

        //デバッグログ一旦は残すけど後で削除すること

        //Debug.LogWarning(status.MaxHP);
        //Debug.LogWarning(status.TPMax);

        //死亡していない＋初期化完了に設定
        isDead = false;
        isInitialized = true;
        // TODO:
        // 本来Idle開始のはずだがHoldStateへ上書きしている
        // 意図確認

        //要検討
        currentAction = new BattleAction
        {
            actionType = ActionType.Hold,
            duration = 0.5f
        };
    }

    protected virtual void Update()
    {
        //初期化されていなければ終了
        if (!isInitialized)
        {
            return;
        }

        //死亡時にAIが動いていないなら終了（ただこの変数いるのか問題は要検討
        if (!isActive)
        {
            return;

        }


        // これさそもそも論isDeadをばとるAI 内部で持つ必要なくね？ブラックボードののIsDead参照でよくないか？
        if (!isDead && blackboard.IsDead)
        {
            isDead = true;
            //死亡死亡後の処理呼び出してるけど内部はただのデバッグログだし、そもそもバトルマネージャー側でも死亡後の処理書いてた気が
            OnDeath();
            return;
        }

        //すでに死亡していたら処理しない（isDeadの責務問題以下略）
        if (isDead)
        {
            return;
        }

        //Tickを進めるだけど処理順番ここか？
        currentState.Tick();
    }

    /// <summary>
    /// 手動介入要求が行われた時に外部で呼ばれる関数（てか外部ですら今呼ばれてないやん、ほかの関数との関係次第で判断かな
    /// </summary>
    public void RequestManualSkill()
    {
        manualSkillRequested = true;
    }

    /// <summary>
    /// 手動介入要求を消費したかどうかを判断して真偽値として返す（そもそもこの書き方でいいのかすごい疑問If,elseにするとかそもそもRequestManualSkillこれを使わずに別々で管理してるところとか,引数持たせてないところとか本当にこれでいいのか？
    /// </summary>
    /// <returns></returns>
    public bool ConsumeManualSkillRequest()
    {
        if (!manualSkillRequested)
        {
            return false;
        }

        manualSkillRequested = false;
        return true;
    }

    /// <summary>
    /// 手動介入できるかどうか　これさそもそも手動介入の要求が送られてきて、その条件とスキルのタイミング的に猶予があればOKとみなすようにしないといけないけど実際にスキルのタイミング的に猶予があればの判断してるのが、skillState側かつ介入できるかどうかのフラグ切り替えしかここはしていないから本当にそれでいいのか？
    /// </summary>
    /// <param name="value"></param>
    public void SetInterventionWindow(bool value)
    {
        canIntervention = value;
    }
    /// <summary>
    /// 一番の問題児　これは一旦放置
    /// </summary>
    /// <returns></returns>
    public bool TryDecideSkill()
    {
        //UB再生中ならskill決定しない
        if (isPlayingUltimate)
        {
            return false;
        }

        //デバッグログ一旦は残すけど後で削除すること
        // Debug.Log($"{name} TryDecideSkill");

        //// SkillState実行中は再度スキル決定しないためのガード
        // ただし「スキル決定」自体をBattleAIが持つ設計が適切かは要検討

        if (currentState == SkillState)
        {
            return false;
        }

        //まずUB条件だけを最優先で確定させる
        // ---- ここから ----てかなんでIFelseでUBかどうか判定してないんだろう？→関数切り出しでこの違和感が消える可能性大
        // manual入力取得しているが現在未使用
        //// 以前はUB発動条件に含める予定だった可能性あり
        ///// HoldState経由の介入仕様へ移行予定のため保留。
        //bool hasManualInput =
        //ConsumeManualSkillRequest();
        // 現在のUB発動条件はTP満タンのみ
        // 手動介入成功との関係は要確認
        //ゲージフルかどうか確認
        bool gaugeFull = IsGaugeFull();

        if (gaugeFull && TryStartUltimate())
        {
            return true;
        }
        //デバッグログ一旦は残すけど後で削除すること

        //Debug.Log($"[UB CHECK] TP:{Blackboard.CurrentTP}/{Blackboard.MaxTP} manual:{hasManualInput} full:{gaugeFull}");

       

        // ---- ここまでUB ----

        // ---- ここから下は通常スキルだけ ----
        SkillSO nextSkill = GetNextNormalSkill();

        Debug.Log(nextSkill);
        //int safety = 0;




        //スキルの中身が何もないなら決まってないとみなしてFalseを返す
        if (nextSkill == null)
        {
            return false;
        }

        return StartSkill(nextSkill);
    }

    private bool StartSkill(SkillSO skill)
    {
        if (skill == null)
        {
            return false;
        }

        SkillState.SetSkill(skill);
        ChangeState(SkillState);

        return true;
    }
    private bool TryStartUltimate()
    {
        var ultimate =
            skills.FirstOrDefault(s => s.skillType == SkillType.Ultimate);

        if (ultimate == null)
        {
            return false;
        }

        isPlayingUltimate = true;

        ultimatePresentation.Play().Forget();

        return StartSkill(ultimate);
        
    }
    /// <summary>
    /// 通常スキル

    /// </summary>
    /// <returns></returns>
    private SkillSO GetNextNormalSkill()
    {
        SkillSO nextSkill = null;
        int safety = 0;

        while (safety < skills.Count)
        {
            nextSkill = skills[skillIndex];
            skillIndex = (skillIndex + 1) % skills.Count;
            safety++;

            if (nextSkill.skillType != SkillType.Ultimate)
            {
                return nextSkill;
            }
        }

        return null;
    }
    /// <summary>
    /// ここは内容込みで合ってそう。ただChangeStateを呼ぶタイミングは要検討
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

    /// <summary>
    /// 必殺技を2度連続で打てなくなるための関数でもこれをAiがそもそも持つべきじゃない気がする
    /// </summary>
    public void ResetUltimateLock()
    {
        isPlayingUltimate = false;
    }

    // ===== 以下は State から呼ばれる「能力」 =====

    /// <summary>
    /// TPのゲージがマックスかどうかを判定ギリAIが管理するべきかな
    /// とはいえ内部の計算処理はブラックボードかキャラがもってそのフラグをこっちで参照する形がいい
    /// </summary>
    /// <returns></returns>
    public virtual bool IsGaugeFull()
    {
        return blackboard.CurrentTP >= status.TPMax;
    }

    /// <summary>
    /// 各AIのターゲットを設定するAI側にあるべきか？てかそもそもこの関数をブラックボードが持つ必要あるのか？
    /// キャラのコントローラーが持つべき
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(BattleAI target)
    {
        blackboard.SetTarget(target);
    }

    /// <summary>
    /// ダメージを与えるための関数これはバトルシステムクラスが持つべき（AIはだめ）
    /// </summary>
    /// <param name="skill"></param>
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

        //そもそもここから下のSE流す場所本当にここか？


        //必殺技なら
        if (skill.skillType == SkillType.Ultimate)
        {
            //必殺技時の攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.UBImpact);
        }
        else
        {
            //通常Skill時の攻撃SEを流す
            AudioManager.Instance.PlaySEById(SEName.Attack);
        }

        //各キャラの攻撃力を参照する
        int attack = status.PhysicalAttack;

        //与えるターゲットの防御力を参照する
        int defense = Blackboard.Target.status.PhysicalDefense;

        //実際に与えることのできる力の最大：攻撃力＊スキルの倍率＊１００（マジックナンバーだし、何で？）

        int scaled = attack * skill.power / 100;

        //Refactor: ここシリアライズフィールドで書いてるけど将来データ駆動

        //実際に与えることのできる力の最大ー防御力で実際に与えれるダメージを計算し、最小値より低かったらMinimumDamageを与える
        int damage = Mathf.Max(MinimumDamage, scaled - defense);

        Debug.Log($"{name} deals {damage} damage");

        //ターゲットへのダメージ量が決定したのでブラックボードに渡す
        Blackboard.Target.TakeDamage(damage);
    }

    /// <summary>
    /// ダメージを与える（ここはブラックボード関連が変わるからそれに伴って修正
    /// </summary>
    /// <param name="amount"></param>

    internal virtual void TakeDamage(int amount)
    {
        blackboard.TakeDamage(amount);

        //ここからは本来な演出系のクラスがもってそこからの関数を参照込みで呼ぶべき
        PlayDamageText(amount);
        PlayDamageReaction();
        //ここまで
    }

    /// <summary>
    /// AIがもつべきではない（けどアニメーションのマネージャーから関数は呼んでるのか、うーん
    /// ダメージをエフェクトとして出す関数
    /// </summary>
    /// <param name="damage"></param>

    private void PlayDamageText(int damage)
    {
        if (damageTextPool == null) return;

        var text = damageTextPool.Get();
        text.transform.position = transform.position;
        text.Play(damage);
    }

    /// <summary>
    /// AIがもつべきではない
    /// ヒットストップ関数呼びだしたり、画面シェイクしたりする関数
    /// </summary>
    internal virtual void PlayDamageReaction()
    {
        //画面シェイクする
        transform.DOShakePosition(
            damageEffectSettings.shakeDuration,
            new Vector3(
                damageEffectSettings.shakeStrength,
                0f,
                0f
            ),
            damageEffectSettings.shakeVibrato
        );

        //ヒットストップ関数を呼ぶ
        PlayHitStop().Forget();
    }


    /// <summary>
    /// ヒットストップをするための関数
    /// AIがもつべきではない
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid PlayHitStop()
    {
        //タイムスケールをヒットストップ用の時間に設定する
        Time.timeScale = damageEffectSettings.hitStopScale;

        //タイムスケールを無視した状態でヒットストップ用の待ち時間分待つ
        await UniTask.Delay(
            System.TimeSpan.FromSeconds(
                damageEffectSettings.hitStopDuration
            ),
            ignoreTimeScale: true
        );

        //元の時間に戻して正常の時間の進み方にする
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 派生クラスでSE（音源違いをキャラ事に鳴らすので現在必要だが、要検討箇所でもある
    /// </summary>
    protected virtual void OnDeath()
    {
        Debug.Log($"{name} is dead.");
        Debug.Log($"IsDead{blackboard.IsDead}");
        Debug.Log($"OnDeath called by {this}");

    }



    /// <summary>
    /// 現在はアクティベートAIをバトルマネージャーから呼んでいるが要検討
    /// AI稼働開始するよう関数（でも停止してなさそう？
    /// </summary>
    public void ActivateAI()
    {
        isActive = true;
    }

    /// <summary>
    /// スキル実行（Virtual消してもいいかもな）この関数を呼ぶ場所は要検討
    /// </summary>
    public virtual void ExecuteSkill()
    {
        //TPを各skillごとの手に入れれるTP分加算（うーん個々の書き方はスキルステートとともに要検討かな
        blackboard.AddTP(
            SkillState.GetCurrentSkill()
            .tpGainOnHit
        );
    }

    /// <summary>
    /// スキルループ巡回用の現在のアクションを取得するための関数、今のところはそのままでもいいけど消したほうがよさそうなのは事実本来の役割果たしてないから（けどバグって時間かかるよりかはマシか？）
    /// </summary>
    /// <returns></returns>
    public bool HasCurrentAction()
    {
        return currentAction != null;
    }

    /// <summary>
    /// スキルループ巡回用の現在のアクションを変更
    /// </summary>
    /// <param name="action"></param>
    public void SetCurrentAction(BattleAction action)
    {
        currentAction = action;
    }

    /// <summary>
    /// スキルループ巡回用の現在のアクションをなくす
    /// </summary>
    public void ClearCurrentAction()
    {
        currentAction = null;
    }



    /// <summary>
    ///アクションに対応したステートを返す関数
    ///ではあるけどやっぱりわざわざEnumをステート変換か
    /// </summary>
    /// <returns></returns>
    public BattleStateBase GetStateForCurrentAction()
    {
        //現在のアクションが何もなかったらIdle状態にして返す
        if (currentAction == null)
        {
            return IdleState;
        }

        //デバッグログ一旦は残すけど後で削除すること
        //Debug.Log($"[AI] Action:{currentAction?.actionType}");

        //該当のステートを返す
        return currentAction.actionType switch
        {
            ActionType.Attack => AttackState,
            ActionType.Hold => HoldState,
            ActionType.Wait => IdleState,
            // Waitは何もしない＝IdleでOK
            _ => IdleState
        };
    }


    /// <summary>
    /// 初期アクションにする
    /// ギリAI側かな
    /// わざわざニューするのか少なくてもアイドルステートで呼ぶ必要あるか？
    /// </summary>
    /// <returns></returns>
    public virtual BattleAction GetDefaultAction()
    {
        return new BattleAction
        {
            actionType = ActionType.Hold,
            duration = 10.0f
        };
    }

    /// <summary>
    /// 問題児２そもそも入力関連はAIなのか？
    /// インプットマネージャーから入力された信号を受け取って成功したら手動介入できるとみなして要求を呼ぶ関数
    /// </summary>
    /// <param name="command"></param>

    public virtual void ReceivePlayerCommand(PlayerCommand command)
    {
        Debug.Log($"{name}'s ReceivePlayerCommand called");
        Debug.Log(currentState.GetType().Name);
        switch (command)
        {
            case PlayerCommand.Skill:

                //ここから余計に加算してる？
                //BattleResultData.interventionCount++;

                //if (canIntervention)
                //{
                //    BattleResultData.successCount++;
                //}
                //ここまで

                // 成功時のみ予約
                if (canIntervention)
                {
                    BattleResultData.successCount++;

                    //手動介入要求を許可する
                    RequestManualSkill();

                    Debug.Log("manualSkillRequested = true");
                }
                break;
        }
    }



}