using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityCommons;
using UnityEngine;
using UnityEngine.Events;

public class SwipeController : MonoBehaviour {
    [SerializeField] private List<GameObject> Builders;
    [SerializeField] private UnityEvent<UserModel> OnMatch;
    private List<AccountModel> allUsers = new List<AccountModel>();
    private Dictionary<int, bool> parseCoroutines = new Dictionary<int, bool>();

    private int currentUser;

    private void Start() {
        ServerConnection.Instance.MakeRequestAsync("/users", Method.GET, new List<(string key, string value)>(), response => {
            allUsers.Clear();
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

                // ReSharper disable once AccessToModifiedClosure
                // It's supposed to be this way ^
                cancelToken?.Dispose();
                Initialize();
            });
        });
    }

    public void OnSwipe(bool swipe) {
        var selfUser = UserState.Instance.UserId;
        var other = allUsers[currentUser].UserId;
        var swipeInt = swipe ? "1" : "0";

        if (selfUser == other) {
            currentUser++;
            if (currentUser >= allUsers.Count) currentUser = 0;
            LoadUser();
            Debug.Log("Skipped swipe");
            return;
        }
        
        ServerConnection.Instance.MakeRequestAsync("/users/swipe", Method.POST, new List<(string key, string value)> {
            ("from", selfUser),
            ("to", other),
            ("swipe", swipeInt)
        }, response => {
            var oldCurrentUser = currentUser;
            currentUser++;
            if (currentUser >= allUsers.Count) currentUser = 0;
            LoadUser();

            ServerConnection.Instance.MakeRequestAsync("/users/has-match", Method.GET, new List<(string key, string value)> {
                ("from", selfUser),
                ("to", other)
            }, response1 => {
                var hasMatch = bool.Parse(response1.Content);
                if(hasMatch) OnMatch?.Invoke(allUsers[oldCurrentUser].UserModel);
            });
        });
    }

    private void Initialize() {
        currentUser = 0;
        LoadUser();
    }

    private IEnumerator ParseAccount(int index, JToken account) {
        yield return null;
        if (account["profile"].ToString() == "{}") {
            parseCoroutines[index] = true;
            yield break;
        }
        var acc = AccountModel.Deserialize(account);
        parseCoroutines[index] = true;
        allUsers.Add(acc);
    }

    private void LoadUser() {
        if(currentUser >= allUsers.Count) return;
        
        Builders.ForEach(@object => {
            var builders = @object.GetComponents<IBuilder>();
            foreach (var builder in builders) {
                builder.Cleanup();
                builder.Build(allUsers[currentUser].UserModel);
            }
        });
    }
}