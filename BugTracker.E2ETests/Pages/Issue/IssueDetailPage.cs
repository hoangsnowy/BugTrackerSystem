using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Issue
{
    /// <summary>
    /// Page object for viewing issue details.
    /// </summary>
    public class IssueDetailPage
    {
        private readonly IPage _page;
        private static readonly string UrlTemplate = $"{E2ETestConfig.BaseUrl}/Issue/DetailIssue?issueId={{0}}";

        /// <summary>
        /// Initializes the <see cref="IssueDetailPage"/> with the given Playwright page.
        /// </summary>
        public IssueDetailPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the detail view of the specified issue.
        /// </summary>
        public Task NavigateAsync(int issueId) => _page.GotoAsync(string.Format(UrlTemplate, issueId));

        /// <summary>
        /// Gets the issue title from the detail page.
        /// </summary>
        public Task<string> GetTitleAsync() => _page.InnerTextAsync("h1.issue-title");

        /// <summary>
        /// Gets the issue description HTML from the detail page.
        /// </summary>
        public Task<string> GetDescriptionHtmlAsync() => _page.InnerHTMLAsync("div.issue-description");
    }
}
