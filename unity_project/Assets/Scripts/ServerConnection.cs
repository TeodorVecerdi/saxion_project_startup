using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityCommons;
using UnityEngine;
using UnityEngine.Networking;

public enum Method {
    GET,
    POST
}

public class ServerConnection : MonoSingleton<ServerConnection> {
    private static string MethodToString(Method method) {
        return method switch {
            Method.GET => "GET",
            _ => "POST"
        };
    }

    protected override void OnAwake() {
        DontDestroyOnLoad(gameObject);
    }

    public void MakeRequestAsync(string endpoint, Method method, List<(string key, string value)> data, Action<Response> onComplete) {
        StartCoroutine(RunThreadRequest(endpoint, method, data, onComplete));
    }

    private IEnumerator RunThreadRequest(string endpoint, Method method, IReadOnlyCollection<(string key, string value)> data, Action<Response> onComplete) {
        UnityWebRequest request;
        if (method == Method.POST) {
            request = UnityWebRequest.Post($"http://18.198.156.185:3000{(endpoint.StartsWith("/") ? endpoint : "/" + endpoint)}",
                                           data.ToDictionary(tuple => tuple.key, tuple => tuple.value));
        } else {
            var url = $"http://18.198.156.185:3000{(endpoint.StartsWith("/") ? endpoint : "/" + endpoint)}?";
            foreach (var (key, value) in data) {
                url += $"{key}={Uri.EscapeDataString(value)}&";
            }

            url = url.Substring(0, url.Length - 1);
            request = UnityWebRequest.Get(url);
        }

        yield return request.SendWebRequest();
        
        var response = new Response {
            StatusCode = (int) request.responseCode,
            Content = request.downloadHandler.text
        };
        
        onComplete?.Invoke(response);
    }

    public class Response {
        public int StatusCode;
        public string Content;
    }
}