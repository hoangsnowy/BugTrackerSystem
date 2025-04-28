using System;

namespace BugTracker.Web.ViewModels.Issue
{
    public class IssueViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string AssignedTo { get; set; }
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}