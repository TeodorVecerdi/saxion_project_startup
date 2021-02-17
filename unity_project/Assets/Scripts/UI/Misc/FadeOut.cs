using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    [SerializeField] private List<Graphic> Graphics;
    [SerializeField] private float Duration;
    [SerializeField] private UnityEvent OnFadeIn;
    [SerializeField] private UnityEvent OnBeforeFadeIn;
    [SerializeField] private UnityEvent OnFadeOut;
    [SerializeField] private UnityEvent OnBeforeFadeOut;

    public void Reset(bool state) {
        if(Graphics.Count <= 0) return;
        foreach (var graphic in Graphics) {
            graphic.DOFade(state ? 0 : 1, 0).From(state ? 0 : 1);
        }
    }
    
    public void Fade(bool state) {
        if(Graphics.Count <= 0) return;
        (state ? OnBeforeFadeOut : OnBeforeFadeIn)?.Invoke();
        Graphics[0].DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0).OnComplete(() => { (state ? OnFadeOut : OnFadeIn)?.Invoke(); });
        for (var index = 1; index < Graphics.Count; index++) {
            var graphic = Graphics[index];
            graphic.DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0);
        }
    }
}

