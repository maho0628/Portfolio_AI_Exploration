using UnityEngine;

public enum DamageType
{
    Physical,
    Magical,
    Hybrid,          // 両方参照
    LowerDefense,    // 相手の低い防御を使う
    TrueDamage       // 防御無視
}
