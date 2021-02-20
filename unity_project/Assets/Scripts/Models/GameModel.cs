using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class GameModel {
    public const int WaitingForPrompts = 0;
    public const int WaitingForAnswer = 1;
    public const int WaitingForNextGame = 2;
    
    public string Initiator;
    public string Other;
    public string CurrentPlayer;
    public int Status;
    public List<string> Prompts;
    public List<int> Next;
    public int Answer;

    public static GameModel Deserialize(JToken gameToken) {
        var prompts = gameToken["prompts"].Values<string>().ToList();
        var next = gameToken["next"].Values<int>().ToList();
        return new GameModel {
            Initiator = gameToken.Value<string>("initiator"),
            Other = gameToken.Value<string>("other"),
            CurrentPlayer = gameToken.Value<string>("currentPlayer"),
            Status = gameToken.Value<int>("status"),
            Prompts = prompts,
            Next = next,
            Answer = gameToken.Value<int>("answer")
        };
    }

    public static string Serialize(GameModel gameModel) {
        return new JObject {
            ["initiator"] = gameModel.Initiator,
            ["other"] = gameModel.Other,
            ["currentPlayer"] = gameModel.CurrentPlayer,
            ["status"] = gameModel.Status.ToString(),
            ["prompts"] = new JArray(gameModel.Prompts.Cast<object>().ToArray()),
            ["answer"] = gameModel.Answer.ToString()
        }.ToString(Formatting.None);
    }
}