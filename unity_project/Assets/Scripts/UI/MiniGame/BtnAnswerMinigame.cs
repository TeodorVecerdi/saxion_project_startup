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
public class BtnAnswerMinigames : MonoBehaviour
{
    [SerializeField]
    private GameObject answerPanel;

    [SerializeField] private BtnSendLies btnSendLies;
    //[SerializeField] [Tooltip("Please use 3 buttons")]
    private List<Button> outputBtnObjects;
    
    private int _selected = 0;
    

    public void OnClick() {
        btnSendLies = gameObject.transform.GetComponentInParent<BtnSendLies>();
        var lieIndex = btnSendLies.GetLieIndex();
        outputBtnObjects = btnSendLies.GetOutputList();
        

        answerPanel.SetActive(false);
    }

    public void OnClickSelectOption()
    {
        
    }
}