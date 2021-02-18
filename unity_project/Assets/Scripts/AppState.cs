using System.Collections.Generic;
using UnityCommons;

public class AppState : MonoSingleton<AppState> {
    public HashSet<string> SwipedUsers = new HashSet<string>();
    public HashSet<string> Matches = new HashSet<string>();
    public List<AccountModel> UserAccounts = new List<AccountModel>();
    public Dictionary<string, AccountModel> UserAccountsDict = new Dictionary<string, AccountModel>();
    public string ChattingWith;
}