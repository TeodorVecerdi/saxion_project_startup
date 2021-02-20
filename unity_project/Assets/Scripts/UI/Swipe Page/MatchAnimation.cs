using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MatchAnimation : MonoBehaviour {
    [SerializeField] private FadeOut NotificationFader;
    [SerializeField] private FadeOut AnimationFader;
    [SerializeField] private RectTransform AvatarTransform;
    [SerializeField] private Graphic Background;
    [SerializeField] private float AnimationDurationOut = 0.5f;
    [SerializeField] private float AnimationDurationIn = 0.25f;
    [SerializeField] private List<GameObject> Builders;
    [SerializeField] private Button HideButton;

    public void Match(UserModel model) {
        foreach (var builder in Builders) {
            var builderComps = builder.GetComponents<IBuilder>();
            foreach (var builderComp in builderComps) {
                builderComp.Cleanup();
                builderComp.Build(model);
            }
        }

        HideButton.enabled = true;
        NotificationFader.Reset(false);
        gameObject.SetActive(true);
        Show();
    }

    private void Show() {
        Background.DOFade(0.9f, AnimationDurationIn).From(0f);
        AvatarTransform.DOScale(1f, AnimationDurationIn).From(0f);
    }

    public void Hide() {
        NotificationFader.Fade(true);
        AnimationFader.Fade(false);
        HideButton.enabled = false;
    }

    public void SecondAnimation() {
        var rectTransform = AnimationFader.GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(-207, -276), AnimationDurationOut).From(new Vector2(-642.5f, -802.061f));
        rectTransform.DOScale(0, AnimationDurationOut-0.1f).From(1f).SetDelay(0.1f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}

