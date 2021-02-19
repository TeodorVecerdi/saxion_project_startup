using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginTransition : MonoBehaviour {
    [SerializeField] private Image SpinnerImage;
    [SerializeField] private TextMeshProUGUI LoadingText;
    [SerializeField] private GameObject TransitionContainer;
    [SerializeField] private TextMeshProUGUI PleaseWaitText;
    [SerializeField] private GameObject LoginContainer;
    [SerializeField] private GameObject HeaderContainer;

    private RectTransform spinnerTransform;
    private RectTransform textTransform;
    public bool StartWaitTimer;
    public Action OnLoadComplete;

    private float spinnerAlpha = 0.0f;
    private bool animateSpinnerAlpha;
    private static readonly int alpha = Shader.PropertyToID("_Alpha");
    private float waitTimer;

    public void Activate() {
        spinnerTransform = SpinnerImage.GetComponent<RectTransform>();
        textTransform = LoadingText.GetComponent<RectTransform>();

        LoadingText.DOFade(1.0f, 1.0f).SetDelay(1.0f);
        textTransform.DOAnchorPosY(384f, 0.25f).SetDelay(3.5f);
        PleaseWaitText.DOFade(1.0f, 0.25f).SetDelay(3.5f);
        DOTween.To(() => spinnerAlpha, value => spinnerAlpha = value, 1.0f, 1.0f).From(0.0f).OnComplete(() => {
            animateSpinnerAlpha = false;
        }).SetDelay(3.5f);
        spinnerTransform.localScale = Vector3.zero;
        spinnerTransform.DOScale(1.0f, 0.25f).From(0.0f).SetDelay(3.5f);
        animateSpinnerAlpha = true;
        TransitionContainer.SetActive(true);
        LoginContainer.SetActive(false);
        HeaderContainer.SetActive(false);
    }

    private void Update() {
        if (animateSpinnerAlpha) {
            SpinnerImage.material.SetFloat(alpha, spinnerAlpha);
            SpinnerImage.enabled = false;
            SpinnerImage.enabled = true;
        }

        if (StartWaitTimer) {
            waitTimer += Time.deltaTime;
            if (waitTimer >= 6.0f) {
                waitTimer = 0f;
                StartWaitTimer = false;
                OnLoadComplete?.Invoke();
            }
        }
    }
}