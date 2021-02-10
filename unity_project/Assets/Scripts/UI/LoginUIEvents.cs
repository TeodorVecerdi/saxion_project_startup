using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUIEvents : MonoBehaviour {
    
    private UserDataController userDataController;
    private PasswordDataController passwordDataController;
    private string userString;
    private string passwordString;
    
    private void Start() {

    }

    public void OnLoginClick()
    {
        userString = userDataController.GetTextString();
        passwordString = passwordDataController.GetTextString();
    }

    public void OnSignUpClick()
    {
        CursorController.Instance.Default();
        SceneManager.LoadScene("RegisterMenu", LoadSceneMode.Single);
    }

    public void OnForgotClick()
    {
        
    }
    
    public void OnMouseEnter() {
        
    }

    public void OnMouseExit() {
    }
}