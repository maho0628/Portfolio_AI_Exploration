using System.Diagnostics;
using Debug = UnityEngine.Debug;
public static class DebugManager
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(string message)
    {
        Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message)
    {
        Debug.LogError(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }
}
