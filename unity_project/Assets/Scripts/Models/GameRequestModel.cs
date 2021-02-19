using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class GameRequestModel {
    public const int Unconfirmed = -1;
    public const int Denied = 0;
    public const int Accepted = 1;
    
    public string Initiator;
    public string Other;
    public int Status;
    
    public static GameRequestModel Deserialize(JToken requestToken) {
        return new GameRequestModel {
            Initiator = requestToken.Value<string>("initiator"),
            Other = requestToken.Value<string>("other"),
            Status = int.Parse(requestToken.Value<string>("status"))
        };
    }

    public static string Serialize(GameRequestModel requestModel) {
        return new JObject {
            ["initiator"] = requestModel.Initiator,
            ["other"] = requestModel.Other,
            ["status"] = requestModel.Status.ToString()
        }.ToString(Formatting.None);
    }
}