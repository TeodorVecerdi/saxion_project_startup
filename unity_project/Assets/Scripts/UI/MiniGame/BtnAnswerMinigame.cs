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
public class BtnAnswerMinigame : MonoBehaviour
{
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private GameObject panels;
    
    private List<Button> _outputBtnObjects;
    private SaveLieData _lieData;
    private int _lieIndex;
    
    public void Awake()
    {
        if(_lieIndex != 9)
             _lieIndex = _lieData.GetLieIndex();
        else
        {
            throw new Exception("Lie index wasn't saved");
        }
        //_outputBtnObjects = _lieData.GetOutputList();
        _outputBtnObjects.AddRange(_lieData.GetOutputList());
    }

    public void OnClick()
    {
        // btnSendLies = gameObject.GetComponent<BtnSendLies>();
        // var lieIndex = btnSendLies.GetLieIndex();
        // _outputBtnObjects = btnSendLies.GetOutputList();

        if (_outputBtnObjects[_lieIndex].GetComponent<BtnSelectAnswer>().GetSelectedBool()); //check if the lie is selected
            //winPanel.SetActive(true);
            Debug.Log("WIN!");
        Debug.Log("LOSE!");
        //losePanel.SetActive(true);
        
        answerPanel.SetActive(false);
    }
    
}