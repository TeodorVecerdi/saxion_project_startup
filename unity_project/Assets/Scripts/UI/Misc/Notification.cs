using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Notification : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private float TransitionDuration = 0.25f;
    [SerializeField] private float HoldDuration = 3.0f;

    private RectTransform rectTransform;

    public void ShowNotification(string text) {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        Text.text = text;
        rectTransform.DOAnchorPosY(0, TransitionDuration).From(new Vector2(0, 512)).OnComplete(() => {
            rectTransform.DOAnchorPosY(512, TransitionDuration).From(Vector2.zero).SetDelay(HoldDuration);
        });
    }
}
