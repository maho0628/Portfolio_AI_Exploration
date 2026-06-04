using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private CanvasGroup interventionCanvasGroup;
    private CanvasGroup successRateCanvasGroup;

    [SerializeField] private SceneMap sceneMap;

    private void Awake()
    {
        retryCanvasGroup = GetOrAddCanvasGroup(retryButton.gameObject);
        interventionCanvasGroup = GetOrAddCanvasGroup(interventionText.gameObject);
        successRateCanvasGroup = GetOrAddCanvasGroup(successRateText.gameObject);


    }

    async void Start()
    {
        SetupResultData();
        SetupTexts();

        retryButton.onClick.AddListener(Retry);
        backToTitleButton.onClick.AddListener(BackToTitle);
        // BGM決定
        if (BattleResultData.resultType == ResultType.Goal)
        {
            AudioManager.Instance.ForcePlayBGM(BGMName.Goal);
        }
        else
        {
            AudioManager.Instance.PlayBGMIfNotPlaying(BGMName.Result);
        }

        // FadeIn
        AudioManager.Instance.FadeInBGM();

        // 演出
        await PlayIntroAnimation();

        if (BattleResultData.resultType == ResultType.Goal)
        {
            await PlayGoalPresentation();
        }
    }

    // =========================
    // INTRO
    // =========================
    private async UniTask PlayIntroAnimation()
    {
        try
        {
            // -----------------
            // RESULT
            // -----------------
            resultText.alpha = 0;
            resultText.transform.localScale = Vector3.one * animationData.resultScaleNormal;

            Sequence resultSeq = DOTween.Sequence();

            resultSeq.Append(
                resultText.DOFade(1f, animationData.resultFadeDuration)
            );

            resultSeq.Join(
                resultText.transform.DOScale(animationData.resultScaleUp, animationData.resultFadeDuration)
                    .SetEase(animationData.resultEase)
            );

            resultSeq.Append(
                resultText.transform.DOScale(animationData.resultScaleBack, animationData.resultFadeDuration)
            );

            resultSeq.Append(
                resultText.transform.DOScale(animationData.resultScaleNormal, animationData.resultFadeDuration)
            );
            AudioManager.Instance.PlaySEById(SEName.ResultAppear);
            await TweenUtil.Play(resultSeq);

            await UniTask.Delay(
                TimeSpan.FromSeconds(animationData.betweenDelayLong)
            );

            TextMeshProUGUI text = retryButton.GetComponentInChildren<TextMeshProUGUI>();

            if (BattleResultData.resultType == ResultType.Victory)
            {
                text.text = resultDataList[(int)ResultType.Victory].resultButtonText;

            }
            else
            {
                text.text = resultDataList[(int)ResultType.Defeat].resultButtonText;

            }
            // -----------------
            // SUCCESS RATE
            // -----------------
            successRateCanvasGroup.alpha = 0;
            successRateText.transform.localPosition += new Vector3(0, -animationData.successMoveOffsetY, 0);

            await TweenUtil.Play(
                successRateCanvasGroup.DOFade(1f, animationData.successFadeDuration)
            );

            await TweenUtil.Play(
                successRateText.transform
                    .DOLocalMoveY(
                        successRateText.transform.localPosition.y + animationData.successMoveOffsetY,
                        animationData.successFadeDuration
                    )
            );

            await UniTask.Delay(TimeSpan.FromSeconds(animationData.betweenDelayMid));

            await TweenUtil.Play(
                successRateText.transform.DOScale(animationData.successScaleUp, animationData.successScaleDuration)
            );

            await TweenUtil.Play(
                successRateText.transform.DOScale(1f, animationData.successScaleDuration)
            );

            await UniTask.Delay(TimeSpan.FromSeconds(animationData.betweenDelayShort));

            // -----------------
            // INTERVENTION
            // -----------------
            interventionCanvasGroup.alpha = 0;
            interventionText.transform.localPosition += new Vector3(0, -animationData.interventionMoveOffsetY, 0);

            await TweenUtil.Play(
                interventionCanvasGroup.DOFade(1f, animationData.interventionFadeDuration)
            );

            await TweenUtil.Play(
                interventionText.transform
                    .DOLocalMoveY(
                        interventionText.transform.localPosition.y + animationData.interventionMoveOffsetY,
                        animationData.interventionFadeDuration
                    )
            );
        }
        catch (OperationCanceledException) { }
    }

    // =========================
    // SETUP
    // =========================
    private void SetupResultData()
    {
        var data = GetResultData(BattleResultData.resultType);
        if (data == null) return;

        resultText.text = data.resultText;
        resultText.color = data.textColor;
    }

    private void SetupTexts()
    {
        int interventionCount = BattleResultData.interventionCount;
        int successCount = BattleResultData.successCount;

        Debug.Log(successCount);
        float successRate = interventionCount > 0
            ? (float)successCount / interventionCount * 100f
            : 0f;

        interventionText.text = $"Interventions : {interventionCount}";
        successRateText.text = $"Success Rate : {successRate:0}%";

        interventionCanvasGroup.alpha = 0;
        successRateCanvasGroup.alpha = 0;
    }

    // =========================
    // DATA
    // =========================
    private ResultDisplayData GetResultData(ResultType type)
    {
        foreach (var data in resultDataList)
            if (data.resultType == type)
                return data;

        Debug.LogWarning($"ResultDisplayData not found : {type}");
        return null;
    }

    // =========================
    // GOAL
    // =========================
    private async UniTask PlayGoalPresentation()
    {
        try
        {
            retryButton.interactable = false;

            await UniTask.Delay(TimeSpan.FromSeconds(animationData.retryFadeDelay));

            Sequence seq = DOTween.Sequence();
            seq.Join(retryButton.transform.DOScale(animationData.retryShrinkScale, animationData.retryFadeDuration));
            seq.Join(retryCanvasGroup.DOFade(0f, animationData.retryFadeDuration));

            await TweenUtil.Play(seq);

            retryCanvasGroup.alpha = 0f;
            retryCanvasGroup.blocksRaycasts = false;
        }
        catch (OperationCanceledException) { }
    }

    // =========================
    // INPUT
    // =========================
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Retry();
        if (Input.GetKeyDown(KeyCode.T)) BackToTitle();
    }

    public void Retry()
    {
        if (BattleResultData.resultType == ResultType.Goal)
            return;

        if (BattleResultData.resultType == ResultType.Victory)
        {
            ExplorationData.currentRouteIndex++;
        }

        BattleResultData.resultType = ResultType.Defeat;

        SceneTransitionManager.Instance.TransitionTo(
            sceneMap.Get(SceneMap.SceneKey.Exploration)
        );
    }

    public void BackToTitle()
    {
        ExplorationData.playerPosition = Vector3.zero;

        SceneTransitionManager.Instance.TransitionToNextScene(FadeMode.SimpleColor);
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        var cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        return cg;
    }
}