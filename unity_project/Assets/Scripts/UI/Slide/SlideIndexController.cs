using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SlideIndexController : MonoBehaviour {
    [HorizontalLine(color: EColor.Blue)]
    [SerializeField, BoxGroup("References")]
    private List<Image> Slides;
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField, Foldout("Duration")] private float EnableDuration = 0.5f;
    [SerializeField, Foldout("Duration")] private float SlideChangeDuration = 0.35f;
    [HorizontalLine(color: EColor.Indigo)]
    [SerializeField, Foldout("States")] private float ActiveSize;
    [SerializeField, Foldout("States")] private Color ActiveColor;
    [SerializeField, Foldout("States")] private float InactiveSize;
    [SerializeField, Foldout("States")] private Color InactiveColor;
    [HorizontalLine(color: EColor.Green)]
    [SerializeField, Foldout("Initialization")] private bool Enabled;
    [SerializeField, Foldout("Initialization")] private int Slide;

    private int currentSlide;
    private bool isEnabled;

    private void Start() {
        for (var index = 0; index < Slides.Count; index++) {
            var slide = Slides[index];

            var slideSize = new Vector2(ActiveSize, ActiveSize);
            if (index != Slide) slideSize.y = InactiveSize;
            var slideColor = index == Slide ? ActiveColor : InactiveColor;
            if (!Enabled) slideColor.a = 0;

            slide.preserveAspect = true;
            slide.rectTransform.sizeDelta = slideSize;
            slide.color = slideColor;
        }

        isEnabled = Enabled;
        currentSlide = Slide;
    }

    public void SetEnabled(bool isEnabled) {
        if (isEnabled && !this.isEnabled) EnableTweenSequence();
        else if (!isEnabled && this.isEnabled) DisableTweenSequence();

        this.isEnabled = isEnabled;
    }

    public void SetSlide(int slide) {
        if(currentSlide == slide) return;
        ChangeSlideTweenSequence(slide);
    }

    private void EnableTweenSequence() {
        foreach (var slide in Slides) {
            slide.DOFade(1.0f, EnableDuration);
        }
    }

    private void DisableTweenSequence() {
        foreach (var slide in Slides) {
            slide.DOFade(0.0f, EnableDuration);
        }
    }

    private void ChangeSlideTweenSequence(int newSlide) {
        var inactiveSize = new Vector2(ActiveSize, InactiveSize);
        var activeSize = new Vector2(ActiveSize, ActiveSize);
       
        Slides[currentSlide].DOColor(InactiveColor, SlideChangeDuration);
        Slides[currentSlide].rectTransform.DOSizeDelta(inactiveSize, SlideChangeDuration);
        currentSlide = newSlide;
        Slides[currentSlide].DOColor(ActiveColor, SlideChangeDuration);
        Slides[currentSlide].rectTransform.DOSizeDelta(activeSize, SlideChangeDuration);
    }
}