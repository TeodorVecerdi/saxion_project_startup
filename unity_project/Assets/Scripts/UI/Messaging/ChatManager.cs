using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using UnityCommons;
using UnityEngine;

public class ChatManager : MonoBehaviour {
    [SerializeField] private Slide ScoresSlide;
    [SerializeField] private Slide ChatSlide;
    [SerializeField] private UserInfoBuilder UserInfoBuilder;
    [SerializeField] private MessageController MessageController;

    private Queue<MessageModel> messageQueue = new Queue<MessageModel>();
    
    private bool isChatOpen;
    private bool pollMessages;
    private bool queueingMessages;
    private float chatPollTimer;

    public void Open() {
        ChatSlide.SetActive(true);
        ScoresSlide.SetActive(false);

        var chattingWith = AppState.Instance.UserAccountsDict[AppState.Instance.ChattingWith];
        UserInfoBuilder.Cleanup();
        UserInfoBuilder.Build(chattingWith.UserModel);
        isChatOpen = true;
        pollMessages = true;
    }

    public void Close() {
        ChatSlide.SetActive(false, true);
        ScoresSlide.SetActive(true, true);
        messageQueue.Clear();
    }

    public void EmitMessage(string message) {
        MessageController.OnMessageSelf(message);
        var from = UserState.Instance.UserId;
        var to = AppState.Instance.ChattingWith;
        ServerConnection.Instance.MakeRequestAsync("/chat/message", Method.POST, new List<(string key, string value)> {("from", from), ("to", to), ("message", message)}, null);
    }

    private void Update() {
        if (!queueingMessages && messageQueue.Count > 0) {
            AddMessage(messageQueue.Dequeue());
        }
        
        if(!isChatOpen || !pollMessages) return;
        chatPollTimer += Time.deltaTime;
        if (chatPollTimer >= 1.0f) {
            pollMessages = false;
            PollMessages();
            chatPollTimer -= 1.0f;
        }
    }

    private void AddMessage(MessageModel model) {
        var isSelf = model.From == UserState.Instance.UserId;
        if (isSelf) MessageController.OnMessageSelf(model.Message);
        else MessageController.OnMessageOther(model.Message);
    }

    private void PollMessages() {
        var from = UserState.Instance.UserId;
        var to = AppState.Instance.ChattingWith;
        queueingMessages = true;
        ServerConnection.Instance.MakeRequestAsync("/chat/unconfirmed-messages", Method.GET, new List<(string key, string value)> {("from", from), ("to", to)}, response => {
            var array = JArray.Parse(response.Content);
            var messages = new List<MessageModel>();
            var messageIds = new List<string>();
            
            foreach (var jToken in array) {
                var messageModel = MessageModel.Deserialize(jToken);
                messages.Add(messageModel);
                messageIds.Add(messageModel.MessageId);
            }

            if (messages.Count == 0) {
                queueingMessages = false;
                pollMessages = true;
            }
            else {
                ServerConnection.Instance.MakeRequestAsync("/chat/confirm-messages", Method.POST, new List<(string key, string value)>{("from", from), ("to", to), ("ids", new JArray(messageIds.ToArray()).ToString(Formatting.None))}, response1 => {
                    QueueMessages(messages.QuickSorted((model1, model2) => string.Compare(model1.Timestamp, model2.Timestamp, StringComparison.Ordinal)));
                    pollMessages = true;
                });
            }
        });
    }

    private void QueueMessages(IEnumerable<MessageModel> messages) {
        foreach (var message in messages) {
            messageQueue.Enqueue(message);
        }

        queueingMessages = false;
    }
}