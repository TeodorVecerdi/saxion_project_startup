using TMPro;
using UnityEngine;

public class SpawnPopup : MonoBehaviour
{
    [SerializeField]private GameObject popupPrefab;
    [SerializeField]private string popupText;
    private GameObject _clickedBtn;

    // private void Initialize() {
    //     
    // }

    public void OnClick()
    {
        _clickedBtn = this.gameObject;
        var popupObj = Instantiate(popupPrefab, transform.parent);
        var textComp = popupObj.GetComponent<TMP_Text>();
        textComp.text = _clickedBtn + popupText;
        
    }
    
}
