using System;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour {
    [SerializeField] private List<Slide> Slides;
    [SerializeField] private bool WrapAtEnd;
    [SerializeField] private int StartingSlide;

    private void Start() {
        currentSlide = StartingSlide;
    }

    private int currentSlide;

    public void NextSlide() {
        var newSlide = currentSlide + 1;
        if (newSlide >= Slides.Count) {
            if (WrapAtEnd) newSlide = 0;
            else return;
        }

        Slides[currentSlide].SetActive(false);
        currentSlide = newSlide;
        Slides[currentSlide].SetActive(true);
    }

    public void PreviousSlide() {
        var newSlide = currentSlide - 1;

        if (newSlide < 0) {
            if (WrapAtEnd) newSlide = Slides.Count - 1;
            else return;
        }

        Slides[currentSlide].SetActive(false, true);
        currentSlide = newSlide;
        Slides[currentSlide].SetActive(true, true);
    }
}