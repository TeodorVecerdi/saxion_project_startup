using DG.Tweening;
using TMPro;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoSingleton<Popup> {
    [SerializeField] private Graphic Background;
    [SerializeField] private RectTransform PopupContainer;
    [SerializeField] private TextMeshProUGUI Text;

    private void ShowPopup(string feature) {
        Background.gameObject.SetActive(true);
        Background.DOFade(0.9f, 0.15f).From(0f);
        Text.text = $"<b>{feature}</b> and many other features are available to our <b><color=#FECF08>ScoreMe Gold</color></b> members for just <color=#65ed13>$3<b><sup>99</sup></b></color> per month!";
        PopupContainer.DOScale(1.1f, 0.25f).From(0f);
        PopupContainer.DOScale(1f, 0.1f).From(1.1f).SetDelay(0.25f);
    }

    public void Close() {
        Background.DOFade(0f, 0.15f).From(0.9f).OnComplete(() => {
            Background.gameObject.SetActive(false);
        });
        PopupContainer.DOScale(0f, 0.25f).From(1f);
    }

    public static void Show(string feature) {
        Instance.ShowPopup(feature);
    }
    
}
