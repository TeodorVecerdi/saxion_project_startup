using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    [SerializeField] private List<Graphic> Graphics;
    [SerializeField] private float Duration;
    [SerializeField] private UnityEvent OnFadeIn;
    [SerializeField] private UnityEvent OnFadeOut;

    public void Fade(bool state) {
        if(Graphics.Count <= 0) return;
        Graphics[0].DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0).OnComplete(() => { (state ? OnFadeOut : OnFadeIn)?.Invoke(); });
        for (var index = 1; index < Graphics.Count; index++) {
            var graphic = Graphics[index];
            graphic.DOFade(state ? 0 : 1, Duration).From(state ? 1 : 0);
        }
    }
}

