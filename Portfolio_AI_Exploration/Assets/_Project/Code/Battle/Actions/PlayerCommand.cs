using UnityEngine;
/// <summary>
/// 戦闘中の手動介入で使用するプレイヤーコマンドの種類。
/// </summary>
public enum PlayerCommand
{
    /// <summary>
    /// スキル発動を指示する。
    /// </summary>
    Skill,

    /// <summary>
    /// 何も行わず待機する。
    /// </summary>
    Wait
}