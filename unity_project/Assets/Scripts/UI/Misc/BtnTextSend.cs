using System;
using UnityEngine;
using Unity;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class BtnTextSend : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField]
    private TMP_InputField target;

    private void Awake()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
    }

    public void OnClick()
    {
        target.text = inputField.text;
    }
}