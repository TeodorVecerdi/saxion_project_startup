using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using RestSharp;
using TMPro;
using UnityEngine;

public class TestGetUsers : MonoBehaviour {
    public TestGameListBuilder BuilderA;
    public TestGameListBuilder2 BuilderB;
    public TextMeshProUGUI About;
    
    private JArray usersArray;

    private void Start() {
        ServerConnection.Instance.MakeRequestAsync("/users", Method.GET, new List<(string key, string value)>(), response => {
            usersArray = JArray.Parse(response.Content);
            Debug.Log(response.Content);

            LoadUser();
        });
    }

    [Button]
    public void GetUsers() {
        ServerConnection.Instance.MakeRequestAsync("/users", Method.GET, new List<(string key, string value)>(), response => {
            usersArray = JArray.Parse(response.Content);
            Debug.Log(response.Content);

            LoadUser();
        });
    }

    [Button]
    public void DeserializeUser() {
        var user0 = usersArray[0];
        var userModel = UserModel.Deserialize(user0["profile"]);
        Debug.Log(userModel.GenrePreferences);
        Debug.Log(userModel.PlayedGames);
        Debug.Log(userModel.RelationshipPreference);
        Debug.Log(userModel.GenderPreference);
        // var serialized = userModel.Serialize();
        // Debug.Log(serialized);
    }

    private void LoadUser() {
        var user0 = usersArray[0];
        var userModel = UserModel.Deserialize(user0["profile"]);
        BuilderA.Build(userModel);
        BuilderB.Build(userModel);
        About.text = userModel.About;
    }
}