using System;
using Newtonsoft.Json.Linq;

[Serializable]
public class AccountModel {
    public string UserId;
    public UserModel UserModel;
    
    private AccountModel() {
        
    }

    public static AccountModel Deserialize(JToken accountJson) {
        var account = new AccountModel {
            UserId = accountJson.Value<string>("id"),
            UserModel = UserModel.Deserialize(accountJson["profile"])
        };
        return account;
    }
}