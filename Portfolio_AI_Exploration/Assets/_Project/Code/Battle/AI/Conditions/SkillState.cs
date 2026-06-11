using UnityEngine;

/// <summary>
/// スキルステート
/// 結局スキルの確定をAIがしてるのが問題かも
/// ここでUBかスキル１、２の実行をしたいところ
/// </summary>
public class SkillState : BattleStateBase
{
    /// <summary>
    /// 現在のskill
    /// </summary>
    //TODO:　実行するかどうかのフラグいらないかどうかの判断　いる場合はTickのIdle処理の条件を見直す
    private SkillSO currentSkill;

    /// <summary>
    /// なぜ仮実装のまま動いてるのよ
    /// タイマーでIdleステートへ遷移か怪しいけど要検討かな
    /// スキル実行もこのタイマーを参照
    /// </summary>
    private float timer;

    /// <summary>
    /// スキル実行中かどうかをフラグで管理
    /// </summary>
    private bool executed;

    /// <summary>
    /// 最初にSkillStateのインスタンスを用意するため
    /// ならパブリックじゃないかな（Internalでいいはず
    /// </summary>
    /// <param name="owner"></param>
    public SkillState(BattleAI owner) : base(owner) { }

    /// <summary>
    /// 実行するスキルを設定する
    /// </summary>
    /// <param name="skill"></param>
    public void SetSkill(SkillSO skill)
    {
        currentSkill = skill;
    }

    /// <summary>
    /// 現在どのスキルを持ってるのかを取得するための関数
    /// </summary>
    /// <returns></returns>
    public SkillSO GetCurrentSkill()
    {
        return currentSkill;
    }

    /// <summary>
    /// ステートに入った直後
    /// 一回スキル実行中じゃなくしてから処理開始
    /// UBだったらTPを０にする
    /// </summary>
    public override void OnEnter()
    {
        //スキル実行中を解除
        executed = false;

        //タイマーを元に戻す
        timer = 0f;

        //UBならTPを０にしてTickが続きの処理を行う
        if (currentSkill.skillType == SkillType.Ultimate)
        {
            owner.Blackboard.ResetTP();
        }

        //デバッグログ一旦は残すけど後で削除すること

        //Debug.Log($"Skill Start: {currentSkill.skillType}");
        //Debug.Log($"{owner.name} Skill Start");

    }

    /// <summary>
    /// スキルステートのTick
    /// ② 介入受付時間計算をしてスキルが起動できそうなら実行、ダメージを与える
    /// スキルが実行され終わっていた場合はUB再生中のフラグを元に戻してIdleに遷移
    /// </summary>
    public override void Tick()
    {

        //キャラ自体が死亡していれば処理をさせない
        if (owner.Blackboard.IsDead)
        {
            return;
        }

        //① タイマー加算
        timer += Time.deltaTime;

        //この計算式関数でまとめれそうかな（とはいえ変数何してるか忘れた→もともとの仕様ではこの処理いらないので削除を検討予定
        float hitTiming =currentSkill.duration * currentSkill.InterventionTiming;

        float window =currentSkill.InterventionWindow;

        bool inWindow =   timer >= hitTiming - window &&    timer <= hitTiming + window;

        //ここまで
        owner.SetInterventionWindow(inWindow);
        //なぜ0.5マジックナンバーすぎるでしょ
        //スキル実行中ではなくてかつスキル手動介入実行時間をタイマーがオーバーしたら
        // 途中で攻撃発生
        if (!executed && timer >= currentSkill.duration * 0.5f)
        {
            //スキル実行中フラグをオンにする
            executed = true;

            //スキルを実行（TPを加算するだけかダメージを与える関数とこれうまいこと関数でまとめれそう）
            owner.ExecuteSkill();

            //ダメージを与える
            owner.DealDamage(currentSkill);
        }

        // スキルが実行され終わっているはず
        if (timer >= currentSkill.duration)
        {
            //UBをもう一度打てるようにフラグをリセット
            owner.ResetUltimateLock();

            //Idleに戻す
            owner.ChangeState(owner.IdleState);
        }
    }
}