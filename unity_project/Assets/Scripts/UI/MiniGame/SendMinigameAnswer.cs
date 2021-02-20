using UnityEngine;

public class SendMinigameAnswer : MonoBehaviour {
    [SerializeField] private ToggleGroup ToggleGroup;
    [SerializeField] private MinigameAnswersController AnswersController;

    public void OnClick() {
        var selectedIndex = ToggleGroup.GetSelectedIndex();
        AnswersController.Select(selectedIndex);
    }
}