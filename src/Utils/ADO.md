## Execute
If your goal is to update the status in the Execute tab of a test case in Azure DevOps and maintain a history of executions from a Playwright C# script, you need to specifically work with Test Points and Test Results. Here’s how to achieve that using the Azure DevOps REST API:

Key Concepts
	1.	Test Points:
	•	Represent the intersection of a test case and a configuration in a test suite.
	•	Execution results are tied to these test points.
	2.	Test Results:
	•	Each test execution (from the Execute tab) creates a new result in the test history.
	•	Updating the execution status updates the corresponding history.

Steps to Update the Status and Execution History

1. Prerequisites
	•	Ensure the test case is already part of a Test Suite within a Test Plan in Azure DevOps.
	•	Generate a Personal Access Token (PAT) with Test Management permissions.

2. Fetch the Test Point ID

You need the Test Point ID for the specific test case and configuration. Use the following API:

Endpoint:

GET https://dev.azure.com/{organization}/{project}/_apis/testplan/Plans/{planId}/Suites/{suiteId}/points?api-version=7.1-preview.1

	•	Replace {planId} and {suiteId} with the IDs of your Test Plan and Test Suite.
	•	The response will include a list of test points for each test case.

Example Response:

{
  "value": [
    {
      "id": 123, // Test Point ID
      "testCase": {
        "id": 456,
        "name": "My Test Case"
      },
      "configuration": {
        "name": "Windows + Chrome"
      }
    }
  ]
}

3. Trigger Execution and Create Test Result

Use Playwright C# to run the test and dynamically update the result:
	1.	Run the Test:
Use Playwright to execute the test and capture the status.

string outcome = "Passed"; // Default status
try
{
    var playwright = await Playwright.CreateAsync();
    var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
    var page = await browser.NewPageAsync();
    await page.GotoAsync("https://example.com");
    await browser.CloseAsync();
}
catch
{
    outcome = "Failed";
}


	2.	Update Test Point Execution Status:
After running the test, use the API to create a new test result for the test point:
Endpoint:

PATCH https://dev.azure.com/{organization}/{project}/_apis/test/runs/{runId}/results?api-version=7.1-preview.1

Payload:

[
  {
    "testPoint": { "id": 123 }, // Test Point ID
    "outcome": "Passed", // "Failed", "Blocked", etc.
    "comment": "Automated execution via Playwright",
    "state": "Completed"
  }
]

Example C# code for the API call:

var testResultPayload = new[]
{
    new
    {
        testPoint = new { id = testPointId }, // Replace with actual Test Point ID
        outcome = outcome,
        comment = "Executed using Playwright",
        state = "Completed"
    }
};

await client.PatchAsJsonAsync(
    $"https://dev.azure.com/{organization}/{project}/_apis/test/runs/{runId}/results?api-version=7.1-preview.1",
    testResultPayload
);

4. View History in Execute Tab
	•	Once the result is updated, Azure DevOps will automatically:
	•	Add an entry in the history of executions under the Execute tab of the test case.
	•	Show the latest status (Passed, Failed, etc.) for the test case.

Full Example in C#

Below is a combined example of running Playwright tests and updating Azure DevOps Test Case status.

using System.Net.Http.Json;

class Program
{
    static async Task Main(string[] args)
    {
        string organization = "YourOrganization";
        string project = "YourProject";
        string patToken = "YourPersonalAccessToken";
        string planId = "YourTestPlanId";
        string suiteId = "YourTestSuiteId";
        int testPointId = 123; // Test Point ID for the test case

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{patToken}")));

        // 1. Run Playwright Test
        string outcome = "Passed";
        try
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://example.com");
            await browser.CloseAsync();
        }
        catch
        {
            outcome = "Failed";
        }

        // 2. Update Execution Status in ADO
        var testResultPayload = new[]
        {
            new
            {
                testPoint = new { id = testPointId },
                outcome = outcome,
                comment = "Automated execution via Playwright",
                state = "Completed"
            }
        };

        await client.PatchAsJsonAsync(
            $"https://dev.azure.com/{organization}/{project}/_apis/test/runs/{runId}/results?api-version=7.1-preview.1",
            testResultPayload
        );

        Console.WriteLine($"Test Point {testPointId} updated with outcome: {outcome}");
    }
}

Key Considerations
	1.	Test Point ID: You must map test cases to their respective Test Point IDs.
	2.	Outcome Values: Use valid status values such as Passed, Failed, Blocked, etc.
	3.	Error Handling: Include error handling for API calls to ensure updates are reliable.

This process ensures that the Execute tab reflects the correct status and maintains the execution history automatically.


## runId
The runId is created when you initiate a Test Run in Azure DevOps. If your goal is to update the test execution status and you don’t have a runId, you’ll need to create a new test run first. Here’s how to handle it:

1. Create a New Test Run

To get a runId, you must first create a new Test Run using the Azure DevOps REST API.

API Endpoint:

POST https://dev.azure.com/{organization}/{project}/_apis/test/runs?api-version=7.1-preview.1

Payload Example:

{
  "name": "Automated Test Run - Playwright",
  "plan": { "id": "{testPlanId}" }, // The ID of your Test Plan
  "pointIds": [123] // List of Test Point IDs you want to include in the run
}

Response Example:

When you create a new test run, the response will include a runId:

{
  "id": 789, // This is your runId
  "name": "Automated Test Run - Playwright",
  "state": "InProgress"
}

2. C# Code to Create a Test Run and Get runId

Here’s how you can create a new test run in C# and retrieve the runId:

using System.Net.Http.Json;

static async Task<string> CreateTestRunAsync(HttpClient client, string organization, string project, string planId, int testPointId)
{
    // Define the payload for creating a test run
    var testRunPayload = new
    {
        name = "Automated Test Run - Playwright",
        plan = new { id = planId },
        pointIds = new[] { testPointId }
    };

    // Make the API call to create a test run
    var response = await client.PostAsJsonAsync(
        $"https://dev.azure.com/{organization}/{project}/_apis/test/runs?api-version=7.1-preview.1",
        testRunPayload);

    // Ensure success
    response.EnsureSuccessStatusCode();

    // Parse and return the runId
    var result = await response.Content.ReadFromJsonAsync<dynamic>();
    return result.id; // This is the runId
}

3. Use the runId for Updating Test Results

Once you have the runId, you can use it in subsequent API calls to update the test results. For example:

string runId = await CreateTestRunAsync(client, organization, project, planId, testPointId);

// Update the test result
var testResultPayload = new[]
{
    new
    {
        state = "Completed",
        outcome = "Passed", // or "Failed", "Blocked", etc.
        comment = "Automated execution via Playwright"
    }
};

await client.PatchAsJsonAsync(
    $"https://dev.azure.com/{organization}/{project}/_apis/test/runs/{runId}/results?api-version=7.1-preview.1",
    testResultPayload);

4. End-to-End Workflow
	1.	Create a Test Run: Use the API to create a test run and retrieve the runId.
	2.	Execute the Test: Run your Playwright script to get the test outcome (Passed, Failed, etc.).
	3.	Update Test Results: Use the runId to update the test execution status via the PATCH API.

Helpful Notes
	•	You must associate Test Points with the Test Run when creating it.
	•	The runId is unique to each test run and reflects the specific execution instance.
	•	You can create a separate test run for each automated Playwright test execution or group multiple test cases into one run.