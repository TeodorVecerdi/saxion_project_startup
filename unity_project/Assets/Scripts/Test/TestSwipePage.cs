using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSwipePage : MonoBehaviour {
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

    private Vector2 gamesListStartSizeDelta = Vector2.one * -64.0f;
    private Vector2 gamesListStartAnchored = Vector2.zero;
    private Vector2 gamesListEndSizeDelta = new Vector2(-64, -160);
    private Vector2 gamesListEndAnchored = new Vector2(0, -48);

    private Vector2 gamesListButtonStartSizeDelta = Vector2.one * -60.0f;
    private Vector2 gamesListButtonStartAnchored = Vector2.zero;
    private Vector2 gamesListButtonEndSizeDelta = new Vector2(-60, -156);
    private Vector2 gamesListButtonEndAnchored = new Vector2(0, -48);
    
    private float profilePosition = -1629.0f;
    private float swipeStartPosition = 64f;
    private float swipeEndPosition = 2161.0f;

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
            ProfileGamesList.DOSizeDelta(gamesListEndSizeDelta, AnimationDurationGamesListButton).From(gamesListStartSizeDelta);
            ProfileGamesList.DOAnchorPos(gamesListEndAnchored, AnimationDurationGamesListButton).From(gamesListStartAnchored);
            ProfileGamesListButton.DOSizeDelta(gamesListButtonEndSizeDelta, AnimationDurationGamesListButton).From(gamesListButtonStartSizeDelta);
            ProfileGamesListButton.DOAnchorPos(gamesListButtonEndAnchored, AnimationDurationGamesListButton).From(gamesListButtonStartAnchored);
        } else {
            ProfileGamesText.DOFade(0.0f, AnimationDurationGamesListButton).From(1f);
            ProfileGamesList.DOSizeDelta(gamesListStartSizeDelta, AnimationDurationGamesListButton).From(gamesListEndSizeDelta);
            ProfileGamesList.DOAnchorPos(gamesListStartAnchored, AnimationDurationGamesListButton).From(gamesListEndAnchored);
            ProfileGamesListButton.DOSizeDelta(gamesListButtonStartSizeDelta, AnimationDurationGamesListButton).From(gamesListButtonEndSizeDelta);
            ProfileGamesListButton.DOAnchorPos(gamesListButtonStartAnchored, AnimationDurationGamesListButton).From(gamesListButtonEndAnchored);
        }
    }

    private void ChangeProfile_ContainerPosition(bool open) {
        if (open) {
            SwipeContainer.DOAnchorPosY(swipeEndPosition, AnimationDurationSwipeContainer).From(new Vector2(0, swipeStartPosition));
            ProfileContainer.DOAnchorPosY(0, AnimationDurationProfileContainer).From(new Vector2(0, profilePosition)).OnComplete(() => {
                GamesListButton.interactable = true;
            });
        } else {
            GamesListButton.interactable = false;
            SwipeContainer.DOAnchorPosY(swipeStartPosition, AnimationDurationSwipeContainer).From(new Vector2(0, swipeEndPosition));
            ProfileContainer.DOAnchorPosY(profilePosition, AnimationDurationProfileContainer).From(Vector2.zero);
        }
    }
}