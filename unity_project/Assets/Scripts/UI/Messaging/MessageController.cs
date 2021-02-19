using System;
using System.Collections;
using TMPro;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {
    [SerializeField] private RectTransform MessageContainer;
    [SerializeField] private ScrollRect ScrollRect;
    [SerializeField] private TMP_InputField MessageSelfPrefab;
    [SerializeField] private TMP_InputField MessageOtherPrefab;

    private bool newMessageScroll;
    
    public void OnMessageSelf(string message) {
        AddMessage(MessageSelfPrefab, message);
    }

    public void OnMessageOther(string message) {
        AddMessage(MessageOtherPrefab, message);
    }

    public void Clear() {
        while (MessageContainer.childCount > 0) {
            var child = MessageContainer.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    private void AddMessage(TMP_InputField prefab, string text) {
        SoundManager.PlaySound("Message");
        var messageObject = Instantiate(prefab, MessageContainer);
        messageObject.text = text;
        IDisposable cancel = null;
        cancel = UpdateUtility.Create(() => {
            var caret = messageObject.gameObject.GetComponentInChildren<TMP_SelectionCaret>(true);
            if (caret != null) {
                caret.enabled = false;
                cancel.Dispose();
            }
        });
        StartCoroutine(SkipFrames(2, () => {
            newMessageScroll = true;
        }));
    }

    private IEnumerator SkipFrames(int frames, Action action) {
        for (var i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
        }

        action?.Invoke();
    }

    private void LateUpdate() {
        if (newMessageScroll) {
            ScrollRect.normalizedPosition = Vector2.zero;
            newMessageScroll = false;
        }
    }
}
