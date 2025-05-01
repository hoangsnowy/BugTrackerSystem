using BugTracker.E2ETests.Pages;

namespace BugTracker.E2ETests.Tests
{
    /// <summary>
    /// End-to-end workflow tests for issue management.
    /// Covers login, listing, creation, and list refresh.
    /// </summary>
    /// 
    [TestClass]
    public class IssueWorkflowTests : BaseE2ETest
    {
        private const string TestUser = "tester1@example.com";
        private const string TestPass = "Password123!";

        /// <summary>
        /// Verifies that creating a new issue updates the list.
        /// </summary>
        [TestMethod]
        public async Task Can_Create_Issue_And_See_It_In_List()
        {
            var page = await NewPageAsync();
            var login = new LoginPage(page);
            await login.NavigateAsync();
            await login.LoginAsync(TestUser, TestPass);

            var index = new IssueIndexPage(page);
            await index.ClickCreateIssueAsync();

            var create = new IssueCreatePage(page);
            await create.CreateAsync("Playwright C#", "\"<p>Automated <strong>E2E</strong> test issue creation with <em>HTML</em> content.</p>\"", "dev1", "Medium");

            Assert.IsTrue(await index.ContainsIssueAsync("Playwright C#"));

            await page.CloseAsync();
        }
    }
}
