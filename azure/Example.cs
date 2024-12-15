using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace YourNamespace
{
    [TestClass]
    public class YourPlaywrightTests
    {
        private const string AdoUrl = "https://dev.azure.com/YourOrganization"; // Your Azure DevOps URL
        private const string ProjectName = "YourProjectName"; // Your Azure DevOps Project Name
        private const string PersonalAccessToken = "YourPAT"; // Your Personal Access Token
        private const int TestCaseId = 1234; // Your ADO Test Case ID

        [TestMethod]
        [TestCategory("ADO Test Case ID: 1234")]
        public void YourTestMethod()
        {
            // Your test logic here
            bool testOutcome = true; // Set this based on your test result

            // Update the test case status
            UpdateTestCaseStatus(testOutcome);
        }

        private void UpdateTestCaseStatus(bool isPassed)
        {
            var tfs = new TfsTeamProjectCollection(new Uri(AdoUrl), new VssBasicCredential(string.Empty, PersonalAccessToken));
            var testManagementService = tfs.GetService<ITestManagementService>();
            var testManagement = testManagementService.GetTeamProject(ProjectName);
            var testCase = testManagement.TestCases.Find(TestCaseId);

            if (testCase != null)
            {
                var outcome = isPassed ? TestOutcome.Passed : TestOutcome.Failed;
                var testRun = testManagement.TestRuns.Create("Automated Test Run", DateTime.Now);
                var testResult = testRun.Results.Create(testCase);
                testResult.Outcome = outcome;
                testResult.Save();
                testRun.Save();
            }
            else
            {
                throw new Exception($"Test case with ID {TestCaseId} not found.");
            }
        }
    }
}
