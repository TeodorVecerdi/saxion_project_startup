using DG.Tweening;
using TMPro;
using UnityEngine;

public class LoginRegisterTransition : MonoBehaviour {
    [SerializeField] private float TransitionPosition = 1300;
    [SerializeField] private float TransitionDuration = 0.5f;
    [SerializeField] private RectTransform LoginTransform;
    [SerializeField] private RectTransform RegisterTransform;
    [Space]
    [SerializeField] private TMP_InputField LoginUsernameText;
    [SerializeField] private TMP_InputField LoginPasswordText;
    [SerializeField] private TMP_InputField RegisterUsernameText;
    [SerializeField] private TMP_InputField RegisterPasswordText;
    [SerializeField] private TMP_InputField RegisterRepeatPasswordText;
    [Space]
    [SerializeField] private FadeOut RegisterSuccessFade;

    public void ShowLogin() {
        LoginTransform.gameObject.SetActive(true);
        LoginTransform.DOAnchorPosX(0, TransitionDuration).From(new Vector2(TransitionPosition, 0));
        RegisterTransform.DOAnchorPosX(-TransitionPosition, TransitionDuration).From(new Vector2(0, 0)).OnComplete(() => {
            RegisterUsernameText.text = "";
            RegisterPasswordText.text = "";
            RegisterRepeatPasswordText.text = "";
            RegisterTransform.gameObject.SetActive(false);
        });
    }

    public void ShowRegister() {
        RegisterTransform.gameObject.SetActive(true);
        RegisterTransform.DOAnchorPosX(0, TransitionDuration).From(new Vector2(-TransitionPosition, 0));
        LoginTransform.DOAnchorPosX(TransitionPosition, TransitionDuration).From(new Vector2(0, 0)).OnComplete(() => {
            LoginUsernameText.text = "";
            LoginPasswordText.text = "";
            LoginTransform.gameObject.SetActive(false);
        });
    }

    public void ShowSuccess() {
        RegisterSuccessFade.Fade(false);
        var test = 0f;
        DOTween.To(() => test, value => test = value, 1.0f, 0.5f).From(0f).OnComplete(ShowLogin);
        DOTween.To(() => test, value => test = value, 1.0f, 2.5f).SetDelay(0.5f).From(0f).OnComplete(() => {
            RegisterSuccessFade.Fade(true);
        });
    }
}