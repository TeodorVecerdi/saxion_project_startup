using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityEngine;

public class MinigameManager : MonoBehaviour {
    [SerializeField] private MinigameMenuController MenuController;

    private string Self => UserState.Instance.UserId;
    private string Other => AppState.Instance.ChattingWith;

    private bool isActive;
    private float requestTimer;
    private bool checkingRequest;
    private bool lookForRequests = true;
    private GameRequestModel gameRequest;

    public void SetActive(bool active) {
        if(isActive && !active) MenuController.Close();
        isActive = active;
        if (isActive) {
            lookForRequests = true;
            requestTimer = 1000.0f;
        }
    }

    private void Update() {
        if (!isActive) return;

        if (lookForRequests) {
            requestTimer += Time.deltaTime;
            if (!checkingRequest && requestTimer >= 2.0f) {
                requestTimer = 0;
                HasRequest();
            }
        }

        if (!checkingRequest && gameRequest != null) {
            if (gameRequest.Initiator == Self) {
                if (gameRequest.Status == GameRequestModel.Unconfirmed) {
                    MenuController.ShowState(1);
                } else {
                    ServerConnection.Instance.MakeRequest("/game/acknowledge-request-status", Method.POST, new List<(string key, string value)> {("from", Self), ("to", Other)});
                    if (gameRequest.Status == GameRequestModel.Denied) {
                        MenuController.ShowState(2);
                    } else {
                        //TODO!: Start game?
                        lookForRequests = false; // game started, stop looking for requests
                    }
                }
            } else {
                if (gameRequest.Status == GameRequestModel.Unconfirmed) {
                    lookForRequests = false; // waiting for input, stop looking for requests
                    MenuController.ShowState(0);
                }
            }
        } else if (!checkingRequest && gameRequest == null) {
            MenuController.ShowState(2);
        }
    }

    public void RequestMinigame() {
        Debug.Log("Requesting minigame");
        gameRequest = new GameRequestModel {
            Initiator = Self,
            Other = Other,
            Status = -1
        };
        ServerConnection.Instance.MakeRequestAsync("/game/request-game", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, null);
    }

    public void ConfirmRequest(int status) {
        Debug.Log("Confirming request");
        gameRequest.Status = status;
        if(status == 0) MenuController.ShowState(2);
        else {
            //TODO!: begin minigame
        }
        ServerConnection.Instance.MakeRequestAsync("/game/confirm-request", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other), ("status", status.ToString())
        }, response => {
            ServerConnection.Instance.MakeRequest("/game/acknowledge-request-status", Method.POST, new List<(string key, string value)> {("from", Self), ("to", Other)});
        });
    }

    private void HasRequest() {
        checkingRequest = true;
        gameRequest = null;
        ServerConnection.Instance.MakeRequestAsync("/game/request", Method.GET, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, response => {
            if (response.Content == "null") {
                gameRequest = null;
                checkingRequest = false;
                return;
            }

            gameRequest = GameRequestModel.Deserialize(JToken.Parse(response.Content));
            checkingRequest = false;
        });
    }
}