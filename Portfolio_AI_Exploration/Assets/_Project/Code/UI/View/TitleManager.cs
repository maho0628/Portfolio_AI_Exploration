using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private ExitGame exitGame;

    private void Start()
    {
        exitGame = FindAnyObjectByType<ExitGame>();

        // UIボタン設定
        SetupButtonListeners();

        // InputAction取得＆イベント登録
        InputManager.Instance.Bind(ActionMapType.UI, InputActionType.Click, OnClickAction);

      

        // Audio
        AudioManager.Instance.FadeInBGM();
        AudioManager.Instance.PlayBGMIfNotPlaying(BGMName.Title);
    }

    private void OnDestroy()
    {
        // イベント解除
        InputManager.Instance.Unbind(ActionMapType.UI, InputActionType.Click);

    }

    private void OnClickAction(InputAction.CallbackContext context)
    {
        OnStartButtonClicked();
    }

    private void SetupButtonListeners()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("StartButton not found!");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        else
        {
            Debug.LogError("QuitButton not found!");
        }
    }

    private void OnStartButtonClicked()
    {
        SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);
    }

    private void OnQuitButtonClicked()
    {
        exitGame.ExitingGame();
    }
}