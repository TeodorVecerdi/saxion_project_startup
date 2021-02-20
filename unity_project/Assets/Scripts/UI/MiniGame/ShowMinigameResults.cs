using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowMinigameResults : MonoBehaviour {
    [SerializeField] private List<Graphic> Backgrounds;
    [SerializeField] private List<TextMeshProUGUI> Texts;
    [SerializeField] private List<TextMeshProUGUI> Prompts;
    [Space]
    [SerializeField] private Color NormalColor;
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color CorrectColor;
    [Space]
    [SerializeField] private Button NextButton;
    [SerializeField] private float AnimationDuration = 0.5f;
    [SerializeField] private float InitialHoldDuration = 1.0f;
    [SerializeField] private float HoldDuration = 2.5f;

    public void Load(bool self, int lieIdx, int selectedIdx, List<string> texts) {
        NextButton.enabled = false;
        for (var i = 0; i < 3; i++) {
            Texts[i].text = texts[i];
        }

        var isCorrect = lieIdx == selectedIdx;
        var liePromptCorrect = $"And {(self ? "you" : "they")} were right!";
        var liePrompt = isCorrect ? liePromptCorrect : "But the lie was";
        var selectedPrompt = $"{(self ? "You" : "The other player")} chose";

        Backgrounds[selectedIdx].DOColor(SelectedColor, AnimationDuration).SetDelay(InitialHoldDuration);
        Prompts[selectedIdx].text = selectedPrompt;
        Prompts[selectedIdx].DOFade(1f, AnimationDuration).From(0f).SetDelay(InitialHoldDuration+AnimationDuration);
        Backgrounds[lieIdx].DOColor(CorrectColor, AnimationDuration).SetDelay(InitialHoldDuration + HoldDuration + AnimationDuration).OnComplete(() => {
            Prompts[lieIdx].text = liePrompt;
            NextButton.enabled = true;
        });
        if (!isCorrect) Prompts[lieIdx].DOFade(1f, AnimationDuration).From(0f).SetDelay(InitialHoldDuration + HoldDuration + AnimationDuration + AnimationDuration);
    }
}

