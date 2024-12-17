using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class TestResultUpdater
{
    private readonly string _azureDevOpsUrl = "https://dev.azure.com/{your_organization}/{your_project}/_apis/test/runs/{runId}/results?api-version=6.0";
    private readonly string _personalAccessToken = "{your_pat}";

    public async Task UpdateTestResult(int testCaseId, bool isPassed)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"username:{_personalAccessToken}")));

            var result = new
            {
                outcome = isPassed ? "Passed" : "Failed",
                testCaseTitle = $"Test Case {testCaseId}",
                // Add other necessary fields as required
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_azureDevOpsUrl, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
