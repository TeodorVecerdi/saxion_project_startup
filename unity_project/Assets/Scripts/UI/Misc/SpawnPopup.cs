using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPopup : MonoBehaviour
{
    [SerializeField]private GameObject popupPrefab;
    [SerializeField]private string titleText = "Sorry";
    [SerializeField]private string thingNotFree = "This feature";
    [SerializeField]private string followUpText = "is unavailable currently. Buy ScoreMe gold to unlock this feature.";

    public void ShowPopup()
    {
        var popupObj = Instantiate(popupPrefab, new Vector3(325, 644), Quaternion.identity, transform.parent); //hardcode yay
        var titleComponent = popupObj.transform.GetChild(0).GetComponent<TMP_Text>();
        var txtComponent = popupObj.transform.GetChild(1).GetComponent<TMP_Text>();

        titleComponent.text = titleText;
        txtComponent.text = thingNotFree + " " + followUpText;
    }
    
}
