using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Issue
{
    /// <summary>
    /// Page object for editing an existing issue.
    /// Encapsulates navigation and form submission.
    /// </summary>
    public class IssueEditPage
    {
        private readonly IPage _page;
        private static readonly string UrlTemplate = $"{E2ETestConfig.BaseUrl}/Issue/EditIssue?issueId={{0}}";

        /// <summary>
        /// Initializes a new <see cref="IssueEditPage"/>.
        /// </summary>
        public IssueEditPage(IPage page) => _page = page;

        /// <summary>
        /// Navigates to the edit page for the specified issue ID.
        /// </summary>
        public Task NavigateAsync(int issueId) => _page.GotoAsync(string.Format(UrlTemplate, issueId));

        /// <summary>
        /// Applies new title and description, submits, and waits for redirect.
        /// </summary>
        public async Task EditAsync(string title, string description)
        {
            await _page.FillAsync("input[name=Title]", title);
            await _page.ClickAsync(".ql-editor");
            await _page.EvaluateAsync("(html) => document.querySelector('.ql-editor').innerHTML = html", description);
            await _page.ClickAsync("#save-issue-button");
            // Wait for redirect to Index
            // Wait for redirect to root or Index page
            await _page.WaitForURLAsync(url =>
                url == E2ETestConfig.BaseUrl + "/" || url.EndsWith("/Issue/Index"));
        }
    }
}
