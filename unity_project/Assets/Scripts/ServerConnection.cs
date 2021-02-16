using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using RestSharp;
using UnityCommons;

public class ServerConnection : MonoSingleton<ServerConnection> {
    private static readonly RestClient client = new RestClient("http://18.198.156.185:3000/");

    protected override void OnAwake() {
        DontDestroyOnLoad(gameObject);
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

    public void MakeRequestAsync(string endpoint, Method method, List<(string key, string value)> data, Action<Response> onComplete) {
        StartCoroutine(RunThreadRequest(endpoint, method, data, onComplete));
    }

    private IEnumerator RunThreadRequest(string endpoint, Method method, IReadOnlyCollection<(string key, string value)> data, Action<Response> onComplete) {
        Response response = null;
        
        var thread = new Thread(() => { 
            var request = new RestRequest(endpoint, method);
            foreach (var (key, value) in data) {
                request.AddParameter(key, value);
            }

            var restResponse = client.Execute(request);
            response = new Response {
                IsSuccessful = restResponse.IsSuccessful,
                StatusCode = (int) restResponse.StatusCode,
                StatusDescription = restResponse.StatusDescription,
                Content = restResponse.Content,
                ErrorMessage = restResponse.ErrorMessage
            };
        });
        thread.Start();
        
        while (thread.IsAlive) {
            yield return null;
        }

        onComplete?.Invoke(response);
    }

    public class Response {
        public bool IsSuccessful;
        public int StatusCode;
        public string StatusDescription;
        public string Content;
        public string ErrorMessage;
    }
}