using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class TestCaseResultUpdater
{
    private readonly string _personalAccessToken = "{your_pat}";
    private readonly string _organization = "{your_organization}";
    private readonly string _project = "{your_project}";

    public async Task AddTestCaseResult(int testCaseId, string outcome, int runId)
    {
        using (var client = new HttpClient())
        {
            // Set up the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"username:{_personalAccessToken}")));

            // Prepare the result payload
            var resultPayload = new[]
            {
                new
                {
                    outcome = outcome, // e.g., "Passed" or "Failed"
                    testCaseTitle = $"Test Case {testCaseId}",
                    // Add other necessary fields as required
                }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(resultPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request to add the test case result
            var response = await client.PostAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/runs/{runId}/results?api-version=6.0", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Test case result for {testCaseId} added successfully.");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error adding test case result: {response.StatusCode}, Details: {errorContent}");
            }
        }
    }
}
