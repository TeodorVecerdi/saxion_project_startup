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
    [SerializeField] private bool ChangeEnabled;
    
    public void Reset(bool state) {
        if(Graphics.Count <= 0) return;
        foreach (var graphic in Graphics) {
            graphic.DOFade(state ? 0 : 1, 0).From(state ? 0 : 1);
            if(ChangeEnabled) graphic.gameObject.SetActive(!state);
        }
    }
    
    public void Fade(bool state) {
        if(Graphics.Count <= 0) return;
        (state ? OnBeforeFadeOut : OnBeforeFadeIn)?.Invoke();
        Graphics[0].DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0).OnComplete(() => {
            (state ? OnFadeOut : OnFadeIn)?.Invoke();
            if (ChangeEnabled && state) Graphics[0].gameObject.SetActive(false);
        });
        if(ChangeEnabled && !state) Graphics[0].gameObject.SetActive(true);
        for (var index = 1; index < Graphics.Count; index++) {
            var graphic = Graphics[index];
            if(ChangeEnabled && !state) graphic.gameObject.SetActive(true);
            graphic.DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0).OnComplete(() => {
                if (ChangeEnabled && state) graphic.gameObject.SetActive(false);
            });
        }
    }
}

