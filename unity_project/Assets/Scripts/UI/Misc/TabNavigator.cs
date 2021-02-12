using System;
using TMPro;
using UnityCommons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNavigator : MonoSingleton<TabNavigator> {
    private void Update() {
        var shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (Input.GetKeyDown(KeyCode.Tab)) {
            var selectable = shift ? GetPrevious() : GetNext();
            if(selectable == null) return;
            
            var inputField = selectable.GetComponent<TMP_InputField>();
            if(inputField != null) inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            
            EventSystem.current.SetSelectedGameObject(selectable.gameObject, new BaseEventData(EventSystem.current));
        }
    }

    private Selectable GetPrevious() {
        if (EventSystem.current.currentSelectedGameObject == null) return null;
        var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        if (currentSelected == null) return null;
        return currentSelected.FindSelectableOnUp() ?? currentSelected.FindSelectableOnLeft();
    }

    private Selectable GetNext() {
        if (EventSystem.current.currentSelectedGameObject == null) return null;
        var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        if (currentSelected == null) return null;
        return currentSelected.FindSelectableOnDown() ?? currentSelected.FindSelectableOnRight();
    }
}

