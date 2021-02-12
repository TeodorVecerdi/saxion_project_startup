using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Toggle : MonoBehaviour {
    [SerializeField] private float AnimationDuration = 0.25f;
    [SerializeField] private Color ActiveColor;
    [SerializeField] private Color InactiveColor;
    [SerializeField] private Image TargetGraphic;
    [SerializeField] private bool Active;
    [SerializeField] private UnityEvent OnSetActive;
    [SerializeField] private ToggleGroup ToggleGroup;

    [HideInInspector] public bool IsActive;
    
    private void Start() {
        if(Active) ToggleGroup.RequestToggle(this);
    }

    public void SetState(bool state) {
        if(IsActive == state) return;
        IsActive = state;
        TweenSequence();
        if(IsActive) OnSetActive?.Invoke();
    }

    private void TweenSequence() {
        TargetGraphic.DOColor(IsActive ? ActiveColor : InactiveColor, AnimationDuration);
    }
}

