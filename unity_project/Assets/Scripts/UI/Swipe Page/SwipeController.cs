using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityCommons;
using UnityEngine;
using UnityEngine.Events;

public class SwipeController : MonoBehaviour {
    [SerializeField] private List<GameObject> Builders;
    [SerializeField] private UnityEvent<UserModel> OnMatch;
    [SerializeField] private ScoresPageBuilder ScoresPageBuilder;
    [Space]
    [SerializeField] private GameObject SwipeContainer;
    [SerializeField] private GameObject Profile;
    [SerializeField] private GameObject NoPeople;
    [Space]
    [SerializeField] private Notification Notification;
    private readonly Dictionary<int, bool> parseCoroutines = new Dictionary<int, bool>();
    
    private float loadUsersTimer;
    private int currentUser = -1;
    private bool noUsers = true;
    private bool loadingUsers;

    private void Start() {
        ServerConnection.Instance.MakeRequestAsync("/users/swipes", Method.GET, new List<(string key, string value)> {("id", UserState.Instance.UserId)}, swipesResponse => {
            var swipes = JArray.Parse(swipesResponse.Content);
            foreach (var swipeToken in swipes) {
                AppState.Instance.SwipedUsers.Add(swipeToken.Value<string>("id"));
            }

            ServerConnection.Instance.MakeRequestAsync("/users/matches", Method.GET, new List<(string key, string value)> {("id", UserState.Instance.UserId)}, matchesResponse => {
                var matches = JArray.Parse(matchesResponse.Content);
                foreach (var matchToken in matches) {
                    AppState.Instance.Matches.Add(matchToken.Value<string>());
                }
                LoadUsers(true);
            });
        });
    }

    private void OnDoneLoading() {
        if (noUsers) {
            SwipeContainer.SetActive(true);
            Profile.SetActive(true);
            NoPeople.SetActive(false);
            
            noUsers = false;
            Next();
        }
        
        ServerConnection.Instance.MakeRequestAsync("/users/likes-count", Method.GET, new List<(string key, string value)> {("id", UserState.Instance.UserId)},
                                                   likesResponse => { ScoresPageBuilder.Build(int.Parse(likesResponse.Content)); });
    }

    public void OnSwipe(bool swipe) {
        var selfUser = UserState.Instance.UserId;
        var other = AppState.Instance.UserAccounts[currentUser].UserId;
        var swipeInt = swipe ? "1" : "0";

        if (selfUser == other) {
            currentUser++;
            if (currentUser >= AppState.Instance.UserAccounts.Count)
                ShowNoPeople();
            else
                LoadUser();
            return;
        }

        AppState.Instance.SwipedUsers.Add(other);
        ServerConnection.Instance.MakeRequestAsync("/users/swipe", Method.POST, new List<(string key, string value)> {
            ("from", selfUser),
            ("to", other),
            ("swipe", swipeInt)
        }, response => {
            var oldCurrentUser = currentUser;
            Next();

            if (swipe) {
                ServerConnection.Instance.MakeRequestAsync("/users/has-match", Method.GET, new List<(string key, string value)> {
                    ("from", selfUser),
                    ("to", other)
                }, response1 => {
                    var hasMatch = bool.Parse(response1.Content);

                    if (hasMatch) {
                        OnMatch?.Invoke(AppState.Instance.UserAccounts[oldCurrentUser].UserModel);

                        ServerConnection.Instance.MakeRequestAsync("/users/confirm-matches", Method.POST, new List<(string key, string value)> {
                            ("id", selfUser),
                            ("ids", $"[\"{other}\"]")
                        }, null);
                    }
                });
            }
        });
    }

    private void Next() {
        string userId = null;
        do {
            currentUser++;
            if (currentUser >= AppState.Instance.UserAccounts.Count) {
                ShowNoPeople();
                return;
            }

            userId = AppState.Instance.UserAccounts[currentUser].UserId;
        } while (userId == UserState.Instance.UserId || AppState.Instance.SwipedUsers.Contains(userId));

        LoadUser();
    }

    private void ShowNoPeople() {
        noUsers = true;
        SwipeContainer.SetActive(false);
        Profile.SetActive(false);
        NoPeople.SetActive(true);
    }

    private IEnumerator ParseAccount(int index, JToken account) {
        yield return null;
        if (account["profile"].ToString() == "{}") {
            parseCoroutines[index] = true;
            yield break;
        }

        var acc = AccountModel.Deserialize(account);
        parseCoroutines[index] = true;
        if (!AppState.Instance.UserAccountsDict.ContainsKey(acc.UserId)) {
            AppState.Instance.UserAccounts.Add(acc);
            AppState.Instance.UserAccountsDict[acc.UserId] = acc;
        }
    }

    private void LoadUser() {
        if (currentUser >= AppState.Instance.UserAccounts.Count) return;

        Builders.ForEach(@object => {
            var builders = @object.GetComponents<IBuilder>();
            foreach (var builder in builders) {
                builder.Cleanup();
                builder.Build(AppState.Instance.UserAccounts[currentUser].UserModel);
            }
        });
    }

    public void LoadUsers(bool initial = false) {
        if(loadingUsers) return;
        
        loadingUsers = true;
        loadUsersTimer = 0;
        ServerConnection.Instance.MakeRequestAsync("/users/unconfirmed-matches", Method.GET, new List<(string key, string value)> {("id", UserState.Instance.UserId)},
        matchesResponse => {
               var matches = JArray.Parse(matchesResponse.Content);
               if(matches.Count > 0)
                    Notification.ShowNotification("You have new matches!");
               foreach (var matchToken in matches) {
                   AppState.Instance.Matches.Add(matchToken.Value<string>());
               }
               ServerConnection.Instance.MakeRequestAsync("/users/confirm-matches", Method.POST,
                                                          new List<(string key, string value)>
                                                              {("id", UserState.Instance.UserId), ("ids", matchesResponse.Content)},
                                                          confirmMatches => {
                                                              
       
                ServerConnection.Instance.MakeRequestAsync("/users", Method.GET, new List<(string key, string value)>(), response => {
                    var usersArray = JArray.Parse(response.Content);
                    for (var index = 0; index < usersArray.Count; index++) {
                        var account = usersArray[index];
                        parseCoroutines[index] = false;
                        StartCoroutine(ParseAccount(index, account));
                    }

                    IDisposable cancelToken = null;
                    cancelToken = UpdateUtility.Create(() => {
                        if (!parseCoroutines.All(pair => pair.Value))
                            return;

                        parseCoroutines.Clear();
                        OnDoneLoading();
                        loadingUsers = false;
                        if (initial) {
                            AppState.Instance.DoneLoadingInitial = true;
                        }

                        cancelToken?.Dispose();
                    });
                });
            });
        });
    }

    private void Update() {
        loadUsersTimer += Time.deltaTime;
        if (loadUsersTimer >= 5f) {
            LoadUsers();
        }
    }
}