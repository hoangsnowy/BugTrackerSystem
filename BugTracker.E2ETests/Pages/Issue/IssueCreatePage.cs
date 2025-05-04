using BugTracker.E2ETests.Settings;
using Microsoft.Playwright;

namespace BugTracker.E2ETests.Pages.Issue
{
    /// <summary>
    /// Page object for the "Create Issue" form.
    /// Encapsulates field filling and submission behavior.
    /// </summary>
    public class IssueCreatePage
    {
        private readonly IPage _page;
        private static readonly string Url = $"{E2ETestConfig.BaseUrl}/Issue/CreateIssue";

        /// <summary>
        /// Initializes a new instance of <see cref="IssueCreatePage"/>.
        /// </summary>
        public IssueCreatePage(IPage page) => _page = page;

        /// <summary>
        /// Navigate directly to the Create Issue page
        /// </summary>
        public Task NavigateAsync() => _page.GotoAsync(Url);

        /// <summary>
        /// Fills out and submits the create-issue form with the specified values.
        /// Waits to be redirected back to the list.
        /// </summary>
        public async Task CreateAsync(string title, string description, string assignee, string priority)
        {
            // Assumes already on CreateIssue page (or use NavigateAsync())
            await _page.FillAsync("input[name=Title]", title);
            await _page.EvaluateAsync("(html) => { document.querySelector('.ql-editor').innerHTML = html; }", description);
            await _page.SelectOptionAsync("select[name=AssignedToId]", new[] { assignee });
            await _page.SelectOptionAsync("select[name=PriorityId]", new[] { priority });
            await _page.ClickAsync("#create-issue-button");
            // Wait for redirect to Index
            // Wait for redirect to root or Index page
            await _page.WaitForURLAsync(url =>
                url == E2ETestConfig.BaseUrl + "/" || url.EndsWith("/Issue/Index"));
        }
    }
}
