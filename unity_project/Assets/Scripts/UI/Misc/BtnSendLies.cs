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
public class BtnSendLies : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField]
    private TMP_InputField truth1;
    [SerializeField] 
    private TMP_InputField truth2;
    [SerializeField]
    private TMP_InputField lie; 
    [SerializeField] [Tooltip("Please use 3 buttons")]
    private List<TMP_Text> outputTexts;

    private int _lastLieIndex;

    public void OnClick()
    {
        var textsCount = outputTexts.Count;

        if (textsCount != 3)
            throw new Exception("The texts list needs to have a size of 3");
        
        var randomIndex = Random.Range(0, textsCount - 1);
        _lastLieIndex = randomIndex;
        outputTexts[randomIndex].text = lie.text;
        outputTexts.Remove(outputTexts[randomIndex]);
        
        randomIndex = Random.Range(0, textsCount - 1);
        outputTexts[randomIndex].text = truth1.text;
        outputTexts.Remove(outputTexts[randomIndex]);
        randomIndex = 0;
        outputTexts[randomIndex].text = truth2.text;
        
        inputPanel.SetActive(false);
    }

    // public List<TMP_Text> GetTextList()
    // {
    //     return outputTexts;
    // }
    
    public int GetLieIndex()
    {
        return _lastLieIndex;
    }
}