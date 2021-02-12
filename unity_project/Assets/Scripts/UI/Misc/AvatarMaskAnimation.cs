using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]
public class AvatarMaskAnimation : MonoBehaviour {
    [SerializeField] private float StartPadding = 300.0f;
    [SerializeField] private float EndPadding = 950.0f;
    [SerializeField] private float AnimationDuration = 2.0f;
    private RectMask2D mask;

    private void Awake() {
        mask = GetComponent<RectMask2D>();
    }

    private void Start() {
        AnimateRight();
    }

    private void AnimateRight() {
        DOTween.To(() => mask.padding.x, value => {
                       var padding = mask.padding;
                       padding.x = value;
                       mask.padding = padding;
                   }, EndPadding, AnimationDuration).From(StartPadding).OnComplete(AnimateLeft);
    }

    private void AnimateLeft() {
        DOTween.To(() => mask.padding.x, value => {
            var padding = mask.padding;
            padding.x = value;
            mask.padding = padding;
        }, StartPadding, AnimationDuration).From(EndPadding).OnComplete(AnimateRight);
    }
}