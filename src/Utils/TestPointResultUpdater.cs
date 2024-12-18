using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class TestPointResultUpdater
{
    private readonly string _personalAccessToken = "{your_pat}";
    private readonly string _organization = "{your_organization}";
    private readonly string _project = "{your_project}";

    public async Task AddTestPointResult(int testPointId, string outcome)
    {
        using (var client = new HttpClient())
        {
            // Set up the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"username:{_personalAccessToken}")));

            // Prepare the result payload
            var resultPayload = new
            {
                outcome = outcome, // e.g., "Passed" or "Failed"
                // Add other necessary fields as required
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(resultPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request to add the test point result
            var response = await client.PostAsync($"https://dev.azure.com/{_organization}/{_project}/_apis/test/runs/{testPointId}/results?api-version=6.0", content);

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
}
