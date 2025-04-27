using System;

namespace BugTracker.Web.ViewModels.Issue
{
    public class IssueDetailsViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; }
        public DateTime Created { get; init; }
        public DateTime? Updated { get; init; }
        public string CreatedBy { get; init; } = null!;
        public string AssignedTo { get; init; }
        public string Priority { get; init; } = null!;
        public string Status { get; init; } = null!;
    }
}