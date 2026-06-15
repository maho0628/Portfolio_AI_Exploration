using System;
using UnityEngine;

public class BattleBlackboard
{
    /// <summary>
    ///     ここら辺は全部ブラックボード
    /// </summary>
    public BattleAI Target { get; private set; }

    public int CurrentHP { get; private set; }
    public int CurrentTP { get; private set; }
    public int MaxHP { get; }
    public int MaxTP { get; }

    // =====ここまでブラックボード =====

    //ステート関連をブラックボードが持つべきじゃない
    private BattleAI owner;



    /// <summary>
    /// 計算自体はブラックボードではなくキャラクタークラスが持つべき
    /// あくまでもこっちの変数はコピーする場所（キャッシュだけ）
    /// </summary>
    public bool IsDead => CurrentHP <= 0;

    /// <summary>
    /// この二つは多分ギリブラックボード側かな
    /// </summary>
    public event Action<BattleAI, int, int> OnHPChanged;
    public event Action<int, int> OnTPChanged;

    /// <summary>
    /// 初期設定だからこれはブラックボード側
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="maxHP"></param>
    /// <param name="maxTP"></param>
    public BattleBlackboard(BattleAI owner, int maxHP, int maxTP)
    {
        this.owner = owner;

        MaxHP = maxHP;
        MaxTP = maxTP;

        CurrentHP = MaxHP;
        CurrentTP = 0;
    }

    /// <summary>
    /// これは参考コードだと「キャラ側が持つべきだけどそしたら今キャラ専用のコントローラー＝キャラ専用のAiコントローラーになってるからリファクタリング必要
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(BattleAI target)
    {
        Target = target;
    }

    /// <summary>
    /// ダメージを与える関数はブラックボードが持ったらダメな気がする
    /// バトルシステム専用のクラスが持つべき
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        CurrentHP = Mathf.Max(CurrentHP - amount, 0);
        DebugManager.Log($"HP:{CurrentHP}/{MaxHP}");

        OnHPChanged?.Invoke(owner, CurrentHP, MaxHP);


    }

    /// <summary>
    /// バトルシステム専用のクラスが持つべき（てか回復機能ないから関数自体なくしていい)
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    /// <summary>
    /// バトルシステム専用のクラスが持つべき(TP上昇するための関数）
    /// </summary>
    /// <param name="amount"></param>
    public void AddTP(int amount)
    {
        if (CurrentTP >= MaxTP)
        {
            return;
        }
        CurrentTP = Mathf.Min(CurrentTP + amount, MaxTP);
      


        //TPの数値変更を通知
        OnTPChanged?.Invoke(CurrentTP, MaxTP);
        }


    /// UB使用時にTPを初期化するための関数（ バトルシステム専用のクラスが持つべき）
    public void ResetTP()
    {
        CurrentTP = 0;
        //TPの数値変更を通知
        OnTPChanged?.Invoke(CurrentTP, MaxTP);
    }
}