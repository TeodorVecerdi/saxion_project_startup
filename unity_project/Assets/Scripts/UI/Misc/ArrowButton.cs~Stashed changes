using System;
using TMPro;
using Unity;
using Unity.VectorGraphics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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