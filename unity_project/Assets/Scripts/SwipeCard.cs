using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private float MaxDrag = 9f;
    [SerializeField, Range(0f, 1f)] private float ConfirmThreshold = .8f;
    [SerializeField] private float DragSensitivity = 0.1f;
    [SerializeField] private float PositionMultiplier = 2.5f;
    [Space, SerializeField] private GameObject Nope;
    [SerializeField] private GameObject Yes;
    
    private RectTransform rectTransform;
    private float startDrag;

    private float baseAnchoredY;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();

        baseAnchoredY = rectTransform.anchoredPosition.y;
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
        Nope.SetActive(dragAmount <= -MaxDrag * ConfirmThreshold);
        Yes.SetActive(dragAmount >= MaxDrag * ConfirmThreshold);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        startDrag = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData) {
        rectTransform.eulerAngles = Vector3.zero;
        rectTransform.anchoredPosition = new Vector2(0, baseAnchoredY);
        
        Nope.SetActive(false);
        Yes.SetActive(false);
    }
}

