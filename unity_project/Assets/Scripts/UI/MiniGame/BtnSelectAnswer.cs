using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BtnSelectAnswer : MonoBehaviour
{ 
    [SerializeField] private bool Selected = false;

    private Transform parentPanel;
    private List<Button> _btnList;
    private SaveLieData _lieData;

    private void OnEnable()
    {
        parentPanel = gameObject.transform.parent.parent;
        _lieData = parentPanel.GetComponent<SaveLieData>();
        _btnList.AddRange(_lieData.GetOutputList());
    }

    public void OnSelectOption()
    {
        var selectAnswerList = new List<BtnSelectAnswer>();
        for (int index = 0; index < _btnList.Count; index++)
        {
            var btnSelectAnswerComp = _btnList[index].GetComponent<BtnSelectAnswer>();
            btnSelectAnswerComp.Selected = false;
            selectAnswerList.Add(btnSelectAnswerComp);
        }

        Selected = true;
    }

    public bool GetSelectedBool()
    {
        return Selected;
    }
}
