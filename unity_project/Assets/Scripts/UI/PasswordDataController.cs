using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class PasswordDataController : MonoBehaviour
{
    public static PasswordDataController Instance { get; private set; }
    private TMP_InputField inputField;
   /*
    private bool wasFocusedLastFrame;
    private bool isFilledIn;
    private float fillTimer;
    private float alphaTimer;
    private bool isFillingIn, isFillingOut, isAlphaIncreasing, isAlphaDecreasing;
    */

   private void Awake() {
       if (Instance == null)
           Instance = this;
       else throw new Exception("There can only be one PasswordDataController in a scene!");
   }
   
   public TMP_InputField GetInputField()
   {
       return inputField;
   }
   
    public string GetTextString()
    {
        return inputField.text;
    }
    
    /*
    private void ProcessInput() {
        if (inputField.isFocused && !wasFocusedLastFrame) {
            if(!isFilledIn) FillIn();
            IncreaseAlpha();
        } else if (!inputField.isFocused && wasFocusedLastFrame) {
            if(isFilledIn) FillOut();
            DecreaseAlpha();
        }

        wasFocusedLastFrame = inputField.isFocused;
    }

    
    public void UpdateName(string newName) {
    }
    
    
    public void OnPointerEnter() {
        if (isFilledIn) return;
        FillIn();
    }

    public void OnPointerExit()
    {
        if (inputField.isFocused) return;
        FillOut();
    }
    private void FillIn() {
        isFillingIn = true;
        isFilledIn = true;
        if (isFillingOut) {
            isFillingOut = false;
            fillTimer = AnimationDuration - fillTimer;
        }
        else fillTimer = 0f;
    }

    private void FillOut() {
        isFillingOut = true;
        isFilledIn = false;
        if (isFillingIn) {
            isFillingIn = false;
            fillTimer = AnimationDuration - fillTimer;
        }
        else fillTimer = 0f;
    }
    
    private void IncreaseAlpha() {
        isAlphaIncreasing = true;
        if (isAlphaDecreasing)
            isAlphaDecreasing = false;
        else alphaTimer = 0f;
    }

    private void DecreaseAlpha() {
        isAlphaDecreasing = true;
        if (isAlphaIncreasing)
            isAlphaIncreasing = false;
        else alphaTimer = 0f;
    }
    */
}
