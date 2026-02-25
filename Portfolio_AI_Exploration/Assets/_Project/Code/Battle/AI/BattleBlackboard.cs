using UnityEngine;

public class BattleBlackboard
{
    public BattleAI Target { get; private set; }

    public int CurrentHP { get; private set; }
    public int CurrentTP { get; private set; }

    public int MaxHP { get; }
    public int MaxTP { get; }

    public bool IsDead => CurrentHP <= 0;

    public BattleBlackboard(int maxHP, int maxTP)
    {
        MaxHP = maxHP;
        MaxTP = maxTP;

        CurrentHP = MaxHP;
        CurrentTP = 0;
    }

    public void SetTarget(BattleAI target)
    {
        Target = target;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP = Mathf.Max(CurrentHP - amount, 0);
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public void AddTP(int amount)
    {
        if (CurrentTP >= MaxTP) return;
        CurrentTP = Mathf.Min(CurrentTP + amount, MaxTP);
        Debug.Log($"skills:{CurrentTP}");
    }

    public void ConsumeTP(int amount)
    {
        CurrentTP = Mathf.Max(CurrentTP - amount, 0);
    }

    public void ResetTP()
    {
        CurrentTP = 0;
    }
}