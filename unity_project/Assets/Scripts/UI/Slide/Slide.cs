using System;
using DG.Tweening;
using UnityEngine;

public class Slide : MonoBehaviour {
    [SerializeField] private float ExitPosition = 1170f;
    [SerializeField] private float AnimationDuration = 0.5f;
    [SerializeField] private GameObject RaycastBlocker;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetActive(bool isActive) {
        RaycastBlocker.SetActive(!isActive);
        
        if(isActive) EnableTweenSequence();
        else DisableTweenSequence();
    }

    private void EnableTweenSequence() {
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosX(0f, AnimationDuration, true).From(new Vector2(ExitPosition, 0));
    }

    private void DisableTweenSequence() {
        rectTransform.DOAnchorPosX(-ExitPosition, AnimationDuration, true).From(new Vector2(0, 0)).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}

