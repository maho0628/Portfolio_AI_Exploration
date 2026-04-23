using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("Display Data")]
    [SerializeField] private ResultDisplayData[] resultDataList;

    [Header("Animation Data")]
    [SerializeField] private ResultAnimationData animationData;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI interventionText;
    [SerializeField] private TextMeshProUGUI successRateText;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToTitleButton;

    private CanvasGroup retryCanvasGroup;


    private void Awake()
    {
        retryCanvasGroup = retryButton.GetComponent<CanvasGroup>();
        if (retryCanvasGroup == null)
        {
            retryCanvasGroup = retryButton.gameObject.AddComponent<CanvasGroup>();
        }

       
    }
    void Start()
    {
        ResultDisplayData data = GetResultData(BattleResultData.resultType);


        resultText.text = data.resultText;

        if (data != null)
        {
            resultText.text = data.resultText;
            resultText.color = data.textColor;
        }

        int interventionCount = BattleResultData.interventionCount;
        int successCount = BattleResultData.successCount;

        float successRate = 0f;
        if (interventionCount > 0)
        {
            successRate = (float)successCount / interventionCount * 100f;
        }

        interventionText.text = $"Interventions : {interventionCount}";
        successRateText.text = $"Success Rate : {successRate:0}%";
        retryButton.onClick.AddListener(Retry);
        backToTitleButton.onClick.AddListener(BackToTitle);
        if (BattleResultData.resultType == ResultType.Goal)
        {
            PlayGoalPresentation().Forget();
        }
    }

    ResultDisplayData GetResultData(ResultType type)
    {
        foreach (var data in resultDataList)
        {
            if (data.resultType == type)
                return data;
        }
        Debug.LogWarning($"ResultDisplayData not found : {type}");

        return null;
    }
    private async UniTaskVoid PlayGoalPresentation()
    {
        try
        {
            // Goal時はRetryできない
            retryButton.interactable = false;

            // ちょい間を置いてから消す
            await UniTask.Delay(
                TimeSpan.FromSeconds(animationData.retryFadeDelay)
            );

            Sequence hideRetrySequence = DOTween.Sequence();

            // 少し縮みながら
            hideRetrySequence.Join(
                retryButton.transform
                    .DOScale(
                        animationData.retryShrinkScale,
                        animationData.retryFadeDuration
                    )
                    .SetEase(animationData.retryFadeEase)
            );

            // フェードアウト
            hideRetrySequence.Join(
                retryCanvasGroup.DOFade(
                    0f,
                    animationData.retryFadeDuration
                )
            );

            bool sequenceFinished = false;

            hideRetrySequence.OnComplete(() => sequenceFinished = true);

            await UniTask.WaitUntil(() => sequenceFinished);
            // 完全に消す

            retryButton.interactable = false;
            retryCanvasGroup.blocksRaycasts = false;
            retryCanvasGroup.alpha = 0f;
        }
        catch (OperationCanceledException)
        {
            // シーン遷移時などに破棄されたら無視
        }
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

        if (BattleResultData.resultType == ResultType.Goal)
        {
            return;

        }
        if (BattleResultData.resultType == ResultType.Victory)
        {
            ExplorationData.currentRouteIndex++;
        }
        BattleResultData.resultType = ResultType.Defeat;




        //Refactor::シーン遷移マネージャーに書き換え
        SceneManager.LoadScene("Exploration_Main");
    }


  

    public void BackToTitle()
    {
        ExplorationData.playerPosition = Vector3.zero;

        //Refactor::シーン遷移マネージャーに書き換え

        SceneManager.LoadScene("TitleScene");
    }
}
