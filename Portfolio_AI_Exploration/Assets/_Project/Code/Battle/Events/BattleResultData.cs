using UnityEngine;

public class BattleResultData
{
    public static ResultType resultType =ResultType.Defeat;
    public static int interventionCount;
    public static int successCount;


    public static void Reset()
    {
        resultType = ResultType.Defeat;
        interventionCount = 0;
        successCount = 0;
        DebugManager.Log($"ResultType Reset: {resultType}");
    }
}
