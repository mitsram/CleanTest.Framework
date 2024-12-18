using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class TestCaseUpdater
{
    private readonly string _personalAccessToken = "{your_pat}";
    private readonly string _organization = "{your_organization}";
    private readonly string _project = "{your_project}";

    public async Task UpdateTestCase(int testCaseId, string newOutcome)
    {
        using (var client = new HttpClient())
        {
            // Set up the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"username:{_personalAccessToken}")));

            // Prepare the update payload
            var updatePayload = new
            {
                outcome = newOutcome, // e.g., "Passed" or "Failed"
                // Add other fields to update as necessary
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(updatePayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the PATCH request to update the test case
            var response = await client.PatchAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/plans/{yourTestPlanId}/suites/{yourTestSuiteId}/testcases/{testCaseId}?api-version=6.0", content);

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
    }
}
