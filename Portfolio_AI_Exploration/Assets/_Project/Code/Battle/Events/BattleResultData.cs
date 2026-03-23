using UnityEngine;

public class BattleResultData
{
    public static bool playerWin;
    public static int interventionCount;
    public static int successCount;


    public static void Reset()
    {
        playerWin = false;
        interventionCount = 0;
        successCount = 0;
    }
}
