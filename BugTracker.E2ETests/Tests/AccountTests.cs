using BugTracker.E2ETests.Pages.Account;
using BugTracker.E2ETests.Pages.Issue;

namespace BugTracker.E2ETests.Tests
{
    /// <summary>
    /// End-to-end tests for account registration, login, logout, and profile update.
    /// </summary>
    [TestClass]
    public class AccountTests : BaseE2ETest
    {
        private string _regEmail;
        private string _regUserName;
        private const string TestPass = "Password123!";
        private const string TestUser = "autotest1@example.com";

        [TestInitialize]
        public void Setup() 
        {
            var guid = System.Guid.NewGuid();
            _regEmail = $"e2e{guid:N}@test.local";
            _regUserName = $"e2e{guid:N}";
        } 

        [TestMethod]
        public async Task UC00_Can_Register_Then_Login()
        {
            var page = await NewPageAsync();
            var register = new RegisterPage(page);
            await register.NavigateAsync();
            await register.RegisterAsync(_regUserName, _regEmail, TestPass, TestPass);

            var indexPage = new IssueIndexPage(page);
            await indexPage.NavigateAsync();
            var headers = await indexPage.GetHeaderTitlesAsync();
            CollectionAssert.AreEqual(
                new List<string> { "Id", "Title", "Author", "Assignee", "Priority", "Status", "Actions" },
                headers.ToList());
            await page.CloseAsync();
        }

        [TestMethod]
        public async Task UC02_Can_Logout()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var indexPage = new IssueIndexPage(page);
            await indexPage.NavigateAsync();
            var headers = await indexPage.GetHeaderTitlesAsync();
            CollectionAssert.AreEqual(
                new List<string> { "Id", "Title", "Author", "Assignee", "Priority", "Status", "Actions" },
                headers.ToList());
            await indexPage.LogoutAsync();

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task UC03_Can_Update_Profile()
        {
            var page = await NewPageAsync();
            var loginPage = new LoginPage(page);
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(TestUser, TestPass);

            var profile = new ProfilePage(page);
            await profile.NavigateAsync();
            await profile.UpdatePhoneNumberAsync("0999988887777");
            await page.CloseAsync();
        }
    }
}
