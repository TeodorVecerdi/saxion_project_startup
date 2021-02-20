using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCustomizerTabButtons : MonoBehaviour {
    [SerializeField] private List<RectTransform> Tabs;
    [SerializeField] private int StartingTab;
    [SerializeField] private SlideController SlideController;

    private const float containerSize = 1030.0f;
    private int currentTab;

    private void Start() {
        currentTab = StartingTab;
    }

    public void Activate(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;
        if(index == currentTab) return;
        bool activateLeft = index >= 1;
        bool activateRight = index < Tabs.Count-1;
        bool moveInLeft = index >= 2;
        bool moveInRight = index < Tabs.Count-2;
        if (index > currentTab) {
            SlideController.NextSlide();
            DeactivateToLeft(currentTab);
            MoveOutToLeft(currentTab - 1);
            MoveInFromRight(currentTab + 2);
        } else {
            SlideController.PreviousSlide();
            DeactivateToRight(currentTab);
            MoveOutToRight(currentTab + 1);
            MoveInFromLeft(currentTab - 2);
        }
        
        // Compare with currentTab
        // - check if should move in offscreen tab to left/right
        // - check if should move offscreen tab to left/right
        // - check if should deactivate to left/right

        currentTab = index;
        var tab = Tabs[currentTab];
        var fitter = tab.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        
        // tab.pivot = tab.anchorMin = tab.anchorMax = Vector2.one * 0.5f;
        tab.DOPivotX(0.5f, 0.5f);
        tab.DOAnchorMin(Vector2.one * 0.5f, 0.5f);
        tab.DOAnchorMax(Vector2.one * 0.5f, 0.5f);
        tab.DOAnchorPosX(0.0f, 0.5f);
        tab.DOSizeDelta(new Vector2(512.0f, 116.0f), 1.0f);
    }

    public void DeactivateToLeft(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;

        var tab = Tabs[index];
        var fitter = tab.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        tab.DOPivotX(0.0f, 0.5f);
        tab.DOAnchorMin(Vector2.up * 0.5f, 0.5f);
        tab.DOAnchorMax(Vector2.up * 0.5f, 0.5f);
        tab.DOAnchorPosX(8.0f, 0.5f);
    }
    
    public void DeactivateToRight(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;

        var tab = Tabs[index];
        var fitter = tab.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        tab.DOPivotX(1.0f, 0.5f);
        tab.DOAnchorMin(new Vector2(1.0f, 0.5f), 0.5f);
        tab.DOAnchorMax(new Vector2(1.0f, 0.5f), 0.5f);
        tab.DOAnchorPosX(-8.0f, 0.5f);
    }
    
    public void MoveOutToLeft(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;
        var tab = Tabs[index];
        tab.DOAnchorPosX(-containerSize/2.0f - 128.0f, 0.5f);
    }
    
    public void MoveOutToRight(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;
        var tab = Tabs[index];
        tab.DOAnchorPosX(containerSize/2.0f + 128.0f, 0.5f);
    }
    
    public void MoveInFromLeft(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;
        var tab = Tabs[index];
        tab.DOAnchorPosX(8.0f, 0.5f);
    }
    
    public void MoveInFromRight(int index) {
        if(!Utils.RangeCheck(index, Tabs.Count)) return;
        var tab = Tabs[index];
        tab.DOAnchorPosX(-8.0f, 0.5f);
    }
}