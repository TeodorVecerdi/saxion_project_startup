using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class TestLikesAnimation : MonoBehaviour {
    [SerializeField] private RectTransform Source;
    [SerializeField] private RectTransform Target;
    [SerializeField] private FadeOut Fade1;
    [SerializeField] private FadeOut Fade2;
    [SerializeField] private float AnimationDuration = 1f;
    [SerializeField] private List<GameObject> ObjectsToHide;
    [SerializeField] private List<GameObject> ObjectsToShow;

    private Vector2 startingSize = new Vector2(144, 240);
    private Vector2 endSize = new Vector2(288, 420);

    private static Vector2 anchorLeftMin = Vector2.up;
    private static Vector2 anchorLeftMax = Vector2.up;
    private static Vector2 anchorCenterMin = new Vector2(0.5f, 1f);
    private static Vector2 anchorCenterMax = new Vector2(0.5f, 1f);
    private static Vector2 anchorRightMin = Vector2.one;
    private static Vector2 anchorRightMax = Vector2.one;

    private List<(Vector2, Vector2)> anchors = new List<(Vector2, Vector2)> {(anchorLeftMin, anchorLeftMax), (anchorCenterMin, anchorCenterMax), (anchorRightMin, anchorRightMax)};
    private List<RectTransform> children = null;

    [Button]
    public void Open() {
        if (children == null) {
            children = new List<RectTransform>();
            for (var i = 0; i < Source.childCount; i++) {
                children.Add(Source.GetChild(i).GetComponent<RectTransform>());
            }
        }
        
        Fade1.Fade(false);
        Fade2.Fade(true);
        
        foreach (var @object in ObjectsToHide) {
            @object.SetActive(false);
        }
        foreach (var @object in ObjectsToShow) {
            @object.SetActive(true);
        }

        foreach (var child in children) {
            child.SetParent(Target, true);
        }

        var height = ((children.Count / 3) + 1) * (endSize.y + 64f) - 64f;
        Target.sizeDelta = new Vector2(0, height);
        for (var i = 0; i < Mathf.Min(children.Count, 9); i++) {
            var (anchorMin, anchorMax) = anchors[i % 3];
            var topOffset = (i / 3) * (endSize.y + 64f);
            children[i].DOAnchorMin(anchorMin, AnimationDuration);
            children[i].DOAnchorMax(anchorMax, AnimationDuration);
            children[i].DOPivot(anchorMax, AnimationDuration);
            children[i].DOSizeDelta(endSize, AnimationDuration);
            children[i].DOAnchorPosY(-topOffset, AnimationDuration);
            children[i].DOAnchorPosX(0, AnimationDuration);
        }
        for (var i = 9; i < children.Count; i++) {
            children[i].gameObject.SetActive(false);
        }
    }

    [Button]
    public void Close() {
        Fade1.Fade(true);
        Fade2.Fade(false);
        
        foreach (var @object in ObjectsToHide) {
            @object.SetActive(true);
        }
        
        children[0].DOAnchorMin(anchorLeftMin, AnimationDuration);
        children[0].DOAnchorMax(anchorLeftMax, AnimationDuration);
        children[0].DOPivot(anchorLeftMin, AnimationDuration);
        children[0].DOSizeDelta(startingSize, AnimationDuration);
        children[0].DOAnchorPosX(0, AnimationDuration);
        children[0].DOAnchorPosY(0, AnimationDuration).OnComplete(() => {
            foreach (var child in children) {
                child.SetParent(Source, true);
            }
            
            foreach (var @object in ObjectsToShow) {
                @object.SetActive(false);
            }
            for (var i = 9; i < children.Count; i++) {
                children[i].gameObject.SetActive(true);
            }
        });
        for (var i = 1; i < Mathf.Min(children.Count, 9); i++) {
            var xOffset = i * (startingSize.x + 32f);
            children[i].DOAnchorMin(anchorLeftMin, AnimationDuration);
            children[i].DOAnchorMax(anchorLeftMax, AnimationDuration);
            children[i].DOPivot(anchorLeftMin, AnimationDuration);
            children[i].DOSizeDelta(startingSize, AnimationDuration);
            children[i].DOAnchorPosX(xOffset, AnimationDuration);
            children[i].DOAnchorPosY(0, AnimationDuration);
        }
    }
}