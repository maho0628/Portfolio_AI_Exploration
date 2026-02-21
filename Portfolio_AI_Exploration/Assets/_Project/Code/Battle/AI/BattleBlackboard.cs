using UnityEngine;

public class BattleBlackboard
{
    // 戦闘中の状態のみ保持
    public BattleAI Target { get; private set; }

    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }

    public bool IsDead => CurrentHP <= 0;

    public BattleBlackboard(int maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
        Debug.Log(CurrentHP);
    }

    public void SetTarget(BattleAI target)
    {
        Target = target;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP < 0)
            CurrentHP = 0;
        Debug.Log(CurrentHP);
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }
}
