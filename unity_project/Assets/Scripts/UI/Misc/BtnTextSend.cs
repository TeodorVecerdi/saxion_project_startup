using System;
using DG.Tweening;
using UnityEngine;
using Unity;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BtnTextSend : MonoBehaviour
{
    [SerializeField]
    private GameObject messagePrefab;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField] 
    private GameObject contentPanel;

    private RectTransform _panelRectTransform;

    public void Awake()
    {
        _panelRectTransform = contentPanel.GetComponent<RectTransform>();
    }

    public void OnClick()
    {
        var newMessage = Instantiate(messagePrefab, _panelRectTransform);
        var outputField = newMessage.GetComponent<TMP_InputField>();
        outputField.text = inputField.text;
    }
}