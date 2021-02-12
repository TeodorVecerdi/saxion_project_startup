using System.Collections.Generic;
using System.Linq;
using RestSharp;
using UnityCommons;

public class ServerConnection : MonoSingleton<ServerConnection> {
    private static readonly RestClient client = new RestClient("http://localhost:3000");

    public Response MakeRequest(string endpoint, Method method, Dictionary<string, string> data) {
        return MakeRequest(endpoint, method, data.Select(pair => (pair.Key, pair.Value)).ToList());
    }
    
    public Response MakeRequest(string endpoint, Method method, List<(string key, string value)> data) {
        var request = new RestRequest(endpoint, method);
        foreach (var parameter in data) {
            request.AddParameter(parameter.key, parameter.value);
        }
        var response = client.Execute(request);
        return new Response {
            IsSuccessful = response.IsSuccessful,
            StatusCode = (int)response.StatusCode,
            StatusDescription = response.StatusDescription,
            Content = response.Content,
            ErrorMessage = response.ErrorMessage
        };
    }

    public class Response {
        public bool IsSuccessful;
        public int StatusCode;
        public string StatusDescription;
        public string Content;
        public string ErrorMessage;
    }
}