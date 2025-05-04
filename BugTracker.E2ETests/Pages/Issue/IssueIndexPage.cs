using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Issue
{
    /// <summary>
    /// Page object for the issues listing page.
    /// Provides navigation, search, delete, status-change, and inspection methods.
    /// </summary>
    public class IssueIndexPage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Issue/Index";

        /// <summary>
        /// Initializes the <see cref="IssueIndexPage"/>.
        /// </summary>
        public IssueIndexPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the issue index page.
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Enters the search term and triggers filtering.
        /// </summary>
        public async Task SearchAsync(string term)
        {
            await _page.FillAsync("input[name=searchString]", term);
            await _page.ClickAsync("button#search-button");
            await _page.WaitForTimeoutAsync(500);
        }

        /// <summary>
        /// Checks if the table contains an issue with the given title.
        /// </summary>
        public async Task<bool> ContainsIssueAsync(string title)
        {
            var text = await _page.Locator("table").InnerTextAsync();
            return text.Contains(title);
        }

        /// <summary>
        /// Clicks the link to open the create issue form.
        /// </summary>
        public Task ClickCreateIssueAsync()
            => _page.ClickAsync("a[href=\"/Issue/CreateIssue\"]");

        /// <summary>
        /// Deletes the issue with the specified ID directly from the list.
        /// Handles the confirmation popup automatically.
        /// </summary>
        public async Task DeleteIssueAsync(int issueId)
        {
            // Hook into any confirmation dialog and accept it
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();

            // Click the delete link whose href ends with "?issueId={issueId}"
            await _page.ClickAsync($"a[href=\"/Issue/DeleteIssue?issueId={issueId}\"]");

            // Optionally wait a moment for the row to be removed
            await _page.WaitForTimeoutAsync(500);
        }

        /// <summary>
        /// Changes the status of the specified issue via the Bootstrap dropdown.
        /// </summary>
        public async Task ChangeIssueStatusAsync(int issueId, string newStatus)
        {
            // 1) Open the dropdown toggle button
            await _page.ClickAsync(
                $"tr:has-text(\"#{issueId}\") button.dropdown-toggle[title=\"Change status\"]");

            // 2) Wait for the menu to appear
            await _page.WaitForSelectorAsync(
                $"tr:has-text(\"#{issueId}\") .dropdown-menu.show");

            // 3) Click the menu item that matches newStatus
            await _page.ClickAsync(
                $"tr:has-text(\"#{issueId}\") .dropdown-menu >> text=\"{newStatus}\"");

            // 4) Give the UI a moment to refresh
            await _page.WaitForTimeoutAsync(300);
        }

        /// <summary>
        /// Retrieves the titles of the table headers of Issue Index Page.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<string>> GetHeaderTitlesAsync() => _page.Locator("table thead tr th").AllTextContentsAsync();

        /// <summary>
        /// Submits the logout form to sign out the user.
        /// </summary>
        public Task LogoutAsync()
            => _page.ClickAsync("#logoutForm button[type='submit']");

    }
}
