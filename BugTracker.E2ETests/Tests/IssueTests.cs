using BugTracker.E2ETests.Pages.Account;
using BugTracker.E2ETests.Pages.Issue;

namespace BugTracker.E2ETests.Tests
{
    /// <summary>
    /// End-to-end workflow tests for issue management use cases.
    /// </summary>
    [TestClass]
    public class IssueTests : BaseE2ETest
    {
        private const string TestUser = "autotest1@example.com";
        private const string TestPass = "Password123!";

        /// <summary>
        /// TC04-01: Create a new issue with valid data (UC04).
        /// </summary>
        [TestMethod]
        public async Task TC04_01_CreateIssue_Valid()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var indexPage = new IssueIndexPage(page);
            await indexPage.ClickCreateIssueAsync();

            var create = new IssueCreatePage(page);
            await create.CreateAsync(
                "E2E Issue",
                "<p>Test description for issue creation.</p>",
                "dev1",
                "Medium");

            Assert.IsTrue(await indexPage.ContainsIssueAsync("E2E Issue"));
            await page.CloseAsync();
        }

        /// <summary>
        /// TC05-01: Update an existing issue's description (UC05).
        /// </summary>
        [TestMethod]
        public async Task TC05_01_UpdateIssue_Valid()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var index = new IssueIndexPage(page);
            await index.NavigateAsync();
            // Find the last issue ID dynamically from the table
            var rows = page.Locator("table tbody tr");
            var lastRow = rows.Last;
            var idText = await lastRow.Locator("td").First.InnerTextAsync();
            int issueId = int.Parse(idText.Replace("#", string.Empty));

            var edit = new IssueEditPage(page);
            await edit.NavigateAsync(issueId);
            await edit.EditAsync(
                $"Issue #{issueId} - Updated",
                "<p>Updated description content.</p>");

            // Verify update in the list
            Assert.IsTrue(await index.ContainsIssueAsync($"Issue #{issueId} - Updated"));
            await page.CloseAsync();
        }

        /// <summary>
        /// TC06-01: Delete an existing issue (UC06).
        /// </summary>
        [TestMethod]
        public async Task TC06_01_DeleteIssue_Valid()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var index = new IssueIndexPage(page);
            await index.NavigateAsync();
            // Find the last issue ID and title
            var rows = page.Locator("table tbody tr");
            var lastRow = rows.Last;
            var cells = lastRow.Locator("td");
            var idText = await cells.Nth(0).InnerTextAsync();
            var titleText = await cells.Nth(1).InnerTextAsync();
            int issueId = int.Parse(idText.Replace("#", string.Empty));

            // Delete dynamically
            await index.DeleteIssueAsync(issueId);
            // Verify the title no longer appears
            Assert.IsFalse(await index.ContainsIssueAsync(titleText));
            await page.CloseAsync();
        }

        /// <summary>
        /// TC07-01: Change status of an existing issue (UC07).
        /// </summary>
        [TestMethod]
        public async Task TC07_01_ChangeIssueStatus_Valid()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var index = new IssueIndexPage(page);
            await index.NavigateAsync();
            // Find first issue ID
            var firstRow = page.Locator("table tbody tr").First;
            var idText = await firstRow.Locator("td").First.InnerTextAsync();
            int issueId = int.Parse(idText.Replace("#", string.Empty));

            // Change status dynamically
            await index.ChangeIssueStatusAsync(issueId, "Resolved");
            await page.WaitForTimeoutAsync(500);
            // Verify status cell updated
            var statusCell = firstRow.Locator("td").Nth(5); // assuming status is 6th column
            var statusText = await statusCell.InnerTextAsync();
            Assert.IsTrue(statusText.Contains("Resolved"));
            await page.CloseAsync();
        }
    }
}
