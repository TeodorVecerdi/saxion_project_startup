using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private float MaxDrag = 9f;
    [SerializeField, Range(0f, 1f)] private float ConfirmThreshold = .8f;
    [SerializeField] private float DragSensitivity = 0.1f;
    [SerializeField] private float PositionMultiplier = 2.5f;
    [Space, SerializeField] private GameObject Nope;
    [SerializeField] private GameObject Yes;
    [Space]
    [SerializeField] private List<ImageIndicatorPair> Slides;
    [SerializeField] private Color ActiveColor;
    [SerializeField] private Color InactiveColor;
    private bool confirmed = false;
    private bool confirmValue = false;

    private int currentSlide;
    private RectTransform rectTransform;
    private float startDrag;

    private float baseAnchoredY;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();

        baseAnchoredY = rectTransform.anchoredPosition.y;
        SetActiveSlide(0);
    }

    public void OnDrag(PointerEventData eventData) {
        // Calculate drag
        var dragAmount = (eventData.position.x - startDrag) * DragSensitivity;
        if (dragAmount > MaxDrag) dragAmount = MaxDrag;
        if (dragAmount < -MaxDrag) dragAmount = -MaxDrag;
        
        // Change visuals
        rectTransform.anchoredPosition = new Vector2(PositionMultiplier * dragAmount, baseAnchoredY);
        rectTransform.eulerAngles = new Vector3(0, 0, -dragAmount);
        
        // Check threshold
        var confirmNope = dragAmount <= -MaxDrag * ConfirmThreshold;
        var confirmYes = dragAmount >= MaxDrag * ConfirmThreshold;
        if (Nope != null) Nope.SetActive(confirmNope);
        if (Yes != null) Yes.SetActive(confirmYes);

        confirmed = confirmNope || confirmYes;
        confirmValue = confirmYes;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        startDrag = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData) {
        rectTransform.eulerAngles = Vector3.zero;
        rectTransform.anchoredPosition = new Vector2(0, baseAnchoredY);
        
        if (Nope != null) Nope.SetActive(false);
        if (Yes != null) Yes.SetActive(false);

        if (confirmed) {
            Debug.Log($"You chose: {(confirmValue ? "YES" : "NO")}");
        }
    }

    private void SetActiveSlide(int index) {
        Slides[currentSlide].Slide.SetActive(false);
        Slides[currentSlide].Indicator.color = InactiveColor;
        currentSlide = index;
        Slides[currentSlide].Slide.SetActive(true);
        Slides[currentSlide].Indicator.color = ActiveColor;
    }

    public void ChangeSlide(int direction) {
        int newSlide = (currentSlide + direction).Clamped(0, Slides.Count-1);
        SetActiveSlide(newSlide);        
    }

    [Serializable]
    class ImageIndicatorPair {
        public GameObject Slide;
        public Image Indicator;
    }
}

