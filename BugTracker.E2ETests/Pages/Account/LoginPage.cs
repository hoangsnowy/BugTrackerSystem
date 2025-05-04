using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Account
{
    /// <summary>
    /// Page object representing the login page of the BugTracker application.
    /// Encapsulates navigation and login actions.
    /// </summary>
    public class LoginPage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Identity/Account/Login";

        /// <summary>
        /// Initializes a new instance of <see cref="LoginPage"/> using the provided Playwright page.
        /// </summary>
        public LoginPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates the browser to the login URL.
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Fills in the login form with the specified credentials and submits it.
        /// Waits until redirected to the root or issues index.
        /// </summary>
        public async Task LoginAsync(string email, string password)
        {
            await _page.FillAsync("input[name=\"Input.Email\"]", email);
            await _page.FillAsync("input[name=\"Input.Password\"]", password);
            await _page.ClickAsync("button[type=submit]");
            // Wait for redirect to root or Index page
            await _page.WaitForURLAsync(url =>
                url == E2ETestConfig.BaseUrl + "/" || url.EndsWith("/Issue/Index"));
        }
    }
}
