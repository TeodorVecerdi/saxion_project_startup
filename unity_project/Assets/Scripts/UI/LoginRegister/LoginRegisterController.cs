using System;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using RestSharp;
using TMPro;
using UnityCommons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginRegisterController : MonoBehaviour {
    [SerializeField] private Button LoginButton;
    [SerializeField] private Button RegisterButton;
    [SerializeField] private TextMeshProUGUI LoginServerResponseText;
    [SerializeField] private TextMeshProUGUI RegisterServerResponseText;
    [SerializeField] private LoginRegisterTransition LoginRegisterTransition;
    [SerializeField] private LoginTransition LoginTransition;

    private string loginUsername;
    private string loginPassword;

    private string registerUsername;
    private string registerPassword;
    private string registerRepeatPassword;

    public void OnLoginClick() {
        ServerConnection.Instance.MakeRequestAsync("login", Method.POST, new List<(string key, string value)> {("username", loginUsername), ("password", loginPassword)},
                                                   response => {
                                                       if (response.StatusCode == 404) {
                                                           LoginServerResponseText.text = "Invalid username or password";
                                                       } else if (response.StatusCode == 200) {
                                                           OnLoginSuccessful(response.Content);
                                                       }
                                                   });
    }

    private void OnLoginSuccessful(string accountJson) {
        var json = JObject.Parse(accountJson)["account"];
        
        UserState.Instance.UserId = json.Value<string>("id");
        var profile = json["profile"];
        var profileSetUp = profile["name"] != null;
        if (profileSetUp) {
            LoginTransition.Activate();
            SceneLoader.Instance.LoadScene(Scenes.SwipeMenu, () => {
                IDisposable cancel = null;
                cancel = UpdateUtility.Create(() => {
                    if(!AppState.Instance.DoneLoadingInitial) return;
                    
                    cancel.Dispose();
                    LoginTransition.OnLoadComplete += () => {
                        SceneManager.UnloadSceneAsync(Scenes.LoginRegister);
                    };
                    LoginTransition.StartWaitTimer = true;
                });
            });
        } else {
           SceneLoader.Instance.LoadScene(Scenes.Onboarding, () => {
               SceneManager.UnloadSceneAsync(Scenes.LoginRegister);
           });
        }
    }

    public void OnRegisterClick() {
        ServerConnection.Instance.MakeRequestAsync("register", Method.POST, new List<(string key, string value)> {("username", registerUsername), ("password", registerPassword)},
                                                   response => {
                                                       if (response.StatusCode == 409) {
                                                           RegisterServerResponseText.text = "Username already exists";
                                                       } else if (response.StatusCode == 201) {
                                                           RegisterServerResponseText.text = "Signup successful";
                                                           LoginRegisterTransition.ShowSuccess();
                                                       }
                                                   });
    }

    public void OnLoginUsernameChanged(string newValue) {
        loginUsername = newValue.Trim();
        LoginButton.interactable = !string.IsNullOrWhiteSpace(loginUsername) && !string.IsNullOrWhiteSpace(loginPassword);
        LoginServerResponseText.text = "";
    }

    public void OnLoginPasswordChanged(string newValue) {
        loginPassword = newValue.Trim();
        LoginButton.interactable = !string.IsNullOrWhiteSpace(loginUsername) && !string.IsNullOrWhiteSpace(loginPassword);
        LoginServerResponseText.text = "";
    }

    public void OnRegisterUsernameChanged(string newValue) {
        registerUsername = newValue.Trim();
        RegisterButton.interactable = !string.IsNullOrWhiteSpace(registerUsername)
                                   && !string.IsNullOrWhiteSpace(registerPassword)
                                   && !string.IsNullOrWhiteSpace(registerRepeatPassword)
                                   && string.Equals(registerPassword, registerRepeatPassword);
        RegisterServerResponseText.text = "";
    }

    public void OnRegisterPasswordChanged(string newValue) {
        registerPassword = newValue.Trim();
        RegisterButton.interactable = !string.IsNullOrWhiteSpace(registerUsername)
                                   && !string.IsNullOrWhiteSpace(registerPassword)
                                   && !string.IsNullOrWhiteSpace(registerRepeatPassword)
                                   && string.Equals(registerPassword, registerRepeatPassword);

        RegisterServerResponseText.text = !string.Equals(registerPassword, registerRepeatPassword) ? "Password and repeat password don't match" : "";
    }

    public void OnRegisterRepeatPasswordChanged(string newValue) {
        registerRepeatPassword = newValue.Trim();
        RegisterButton.interactable = !string.IsNullOrWhiteSpace(registerUsername)
                                   && !string.IsNullOrWhiteSpace(registerPassword)
                                   && !string.IsNullOrWhiteSpace(registerRepeatPassword)
                                   && string.Equals(registerPassword, registerRepeatPassword);
        
        RegisterServerResponseText.text = !string.Equals(registerPassword, registerRepeatPassword) ? "Password and repeat password don't match" : "";
    }
}