﻿using BugTracker.Business.Enums;

namespace BugTracker.Business.DTOs
{
    public class IssueDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToId { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }

    public class CreateIssueDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedToId { get; set; }
        public Priority Priority { get; set; }
    }

    public class EditIssueDto : CreateIssueDto
    {
        public int Id { get; set; }
    }
}
