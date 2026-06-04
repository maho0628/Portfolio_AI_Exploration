using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonFeedback : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private UIButtonFeedbackData data;

    private Transform t;
    private Button button;

    private void Awake()
    {
        t = transform;
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);

    }

    private void OnClick()
    {
        t.DOKill();
        t.DOScale(data.clickScale, data.clickDuration)
         .OnComplete(() => t.DOScale(data.normalScale, data.hoverDuration));
        AudioManager.Instance.PlaySEById(SEName.ButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        t.DOKill();
        t.DOScale(data.hoverScale, data.hoverDuration);
        AudioManager.Instance.PlaySEById(SEName.ButtonHover);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t.DOKill();
        t.DOScale(data.normalScale, data.hoverDuration);
    }
}