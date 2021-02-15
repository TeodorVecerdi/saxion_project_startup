using System;
using TMPro;
using Unity;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private SVGImage chatBubble;

    [SerializeField] private TMP_Text text;

    private void Start()
    {
        var fontSize = text.fontSize;
    }

    private void OnInputText()
    {
        
    }
}