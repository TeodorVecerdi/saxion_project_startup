using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BtnAnswerMinigame : MonoBehaviour
{
    [SerializeField] private GameObject answerPanel;
    
    private Transform _parentPanel;
    private List<Button> _outputBtnObjects;
    private SaveLieData _lieData;
    private int _lieIndex;

    public void OnEnable()
    {
        _parentPanel = gameObject.transform.parent.parent;
        _lieData = _parentPanel.GetComponent<SaveLieData>();
        
        if(_lieIndex != 9)
             _lieIndex = _lieData.GetLieIndex();
        else
            throw new Exception("Lie index wasn't saved");
        
        _outputBtnObjects = _lieData.GetOutputList();
    }

    public void OnClick()
    {
        if (_outputBtnObjects[_lieIndex].GetComponent<BtnSelectAnswer>().GetSelectedBool()) //check if the lie is selected
            //winPanel.SetActive(true);
            Debug.Log("WIN!");
        else
            Debug.Log("LOSE!");
        //losePanel.SetActive(true);
        
        //answerPanel.SetActive(false);
    }
    
}