using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinigameAnswersController : MonoBehaviour {
    [SerializeField] private MinigameManager Manager;
    [SerializeField] private List<TextMeshProUGUI> Texts;

    private List<int> originalIndices;

    public void Load(List<string> prompts, List<int> originalIndices) {
        this.originalIndices = originalIndices;
        for (var i = 0; i < 3; i++) {
            Texts[i].text = prompts[i];
        }
    }

    public void Select(int index) {
        var realIndex = originalIndices[index];
        Manager.SubmitAnswer(realIndex);
    }
}

