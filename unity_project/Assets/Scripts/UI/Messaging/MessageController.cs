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
    
    public void OnMessageSelf(string message) {
        AddMessage(MessageSelfPrefab, message);
    }

    public void OnMessageOther(string message) {
        AddMessage(MessageOtherPrefab, message);
    }

    private void AddMessage(TMP_InputField prefab, string text) {
        StartCoroutine(SpawnMessage(prefab, text));
    }

    private IEnumerator SpawnMessage(TMP_InputField prefab, string message) {
        var messageObject = Instantiate(prefab, MessageContainer);
        messageObject.text = message;
        IDisposable cancel = null;
        cancel = UpdateUtility.Create(() => {
            var caret = messageObject.gameObject.GetComponentInChildren<TMP_SelectionCaret>(true);
            if (caret != null) {
                caret.enabled = false;
                cancel.Dispose();
            }
        });
        yield return null;
        ScrollRect.normalizedPosition = Vector2.zero;
    }
}
