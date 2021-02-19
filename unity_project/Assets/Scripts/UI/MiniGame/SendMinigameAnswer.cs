using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendMinigameAnswer : MonoBehaviour {
    [SerializeField] private ToggleGroup ToggleGroup;

    public void OnClick() {
        // Get toggle group value
        var selectedIndex = ToggleGroup.GetSelectedIndex();
        // TODO!: Send request!
    }
}