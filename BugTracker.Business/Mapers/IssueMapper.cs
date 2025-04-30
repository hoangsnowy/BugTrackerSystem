using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Data.Models;

namespace BugTracker.Business.Mapers
{
    public static class IssueMapper
    {
        public static IssueDto ToDto(Issue i) => new IssueDto
        {
            Id = i.Id,
            Title = i.Title,
            Description = i.Description,
            Created = i.Created,
            Updated = i.Updated,
            CreatedBy = i.CreatedBy.UserName,
            AssignedTo = i.AssignedTo?.UserName,
            AssignedToId = i.AssignedToId,
            Priority = (Priority)i.Priority,
            Status = (Status)i.Status
        };

        public static Issue Create(CreateIssueDto form, string creatorId) => new Issue
        {
            Title = form.Title,
            Description = form.Description,
            Created = DateTime.UtcNow,
            CreatedById = creatorId,
            AssignedToId = form.AssignedToId,
            Priority = (byte)form.Priority,
            Status = (byte)Status.Open
        };

        public static Issue Update(EditIssueDto form) => new Issue
        {
            Id = form.Id,
            Title = form.Title,
            Description = form.Description,
            Updated = DateTime.UtcNow,
            AssignedToId = form.AssignedToId,
            Priority = (byte)form.Priority
        };

        public static Issue ChangeStatus(int issueId, Status status) => new Issue
        {
            Id = issueId,
            Status = (byte)status,
            Updated = DateTime.UtcNow
        };
    }
}
