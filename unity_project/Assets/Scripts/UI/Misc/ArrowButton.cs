using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    
    public void OnArrowDownClick()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }
    
    public void OnArrowUpClick()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
    }
}