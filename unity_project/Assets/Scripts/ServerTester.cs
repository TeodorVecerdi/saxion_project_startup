using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RestSharp;
using UnityEngine;

public class ServerTester : MonoBehaviour {
    public List<Request> Requests;
    [Dropdown("GetRequests")] public string Request;

    private static readonly RestClient client = new RestClient("http://localhost:3000");

    private List<string> GetRequests() {
        var requests = Requests.Select(request => request.RequestName).ToList();
        requests.Insert(0, "None");
        return requests;
    }

    [Button]
    private void Execute() {
        if(string.IsNullOrEmpty(Request) || Request == "None") return;
        
        var requestToExecute = Requests.Find(request1 => request1.RequestName == Request);

        var request = new RestRequest(requestToExecute.Resource, requestToExecute.Method);
        foreach (var requestParameter in requestToExecute.Parameters) {
            request.AddParameter(requestParameter.Key, requestParameter.Value);
        }
        var response = client.Execute(request);
        
        var responseString =
            @$"[{Request.ToUpperInvariant()}]
Successful={response.IsSuccessful}

Error Message:
{response.ErrorMessage}

Response Status:
{response.ResponseStatus}

Status Code:
{response.StatusCode} ({(int)response.StatusCode})
{response.StatusDescription}

Content:
{response.Content}
";

        Debug.Log(responseString);
    }
}

[Serializable]
public struct Request {
    public string RequestName;
    public Method Method;
    public string Resource;
    public List<RequestParameter> Parameters;
}

[Serializable]
public struct RequestParameter {
    public string Key;
    public string Value;
}