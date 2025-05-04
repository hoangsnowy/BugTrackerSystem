using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Account
{
    /// <summary>
    /// Page object for user registration.
    /// Handles navigation and form submission for new accounts.
    /// </summary>
    public class RegisterPage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Identity/Account/Register";

        /// <summary>
        /// Initializes a new <see cref="RegisterPage"/>.
        /// </summary>
        public RegisterPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the registration page.
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Completes and submits the registration form.
        /// Waits for redirect to login page.
        /// </summary>
        public async Task RegisterAsync(string userName, string email, string password, string confirm)
        {
            await _page.FillAsync("input[name=\"Input.Login\"]", userName);
            await _page.FillAsync("input[name=\"Input.Email\"]", email);
            await _page.FillAsync("input[name=\"Input.Password\"]", password);
            await _page.FillAsync("input[name=\"Input.ConfirmPassword\"]", confirm);
            await _page.ClickAsync("button[type=submit]");
        }
    }
}
