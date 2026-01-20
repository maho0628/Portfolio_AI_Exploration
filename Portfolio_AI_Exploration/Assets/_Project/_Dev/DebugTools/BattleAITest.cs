using UnityEngine;

/// <summary>
/// 戦闘AIのテスト用実装。
/// BattleAI の動作確認専用。
/// </summary>
public class BattleAITest : BattleAI
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("BattleAI Test Awake");
    }

    // 仮入力用（後でPlayerInputControllerに置き換える）
    public override bool IsGaugeFull()
    {
        return true; // 強制でHoldに行かせる
    }

    public override bool HasSkillInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool HasWaitInput()
    {
        return Input.GetKeyDown(KeyCode.W);
    }
}
