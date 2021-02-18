using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class MessageModel {
    public string MessageId;
    public string Timestamp;
    public string From;
    public string To;
    public string Message;

    public static MessageModel Deserialize(JToken messageToken) {
        return new MessageModel {
            MessageId = messageToken.Value<string>("id"),
            Timestamp = messageToken.Value<string>("timestamp"),
            From = messageToken.Value<string>("from"),
            To = messageToken.Value<string>("to"),
            Message = messageToken.Value<string>("message")
        };
    }

    public static string Serialize(MessageModel messageModel) {
        return new JObject {
            ["id"] = messageModel.MessageId,
            ["timestamp"] = messageModel.Timestamp,
            ["from"] = messageModel.From,
            ["to"] = messageModel.To,
            ["message"] = messageModel.Message
        }.ToString(Formatting.None);
    }
}