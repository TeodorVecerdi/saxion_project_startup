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
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private TMP_InputField truth1;
    [SerializeField] private TMP_InputField truth2;
    [SerializeField] private TMP_InputField lie; 
    [SerializeField] [Tooltip("Please use 3 buttons")] 
    private List<Button> outputBtnObjects;
    
    private int _lastLieIndex;
    

    public void OnClick()
    {
        var textsCount = outputBtnObjects.Count;
        var tempOutTexts = new List<TMP_Text>();

        if(textsCount != 3) //The list must have 3 components
            throw new Exception("The output list needs to have a size of 3");

        var saveData = inputPanel.transform.parent.GetComponent<SaveLieData>();
        //Debug.Log(inputPanel.transform.parent);
        saveData.SetOutputList(outputBtnObjects);
        
        for (int index = 0; index < textsCount; index++) // Assign _tempOutTexts TMP_Text Component
        {
            var txtComp = outputBtnObjects[index].transform.GetComponentInChildren<TMP_Text>();
            tempOutTexts.Add(txtComp);
        }
        
        var randomIndex = Random.Range(0, textsCount - 1);
        _lastLieIndex = randomIndex;
        saveData.SetLieIndex(_lastLieIndex);
        
        tempOutTexts[randomIndex].text = lie.text;
        tempOutTexts.Remove(tempOutTexts[randomIndex]);
        
        randomIndex = Random.Range(0, textsCount - 1);
        tempOutTexts[randomIndex].text = truth1.text;
        tempOutTexts.Remove(tempOutTexts[randomIndex]);
        
        randomIndex = 0;
        tempOutTexts[randomIndex].text = truth2.text;
        
        answerPanel.SetActive(true);
        inputPanel.SetActive(false);
    }

    // public List<Button> GetOutputList()
    // {
    //     return outputBtnObjects;
    // }
    //
    // public int GetLieIndex()
    // {
    //     return _lastLieIndex;
    // }
}