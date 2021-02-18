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

    public void Build(int likeCount) {
        if (likeCount == 0) {
            BlurredAvatarScroll.SetActive(false);
            BlurredAvatarEmpty.SetActive(true);
            BlurredAvatarButton.enabled = false;
        } else {
            BlurredAvatarScroll.SetActive(true);
            BlurredAvatarEmpty.SetActive(false);
            BlurredAvatarButton.enabled = true;
            
            for (var i = 0; i < likeCount; i++) {
                Instantiate(BlurredAvatarPrefab, BlurredAvatarContainer);
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
            }
        }

    }
}

