using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class AzureTestUpdater
{
    private readonly string _personalAccessToken = "{your_pat}";
    private readonly string _organization = "{your_organization}";
    private readonly string _project = "{your_project}";
    private readonly HttpClient _client;

    public AzureTestUpdater()
    {
        _client = new HttpClient();
        // Set up the authorization header
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"username:{_personalAccessToken}")));
    }

    public async Task UpdateTestCase(int testCaseId, string newOutcome, int yourTestPlanId, int yourTestSuiteId)
    {
        // Prepare the update payload
        var updatePayload = new
        {
            outcome = newOutcome, // e.g., "Passed" or "Failed"
            // Add other fields to update as necessary
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(updatePayload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send the PATCH request to update the test case
        var response = await _client.PatchAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/plans/{yourTestPlanId}/suites/{yourTestSuiteId}/testcases/{testCaseId}?api-version=6.0", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Test case {testCaseId} updated successfully.");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating test case: {response.StatusCode}, Details: {errorContent}");
        }
    }

    public async Task UpdateTestResult(int testCaseId, bool isPassed, int runId)
    {
        var result = new
        {
            outcome = isPassed ? "Passed" : "Failed",
            testCaseTitle = $"Test Case {testCaseId}",
            // Add other necessary fields as required
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/runs/{runId}/results?api-version=6.0", content);
        response.EnsureSuccessStatusCode();
        Console.WriteLine($"Test result for {testCaseId} updated successfully.");
    }

    public async Task AddTestPointResult(int testPointId, string outcome)
    {
        // Prepare the result payload
        var resultPayload = new
        {
            outcome = outcome, // e.g., "Passed" or "Failed"
            // Add other necessary fields as required
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(resultPayload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send the POST request to add the test point result
        var response = await _client.PostAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/runs/{testPointId}/results?api-version=6.0", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Test point result for {testPointId} added successfully.");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error adding test point result: {response.StatusCode}, Details: {errorContent}");
        }
    }
}
