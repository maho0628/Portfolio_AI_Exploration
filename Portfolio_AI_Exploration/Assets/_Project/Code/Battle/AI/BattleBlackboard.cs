using System;
using UnityEngine;

/// <summary>
/// 戦闘中に共有するキャラクターの状態を管理する。
/// HP、TP（必殺技ゲージ）、ターゲット情報を保持し、State間で共有する。
/// </summary>
public class BattleBlackboard
{
    /// <summary>
    /// 現在の攻撃対象。
    /// </summary>
    internal BattleAI Target { get; private set; }

    /// <summary>
    /// 戦闘中に変動する現在HP。
    /// </summary>
    internal int CurrentHP { get; private set; }

    /// <summary>
    /// 戦闘中に変動する現在のスキル命中時に増加するTP（必殺技ゲージ）。
    /// </summary>
    internal int CurrentTP { get; private set; }

    /// <summary>
    /// 戦闘中の最大HP。
    /// </summary>
    internal int MaxHP { get; }

    /// <summary>
    /// 戦闘中の最大のスキル命中時に増加するTP（必殺技ゲージ）の最大値。
    /// </summary>
    internal int MaxTP { get; }

    /// <summary>
    /// キャラクターが死亡しているか。
    /// </summary>
    internal bool IsDead => CurrentHP <= 0;

    /// <summary>
    /// HPが変更されたことをUIに通知。
    /// </summary>
    internal event Action<BattleAI, int, int> OnHPChanged;

    /// <summary>
    /// TPが変更されたことをUIに通知。
    /// </summary>
    internal event Action<int, int> OnTPChanged;


    /// <summary>
    /// ブラックボードの初期設定。
    /// </summary>
    /// <param name="target">このブラックボードを保持するキャラクター</param>
    /// <param name="maxHP">戦闘中の最大HP</param>
    /// <param name="maxTP">戦闘中の最大のスキル命中時に増加するTP（必殺技ゲージ）の最大値</param>
    internal BattleBlackboard(BattleAI target, int maxHP, int maxTP)
    {
        this.Target = target;

        MaxHP = maxHP;
        MaxTP = maxTP;

        CurrentHP = MaxHP;
        CurrentTP = 0;
    }

    /// <summary>
    /// 攻撃対象を更新する。
    /// </summary>
    /// <param name="target">新しい攻撃対象</param>
    internal void SetTarget(BattleAI target)
    {
        Target = target;
    }

    /// <summary>
    /// ダメージを適用し、HPの変更を通知する。
    /// </summary>
    /// <param name="amount">受けるダメージ量</param>
    internal void TakeDamage(int amount)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHP = Mathf.Max(CurrentHP - amount, 0);
        DebugManager.Log($"HP:{CurrentHP}/{MaxHP}");

        OnHPChanged?.Invoke(Target, CurrentHP, MaxHP);

    }

    /// <summary>
    ///スキル命中時に増加するTP（必殺技ゲージ）を加算し、変更を通知する。
    /// </summary>
    /// <param name="amount">加算するTP（必殺技ゲージ）量</param>
    internal void AddTP(int amount)
    {
        if (CurrentTP >= MaxTP)
        {
            return;
        }

        CurrentTP = Mathf.Min(CurrentTP + amount, MaxTP);

        //TPの数値変更を通知
        OnTPChanged?.Invoke(CurrentTP, MaxTP);

    }
    /// <summary>
    /// 必殺技使用時にスキル命中時に増加するTP（必殺技ゲージ）量を初期化し、変更を通知する。
    /// </summary>
    internal void ResetTP()
    {
        CurrentTP = 0;
        //TPの数値変更を通知
        OnTPChanged?.Invoke(CurrentTP, MaxTP);
    }

}