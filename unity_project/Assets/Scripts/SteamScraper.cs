using HtmlAgilityPack;
using NaughtyAttributes;
using RestSharp;
using UnityEngine;

public class SteamScraper : MonoBehaviour {
    [ReadOnly] public string Name;
    [ReadOnly] public string Level;
    
    [Space]
    public string SteamID;
    
    private static readonly RestClient client = new RestClient($"https://steamcommunity.com");
    private const string nameXPATH = "//div[@class=\"persona_name\"]//span[@class=\"actual_persona_name\"]";
    private const string levelXPATH = "//div[@class=\"persona_name persona_level\"]/div/span";

    [Button]
    private void Run() {
        var request = new RestRequest($"{SteamID}", Method.GET);
        var response = client.Get(request);
        if (!response.IsSuccessful) {
            Debug.LogWarning(GetRequestString(response));
            return;
        }
        
        var html = response.Content;
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var nameNode = document.DocumentNode.SelectSingleNode(nameXPATH);
        var levelNode = document.DocumentNode.SelectSingleNode(levelXPATH);
        Name = nameNode.GetDirectInnerText();
        Level = levelNode.GetDirectInnerText();
    }

    private string GetRequestString(IRestResponse response) {
        return $@"
Successful={response.IsSuccessful}

Error Message:
{response.ErrorMessage}

Response Status:
{response.ResponseStatus}

Status Code:
{response.StatusCode} ({(int) response.StatusCode})
{response.StatusDescription}

Content:
{response.Content}
";
    }
}