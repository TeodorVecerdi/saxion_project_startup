using UnityEngine;

public class MessageEntry : MonoBehaviour {
    [SerializeField] private AvatarBuilder AvatarBuilder;
    [SerializeField] private UserInfoBuilder UserInfoBuilder;

    private string userId;

    public void Build(AccountModel accountModel) {
        userId = accountModel.UserId;
        AvatarBuilder.Cleanup();
        AvatarBuilder.Build(accountModel.UserModel);
        UserInfoBuilder.Cleanup();
        UserInfoBuilder.Build(accountModel.UserModel);
    }

    public void OpenChat() {
        // TODO!: Open chat with UserId
        Debug.Log($"Opening chat with user [{userId}]");
    }
}
