using System.Collections.Generic;
using UnityEngine;
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
        _btnList = _lieData.GetOutputList();
    }

    public void OnSelectOption()
    {
        for (int index = 0; index < _btnList.Count; index++)
        {
            var btnSelectAnswerComp = _btnList[index].GetComponent<BtnSelectAnswer>();
            btnSelectAnswerComp.Selected = false;
        }
        Selected = true;
    }

    public void OnDeselect()
    {
        for (int index = 0; index < _btnList.Count; index++)
        {
            var btnSelectAnswerComp = _btnList[index].GetComponent<BtnSelectAnswer>();
            btnSelectAnswerComp.Selected = false;
        }
    }

    public bool GetSelectedBool()
    {
        return Selected;
    }
}
