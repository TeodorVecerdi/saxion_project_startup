
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLieData : MonoBehaviour
{
    private int _lieIndex = 9;
    private List<Button> _outputList;

    public int GetLieIndex()
    {
        return _lieIndex;
    }

    public List<Button> GetOutputList()
    {
        return _outputList;
    }
    
    public void SetLieIndex(int index)
    {
        _lieIndex = index;
    }

    public void SetOutputList(List<Button> btnList)
    {
        _outputList = btnList;
    }

}
