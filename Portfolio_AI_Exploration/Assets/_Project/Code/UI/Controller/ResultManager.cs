using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private ResultDisplayData[] resultDataList;

    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI interventionText;
    [SerializeField] private TextMeshProUGUI successRateText;


    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToTitleButton;


    void Start()
    {
        ResultType type =
            BattleResultData.playerWin ? ResultType.Victory : ResultType.Defeat;

        ResultDisplayData data = GetResultData(type);
        resultText.text = data.resultText;

        int count = BattleResultData.interventionCount;
        int success = BattleResultData.successCount;
        Debug.LogWarning(BattleResultData.successCount);
        float rate = 0f;

        if (count > 0)
        {
            rate = (float)success / count * 100f;
        }
        interventionText.text = $"Interventions : {count}";
        successRateText.text = $"Success Rate : {rate:0}%";
        retryButton.onClick.AddListener(Retry);
        backToTitleButton.onClick.AddListener(BackToTitle);
    }

    ResultDisplayData GetResultData(ResultType type)
    {
        foreach (var data in resultDataList)
        {
            if (data.resultType == type)
                return data;
        }

        return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            BackToTitle();
        }
    }

    public void Retry()
    {
        //Refactor::シーン遷移マネージャーに書き換え
        SceneManager.LoadScene("Exploration_Main");
    }

    public void BackToTitle()
    {
        //Refactor::シーン遷移マネージャーに書き換え

        SceneManager.LoadScene("TitleScene");
    }
}
