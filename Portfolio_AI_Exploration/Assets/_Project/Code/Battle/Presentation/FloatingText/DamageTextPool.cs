using UnityEngine;

/// <summary>
/// ダメージ表示用テキストのオブジェクトプール。
/// DamageTextController の生成と再利用を管理する。
/// </summary>
public class DamageTextPool
    : ObjectPool<DamageTextController>
{

}