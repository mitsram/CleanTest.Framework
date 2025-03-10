using System.Text.Json;
using System.Text;
using RestSharp;

namespace CleanTest.Framework.Utils;

public class AzureDevOpsIntegration(string organization, string project, int planId, int suiteId, string pat)
{
    private readonly string _organization = organization;
    private readonly string _project = project;
    private readonly int _planId = planId;
    private readonly int _suiteId = suiteId;
    private readonly string _pat = pat; // Personal Access Token

    public async Task UpdateTestCaseOutcome(int testCaseId, TestOutcome outcome)
    {
        var client = new RestClient("https://dev.azure.com");
         
        // First, get the test point assignment
        var encodedProject = Uri.EscapeDataString(_project);
        var encodedOrg = Uri.EscapeDataString(_organization);
        
        var pointRequest = new RestRequest($"{encodedOrg}/{encodedProject}/_apis/testplan/Plans/{_planId}/suites/{_suiteId}/testcase/{testCaseId}", Method.Get);
        pointRequest.AddQueryParameter("api-version", "6.0");
        pointRequest.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_pat}"))}");

        var pointResponse = await client.ExecuteAsync<TestPointResponse>(pointRequest);
        if (!pointResponse.IsSuccessful)
        {
            throw new Exception($"Failed to get test point: {pointResponse.ErrorMessage}");
        }

        // Create test result
        var resultRequest = new RestRequest($"{encodedOrg}/{encodedProject}/_apis/testPlan/Plans/{_planId}/Suites/{_suiteId}/TestPoint", Method.Patch);
        resultRequest.AddQueryParameter("api-version", "6.0");
        resultRequest.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_pat}"))}");
        
        var pointResponseJson = pointResponse.Content;
        var responseObject = JsonSerializer.Deserialize<TestPointResponse>(pointResponseJson!);

        var resultPayload = new[]
        {
            new
            {
                id = responseObject?.value[0].pointAssignments[0].id,
                Outcome = outcome.ToString()
            }
        };

        resultRequest.AddJsonBody(resultPayload);
        
        var resultResponse = await client.ExecuteAsync(resultRequest);
        if (!resultResponse.IsSuccessful)
        {
            throw new Exception($"Failed to update test result: {resultResponse.ErrorMessage}");
        }
    }
}

public enum TestOutcome
{
    Passed,
    Failed,
    Blocked,
    NotApplicable,
    Paused,
    InProgress,
    NotExecuted
}

public class TestPointResponse
{
    public int count { get; init; }
    public List<Value> value { get; init; }
}

public class Value
{
    public List<PointAssignments> pointAssignments { get; }
}

public class PointAssignments
{
    public int id { get; }
}