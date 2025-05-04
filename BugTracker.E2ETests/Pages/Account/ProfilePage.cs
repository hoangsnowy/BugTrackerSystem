using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Account
{
    /// <summary>
    /// Page object for managing the user profile.
    /// Allows updating display name.
    /// </summary>
    public class ProfilePage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Identity/Account/Manage";

        /// <summary>
        /// Initializes a new <see cref="ProfilePage"/>.
        /// </summary>
        public ProfilePage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the profile management page.
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Updates the phone Number and saves changes.
        /// </summary>
        public async Task UpdatePhoneNumberAsync(string newPhoneNumer)
        {
            await _page.FillAsync("input[name=\"Input.PhoneNumber\"]", newPhoneNumer);
            await _page.ClickAsync("button:has-text(\"Save\")");
            await _page.WaitForSelectorAsync("text=Your profile has been updated");
        }
    }
}
