using DG.Tweening;
using UnityEngine;

public class ProfileTransition : MonoBehaviour {
    [SerializeField] private RectTransform Profile;
    [SerializeField] private RectTransform GameList;
    [SerializeField] private float AnimationDuration = 0.5f;

    public void ShowProfile() {
        Profile.gameObject.SetActive(true);
        Profile.DOAnchorPosX(0, AnimationDuration).From(new Vector2(-1170, -96)).OnComplete(() => {
            GameList.gameObject.SetActive(false);
        });
        GameList.DOAnchorPosX(1170, AnimationDuration).From(new Vector2(0, -96));
    }

    public void ShowGameList() {
        GameList.gameObject.SetActive(true);
        GameList.DOAnchorPosX(0, AnimationDuration).From(new Vector2(1170, -96)).OnComplete(() => {
            Profile.gameObject.SetActive(false);
        });
        Profile.DOAnchorPosX(-1170, AnimationDuration).From(new Vector2(0, -96));
    }
}

