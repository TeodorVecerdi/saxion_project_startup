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

public class SwipePageBuilder : MonoBehaviour {
    [SerializeField] private List<GameObject> Builders;
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
        currentUser++;
        if (currentUser >= allUsers.Count) currentUser = 0;
        LoadUser();
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