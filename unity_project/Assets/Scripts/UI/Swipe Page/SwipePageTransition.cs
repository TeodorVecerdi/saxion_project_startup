using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipePageTransition : MonoBehaviour {
    [SerializeField] private Button OpenProfileButton;
    [SerializeField] private Button CloseProfileButton;
    [SerializeField] private RectTransform SwipeContainer;
    [SerializeField] private RectTransform ProfileContainer;
    [Space]
    [SerializeField] private TextMeshProUGUI ProfileGamesText;
    [SerializeField] private RectTransform ProfileGamesList;
    [SerializeField] private RectTransform ProfileGamesListButton;
    [Space]
    [SerializeField] private float AnimationDurationProfileContainer = 0.75f;
    [SerializeField] private float AnimationDurationSwipeContainer = 0.5f;
    [SerializeField] private float AnimationDurationGamesListButton = 0.25f;
    [Space]
    [SerializeField] private Button GamesListButton;

    public void OpenProfile() {
        ChangeProfile_Buttons(true);
        ChangeProfile_GameList(true);
        ChangeProfile_ContainerPosition(true);
        Debug.Log("Opened profile");
    }

    public void CloseProfile() {
        ChangeProfile_Buttons(false);
        ChangeProfile_GameList(false);
        ChangeProfile_ContainerPosition(false);
        Debug.Log("Closed profile");
    }

    private void ChangeProfile_Buttons(bool open) {
        OpenProfileButton.gameObject.SetActive(!open);
        CloseProfileButton.gameObject.SetActive(open);
    }

    private void ChangeProfile_GameList(bool open) {
        if (open) {
            ProfileGamesText.DOFade(1.0f, AnimationDurationGamesListButton).From(0f);
            ProfileGamesList.DOSizeDelta(new Vector2(-64, -160), AnimationDurationGamesListButton).From(Vector2.one * -64.0f);
            ProfileGamesList.DOAnchorPos(new Vector2(0, -48), AnimationDurationGamesListButton).From(Vector2.zero);
            ProfileGamesListButton.DOSizeDelta(new Vector2(-60, -156), AnimationDurationGamesListButton).From(Vector2.one * -60.0f);
            ProfileGamesListButton.DOAnchorPos(new Vector2(0, -48), AnimationDurationGamesListButton).From(Vector2.zero);
        } else {
            ProfileGamesText.DOFade(0.0f, AnimationDurationGamesListButton).From(1f);
            ProfileGamesList.DOSizeDelta(Vector2.one * -64.0f, AnimationDurationGamesListButton).From(new Vector2(-64, -160));
            ProfileGamesList.DOAnchorPos(Vector2.zero, AnimationDurationGamesListButton).From(new Vector2(0, -48));
            ProfileGamesListButton.DOSizeDelta(Vector2.one * -60.0f, AnimationDurationGamesListButton).From(new Vector2(-60, -156));
            ProfileGamesListButton.DOAnchorPos(Vector2.zero, AnimationDurationGamesListButton).From(new Vector2(0, -48));
        }
    }

    private void ChangeProfile_ContainerPosition(bool open) {
        if (open) {
            SwipeContainer.DOAnchorPosY(2161.0f, AnimationDurationSwipeContainer).From(new Vector2(0, 64f));
            ProfileContainer.DOAnchorPosY(0, AnimationDurationProfileContainer).From(new Vector2(0, -1629.0f)).OnComplete(() => {
                GamesListButton.interactable = true;
            });
        } else {
            GamesListButton.interactable = false;
            SwipeContainer.DOAnchorPosY(64f, AnimationDurationSwipeContainer).From(new Vector2(0, 2161.0f));
            ProfileContainer.DOAnchorPosY(-1629.0f, AnimationDurationProfileContainer).From(Vector2.zero);
        }
    }
}