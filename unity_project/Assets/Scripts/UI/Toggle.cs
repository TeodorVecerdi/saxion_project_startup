using System;
using System.Collections.Generic;
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
    [SerializeField] private bool CanBeDisabled;
    [SerializeField] private List<Toggle> ToggleGroup;
    [SerializeField] private UnityEvent OnSetActive;

    private bool isActive;
    
    private void Start() {
        TargetGraphic.color = Active ? ActiveColor : InactiveColor;
        isActive = Active;
    }

    private void SetState(bool state) {
        if(isActive == state) return;
        
        foreach (var toggle in ToggleGroup) {
            if(toggle != this)
                toggle.SetState(false);
        }
        isActive = state;
        TweenSequence();
        if(isActive) OnSetActive?.Invoke();
    }

    public void ToggleState() {
        if(!CanBeDisabled && isActive) return;
        SetState(!isActive);
    }

    private void TweenSequence() {
        TargetGraphic.DOColor(isActive ? ActiveColor : InactiveColor, AnimationDuration);
    }
}

