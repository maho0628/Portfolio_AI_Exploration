using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private Button startButton;
    private InputAction demoAction;
    [SerializeField]
    private Button quitButton;


    private ExitGame exitGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        demoAction = inputActions.FindAction("Click"); 
        InitializeButtons();
        exitGame =FindAnyObjectByType<ExitGame>();

        AudioManager.Instance.FadeInBGM();

        AudioManager.Instance.PlayBGMIfNotPlaying(BGMName.Title);    
        SetupButtonListeners();

    }

    private void SetupButtonListeners()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("TitleButton not found!");
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
    private void InitializeButtons()
    {
        if (startButton == null)
        {
            startButton = FindButtonByName("TitleButton");
        }
    }

    private Button FindButtonByName(string buttonName)
    {
        GameObject buttonObject = GameObject.Find(buttonName);
        if (buttonObject != null)
        {
            return buttonObject.GetComponent<Button>();
        }

        Debug.LogWarning($"Button '{buttonName}' not found in scene.");
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (demoAction != null && demoAction.IsPressed())
        {
            OnStartButtonClicked();


        }



    }
}
