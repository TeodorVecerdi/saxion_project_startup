using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Button))]
public class BtnSendOptions : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField]
    private TMP_InputField truth1;
    [SerializeField] 
    private TMP_InputField truth2;
    [SerializeField]
    private TMP_InputField lie;
    [SerializeField] [Tooltip("Please use 3 buttons")]
    private List<TMP_Text> outputButtons; 
    
    public void OnClick()
    {
        var buttonCount = outputButtons.Count;
        var randomIndex = Random.Range(0, buttonCount - 1);
        outputButtons[randomIndex].text = lie.text;
        outputButtons.Remove(outputButtons[randomIndex]);
        randomIndex = Random.Range(0, buttonCount - 1);
        outputButtons[randomIndex].text = truth1.text;
        outputButtons.Remove(outputButtons[randomIndex]);
        randomIndex = 0;
        outputButtons[randomIndex].text = truth2.text;
        
        inputPanel.SetActive(false);
    }
}