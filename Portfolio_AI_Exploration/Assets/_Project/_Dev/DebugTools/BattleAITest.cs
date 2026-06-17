using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーキャラクター用の BattleAI。
/// プレイヤー入力による手動介入を受け付ける。
/// </summary>
public class BattleAITest : BattleAI
{
    /// <summary>
    /// TP（必殺技ゲージ）増加量を表示するテキストプール。
    /// </summary>

    [SerializeField, Tooltip(" TP（必殺技ゲージ）増加量を表示するテキストプール。")]
    private TPTextPool tpTextPool;



    private void Start()
    {
        //必殺技の入力キーをセット
        InputManager.Instance.Bind(
            ActionMapType.Battle,
            InputActionType.Ultimate,
            OnUltimatePerformed
        );
    }


    private void OnDestroy()
    {
        InputManager.Instance.Unbind(
            ActionMapType.Battle,
            InputActionType.Ultimate
        );
    }

    /// <summary>
    /// 必殺技入力を受け取り、手動介入要求を送信する。
    /// </summary>
    /// <param name="_"></param>
    private void OnUltimatePerformed(InputAction.CallbackContext _)
    {
        ReceivePlayerCommand(PlayerCommand.Skill);
    }

    /// <summary>
    /// 現在のスキルを実行し、TP（必殺技ゲージ）を加算する。
    /// この派生クラスでは、TP増加量の表示演出を追加で行う。
    /// </summary>
    internal override void ExecuteSkill()
    {
        base.ExecuteSkill();

        int tpGain =SkillState.GetCurrentSkill() .TPGainOnHit;

        ShowTPGain(tpGain);
    }

    /// <summary>
    /// TP（必殺技ゲージ）の増加量を表示する。
    /// </summary>
    /// <param name="amount">表示するTP（必殺技ゲージ）増加量</param>
    private void ShowTPGain(int amount)
    {

        if (tpTextPool == null)
        {
            return;
        }

        var text = tpTextPool.Get();

        text.transform.position = transform.position;

        text.TPTextPlay(amount);
    }


    /// <summary>
    /// ダメージを受け、HPを減少させる。
    /// 被弾演出とダメージ表示も実行する。
    /// この派生クラスでは追加でプレイヤー被弾時のSEを鳴らす
    /// </summary>
    /// <param name="amount">受けるダメージ量</param>
    protected override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.PlaySEById(SEName.PlayerHit);

    }

    /// <summary>
    /// 死亡時の演出を行うための関数
    /// プレイヤー死亡時のSEを鳴らす
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.Instance.PlaySEById(SEName.PlayerDeath);

    }

}