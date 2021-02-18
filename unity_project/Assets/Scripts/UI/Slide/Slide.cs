using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Slide : MonoBehaviour {
    [SerializeField] private float ExitPosition = 1170f;
    [SerializeField] private float RotationMagnitude = 10.0f;
    [SerializeField] private float AnimationDuration = 0.5f;
    [SerializeField] private GameObject RaycastBlocker;
    [SerializeField] private UnityEvent OnSlideOpened;
    [SerializeField] private UnityEvent OnSlideClosed;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetActive(bool isActive) {
        if(RaycastBlocker != null) RaycastBlocker.SetActive(!isActive);
        
        if(isActive) EnableTweenSequence();
        else DisableTweenSequence();
    }

    public void SetActive(bool isActive, bool reverse) {
        if (!reverse) {
            SetActive(isActive);
            return;
        }
        if(RaycastBlocker != null) RaycastBlocker.SetActive(!isActive);

        if (isActive) EnableTweenSequenceReverse();
        else DisableTweenSequenceReverse();
    }

    private void EnableTweenSequence() {
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosX(0f, AnimationDuration, true).From(new Vector2(ExitPosition, 0));
        rectTransform.DOLocalRotate(Vector3.zero, AnimationDuration).From(new Vector3(0, 0, -RotationMagnitude)).OnComplete(() => {
            OnSlideOpened?.Invoke();
        });
    }
    
    private void EnableTweenSequenceReverse() {
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosX(0f, AnimationDuration, true).From(new Vector2(-ExitPosition, 0));
        rectTransform.DOLocalRotate(Vector3.zero, AnimationDuration).From(new Vector3(0, 0, RotationMagnitude)).OnComplete(() => {
            OnSlideOpened?.Invoke();
        });
    }

    private void DisableTweenSequence() {
        rectTransform.DOLocalRotate(new Vector3(0, 0, RotationMagnitude), AnimationDuration).From(Vector3.zero);
        rectTransform.DOAnchorPosX(-ExitPosition, AnimationDuration, true).From(new Vector2(0, 0)).OnComplete(() => {
            gameObject.SetActive(false);
            OnSlideClosed?.Invoke();
        });
    }
    
    private void DisableTweenSequenceReverse() {
        rectTransform.DOLocalRotate(new Vector3(0, 0, -RotationMagnitude), AnimationDuration).From(Vector3.zero);
        rectTransform.DOAnchorPosX(ExitPosition, AnimationDuration, true).From(new Vector2(0, 0)).OnComplete(() => {
            gameObject.SetActive(false);
            OnSlideClosed?.Invoke();
        });
    }
}

