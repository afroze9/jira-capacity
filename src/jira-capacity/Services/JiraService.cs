using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Atlassian.Jira;
using jira_capacity.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace jira_capacity.Services;

public class JiraService
{
    private RestClientOptions _options = new RestClientOptions("https://confiz.atlassian.net") {
        Authenticator = new HttpBasicAuthenticator("afroze.amjad@confiz.com", ""),
        MaxTimeout = 10_000,
    };

    private const string StoryPointsEstimateCustomField = "customfield_13508";
    
    public async Task<List<SprintResponse>> GetData()
    {
        var client = new RestClient(_options);
        var request = new RestRequest("/rest/agile/1.0/board/425/sprint");
        try
        {
            JiraResponse<SprintResponse>? response = await client.GetAsync<JiraResponse<SprintResponse>>(request);
            return response == null ? new List<SprintResponse>() : response.Values;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<SprintResponse>();
        }
    }

    public async Task<string> GetSprintIssues(int selectedSprintId)
    {
        var client = new RestClient(_options);
        var request = new RestRequest($"/rest/agile/1.0/board/425/sprint/{selectedSprintId}/issue");
        try
        {
            RestResponse response = await client.GetAsync(request);
            var res = JsonConvert.DeserializeObject<JiraIssuesResponse>(response.Content);
            return string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }
}