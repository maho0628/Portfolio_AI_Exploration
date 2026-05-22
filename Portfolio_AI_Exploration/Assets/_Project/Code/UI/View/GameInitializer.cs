using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : SingletonMonoBehaviour<GameInitializer>
{
    private BGMConfigTable bgmConfigTable;

    private SEConfigTable seConfigTable;
    private GameSettings gameSettings;

    private bool isInitialized = false;
    internal bool Initialized => isInitialized;
    internal GameSettings GetGameSettings() { return gameSettings; }

    internal void SetUpGameInitialize()
    {
        if (isInitialized)return;

        // 既存のリソースロード

        gameSettings = Resources.Load<GameSettings>("ScriptableObject/gameSettings");


        bgmConfigTable = gameSettings.BgmConfigTable;

        seConfigTable = gameSettings.SEConfigTable;


        // AudioManagerを強制的に先に生成
        var audio = AudioManager.Instance;
        // 設定テーブルを渡す
        audio.SetupBGMConfigTable(bgmConfigTable);

        audio.SetupSEConfigTable(seConfigTable);
        
        
       


        isInitialized = true;
    }
}