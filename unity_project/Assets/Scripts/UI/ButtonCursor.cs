using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCursor : MonoBehaviour {
    private static CursorController instance;
    private void Start() {
        instance = FindObjectOfType<CursorController>();
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        
        var pointerEnter = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
        pointerEnter.callback.AddListener(arg0 => instance.Link());
        var pointerExit = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
        pointerExit.callback.AddListener(arg0 => instance.Default());
      //  var pointerDown = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
      //  pointerDown.callback.AddListener(arg0 => SoundManager.PlaySound("click"));
        
        eventTrigger.triggers.Add(pointerEnter);
        eventTrigger.triggers.Add(pointerExit);
        //eventTrigger.triggers.Add(pointerDown);
        
        Destroy(this);
    }
}