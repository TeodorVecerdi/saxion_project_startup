using UnityEngine;
using UnityEngine.UI;

public class ScoresPageBuilder : MonoBehaviour {
    [SerializeField] private BlurredAvatar BlurredAvatarPrefab;
    [SerializeField] private RectTransform BlurredAvatarContainer;
    [SerializeField] private GameObject BlurredAvatarScroll;
    [SerializeField] private GameObject BlurredAvatarEmpty;
    [SerializeField] private Button BlurredAvatarButton;
    [Space]
    [SerializeField] private MessageEntry MessageEntryPrefab;
    [SerializeField] private RectTransform MessageEntryContainer;
    [SerializeField] private GameObject MessageEntryScroll;
    [SerializeField] private GameObject MessageEntryEmpty;
    [Space]
    [SerializeField] private ChatManager ChatManager;

    public void Build(int likeCount) {
        // Cleanup
        while (BlurredAvatarContainer.childCount > 0) {
            var child = BlurredAvatarContainer.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        while (MessageEntryContainer.childCount > 0) {
            var child = MessageEntryContainer.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        
        if (likeCount == 0) {
            BlurredAvatarScroll.SetActive(false);
            BlurredAvatarEmpty.SetActive(true);
            BlurredAvatarButton.enabled = false;
        } else {
            BlurredAvatarScroll.SetActive(true);
            BlurredAvatarEmpty.SetActive(false);
            BlurredAvatarButton.enabled = true;
            
            for (var i = 0; i < likeCount; i++) {
                var blurredAvatar = Instantiate(BlurredAvatarPrefab, BlurredAvatarContainer);
                var backgroundRandomizer = blurredAvatar.GetComponent<ColorRandomizer>();
                backgroundRandomizer.Seed = i;
                blurredAvatar.Seed = i;
            }    
        }

        if (AppState.Instance.Matches.Count == 0) {
            MessageEntryScroll.SetActive(false);
            MessageEntryEmpty.SetActive(true);
        } else {
            MessageEntryScroll.SetActive(true);
            MessageEntryEmpty.SetActive(false);

            
            foreach (var matchId in AppState.Instance.Matches) {
                var messageEntry = Instantiate(MessageEntryPrefab, MessageEntryContainer);
                messageEntry.Build(AppState.Instance.UserAccountsDict[matchId]);
                var button = messageEntry.GetComponent<Button>();
                button.onClick.AddListener(() => OpenChat(matchId));
            }
        }
    }

    private void OpenChat(string userId) {
        AppState.Instance.ChattingWith = userId;
        ChatManager.Open();
    }
}

