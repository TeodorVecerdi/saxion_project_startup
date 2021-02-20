using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class MinigameManager : MonoBehaviour {
    [SerializeField] private MinigameMenuController MenuController;
    [SerializeField] private MinigamePanelController PanelController;

    private string Self => UserState.Instance.UserId;
    private string Other => AppState.Instance.ChattingWith;
    private int NextSelf => gameModel.Initiator == Self ? 0 : 1;
    private int NextOther => gameModel.Initiator == Self ? 1 : 0;

    private bool isActive;
    private float requestTimer;
    private bool checkingRequest;
    private bool lookForRequests = true;
    private GameRequestModel gameRequest;

    private float gameTimer;
    private bool checkingGame;
    private bool lookForGames = true;
    private bool updateGameState;
    private GameModel gameModel;

    public void SetActive(bool active) {
        if (isActive && !active) MenuController.Close();
        isActive = active;
        if (isActive) {
            lookForGames = true;
            updateGameState = true;
            gameTimer = 1000f;
            lookForRequests = true;
            requestTimer = 1000f;
        }
    }

    public void ToggleMinigameMenu() {
        if (gameModel != null && gameModel.Next[NextSelf] != -1) {
            PanelController.Open();
        }
        else MenuController.Toggle();
    }

    private void Update() {
        if (!isActive) return;

        if (lookForGames) {
            gameTimer += Time.deltaTime;
            if (!checkingGame && gameTimer >= 4.0f) {
                gameTimer = 0;
                CheckGame();
            }
        }

        if (lookForRequests) {
            requestTimer += Time.deltaTime;
            if (!checkingRequest && requestTimer >= 2.0f) {
                requestTimer = 0;
                CheckRequest();
            }
        }

        if (!checkingGame && gameModel != null) {
            if (updateGameState) {
                switch (gameModel.Status) {
                    case GameModel.WaitingForPrompts: {
                        if (gameModel.CurrentPlayer == Self) {
                            SoundManager.PlaySound("Match");
                            PanelController.ShowPrompts();
                            updateGameState = false;
                        } else PanelController.ShowWaitingPrompts();

                        break;
                    }
                    case GameModel.WaitingForAnswer: {
                        if (gameModel.CurrentPlayer != Self) {
                            SoundManager.PlaySound("Match");
                            PanelController.ShowAnswers(gameModel);
                            updateGameState = false;
                        } else PanelController.ShowWaitingAnswers();
                        break;
                    }
                    case GameModel.WaitingForNextGame: {
                        if (gameModel.Next[NextOther] == -1 && gameModel.Next[NextSelf] == 1) {
                            FinishGame();
                            PanelController.Close();
                        } else if (gameModel.Next[NextSelf] == 1) {
                            PanelController.ShowWaitingForNewGame();
                        } else {
                            SoundManager.PlaySound("Match");
                            PanelController.ShowResults(gameModel);
                            updateGameState = false;
                        }
                        break;
                    }
                }
            }
        }

        if (!checkingRequest && gameRequest != null) {
            if (gameRequest.Initiator == Self) {
                if (gameRequest.Status == GameRequestModel.Unconfirmed) {
                    MenuController.ShowState(1);
                } else {
                    ServerConnection.Instance.MakeRequestAsync("/game/acknowledge-request-status", Method.POST, new List<(string key, string value)> {("from", Self), ("to", Other)}, null);
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
        if (status == 0) MenuController.ShowState(2);
        else {
            SoundManager.PlaySound("Match");
            MenuController.Close();
            PanelController.Open();
            PanelController.ShowWaitingPrompts();
        }

        ServerConnection.Instance.MakeRequestAsync("/game/confirm-request", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other), ("status", status.ToString())
        }, response => {
            ServerConnection.Instance.MakeRequestAsync("/game/acknowledge-request-status", Method.POST, new List<(string key, string value)> {
                ("from", Self), ("to", Other)
            }, null);
        });
    }

    public void SubmitPrompts(List<string> prompts) {
        SoundManager.PlaySound("Match");
        PanelController.ShowWaitingAnswers();
        gameModel.Status = GameModel.WaitingForAnswer;
        ServerConnection.Instance.MakeRequestAsync("/game/add-prompts", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other), ("prompts", new JArray(prompts.ToArray()).ToString(Formatting.None))
        }, response => {
            updateGameState = true;
        });
    }

    public void SubmitAnswer(int answer) {
        SoundManager.PlaySound("Match");
        ServerConnection.Instance.MakeRequestAsync("/game/add-answer", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other), ("answer", answer.ToString())
        }, response => {
            updateGameState = true;
            if (!checkingGame)
                gameTimer = 10000f;
        });
    }

    public void PlayAgain() {
        gameModel.Next[NextSelf] = 1;
        PanelController.ShowWaitingForNewGame();
        ServerConnection.Instance.MakeRequestAsync("/game/new-game", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, response => {
            updateGameState = true;
        });
    }

    public void StopPlaying() {
        PanelController.Close();
        gameModel.Next[NextSelf] = -1;
        ServerConnection.Instance.MakeRequestAsync("/game/cancel-game", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, response => {
            if (gameModel.Next[NextOther] == -1) {
                ServerConnection.Instance.MakeRequestAsync("/game/finish-game", Method.POST, new List<(string key, string value)> {
                    ("from", Self), ("to", Other)
                }, null);
            }
        });
    }
    
    public void FinishGame() {
        gameModel.Next[NextSelf] = -1;
        PanelController.Close();
        ServerConnection.Instance.MakeRequestAsync("/game/finish-game", Method.POST, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, null);
    }

    private void CheckRequest() {
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

            gameRequest = GameRequestModel.Deserialize(JObject.Parse(response.Content));
            checkingRequest = false;
        });
    }

    private void CheckGame() {
        checkingGame = true;
        gameModel = null;
        ServerConnection.Instance.MakeRequestAsync("/game/game", Method.GET, new List<(string key, string value)> {
            ("from", Self), ("to", Other)
        }, response => {
            if (response.Content == "null") {
                gameModel = null;
                checkingGame = false;
                return;
            }

            gameModel = GameModel.Deserialize(JObject.Parse(response.Content));
            checkingGame = false;
        });
    }
}