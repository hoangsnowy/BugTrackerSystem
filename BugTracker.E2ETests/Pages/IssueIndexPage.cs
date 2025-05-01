using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages
{
    /// <summary>
    /// Page object for the issues listing page.
    /// Provides navigation and data-checking methods.
    /// </summary>
    public class IssueIndexPage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Identity/Account/Login";

        /// <summary>
        /// Initializes a new instance of <see cref="IssueIndexPage"/>.
        /// </summary>
        public IssueIndexPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the issue index page.
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Checks if the table contains an issue row with the specified title.
        /// </summary>
        public async Task<bool> ContainsIssueAsync(string title)
        {
            var txt = await _page.Locator("table").InnerTextAsync();
            return txt.Contains(title);
        }

        /// <summary>
        /// Clicks the "Create Issue" link to open the creation form.
        /// </summary>
        public Task ClickCreateIssueAsync()
            => _page.ClickAsync("a[href=\"/Issue/CreateIssue\"]");
    }
}
