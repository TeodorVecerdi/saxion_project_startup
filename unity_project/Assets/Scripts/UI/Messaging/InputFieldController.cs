using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldController : MonoBehaviour {
    [SerializeField] private RectTransform SendButton;
    [SerializeField] private RectTransform MinigamesContainer;
    [SerializeField] private TMP_InputField Input;
    [SerializeField] private ChatManager ChatManager;
    private bool sendVisible = false;


    public void OnTextChanged(string newText) {
        if (newText.Length > 0 && newText[newText.Length-1] == '\n') {
            EmitMessage();
            return;
        }
        if (string.IsNullOrWhiteSpace(newText)) {
            if(sendVisible)ShowMinigames();
            
        } else {
            if(!sendVisible) ShowSendButton();
        }
    }

    private void ShowMinigames() {
        MinigamesContainer.DOScale(1f, 0.15f).From(0f);
        SendButton.DOScale(0f, 0.15f).From(1f);
        sendVisible = false;
    }

    private void ShowSendButton() {
        MinigamesContainer.DOScale(0f, 0.15f).From(1f);
        SendButton.DOScale(1f, 0.15f).From(0f);
        sendVisible = true;
    }

    public void EmitMessage() {
        var message = Input.text.Trim();
        
        Input.text = "";
        Input.OnPointerClick(new PointerEventData(null));

        if(!string.IsNullOrWhiteSpace(message))
            ChatManager.EmitMessage(message);
    }
}

