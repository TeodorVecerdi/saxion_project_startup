using DG.Tweening;
using UnityEngine;

public class MinigameMenuController : MonoBehaviour {
    [SerializeField] private RectTransform MenuTransform;
    [SerializeField] private CanvasGroup PendingState;
    [SerializeField] private CanvasGroup WaitingState;
    [SerializeField] private CanvasGroup StartState;

    private CanvasGroup currentState;
    private bool isOpen = false;

    public void Toggle() {
        MenuTransform.DOAnchorPosX(isOpen ? 950 : 0, 0.25f).From(new Vector2(isOpen ? 0 : 950, 160));
        isOpen = !isOpen;
    }

    public void Open() {
        MenuTransform.DOAnchorPosX( 0, 0.25f).From(new Vector2(950, 160));
        isOpen = true;
    }

    public void Close() {
        MenuTransform.DOAnchorPosX( 950, 0.25f).From(new Vector2(0, 160));
        isOpen = false;
    }

    public void ShowState(int state) {
        // if(!isOpen) Open();
        var nextState 
            = state switch {
            0 => PendingState,
            1 => WaitingState,
            _ => StartState
        };
        if(nextState == currentState) return;
        if (currentState != null) {
            var oldState = currentState;
            currentState.DOFade(0f, 0.25f).From(1f).OnComplete(()=>oldState.gameObject.SetActive(false));
        }

        currentState = nextState;
        currentState.gameObject.SetActive(true);
        currentState.DOFade(1f, 0.25f).From(0f);
    }
}