using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldNoTabKey : MonoBehaviour {
    private TMP_InputField inputField;
    private void Awake() {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnNewValue);
    }

    private void OnNewValue(string newValue) {
        if (newValue.Length - 1 >= 0) {
            if (newValue[inputField.stringPosition-1] == '\t') {
                newValue = newValue.Substring(0, inputField.stringPosition-1) + newValue.Substring(inputField.stringPosition, newValue.Length - inputField.stringPosition);
                inputField.text = newValue;
            }
        }
    }
}

