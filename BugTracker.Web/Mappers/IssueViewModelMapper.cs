using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Web.ViewModels.Issue;
using BugTracker.Web.ViewModels;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BugTracker.Web.Mappers
{
    public static class IssueViewModelMapper
    {
        public static List<IssueViewModel> ToListViewModels(IEnumerable<IssueDto> dtos) =>
            dtos.Select(ToListViewModel).ToList();

        public static IssueViewModel ToListViewModel(IssueDto d) => new IssueViewModel
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            Created = d.Created,
            Updated = d.Updated,
            CreatedBy = d.CreatedBy,
            AssignedTo = d.AssignedTo,
            Priority = d.Priority.ToString(),
            Status = d.Status.ToString()
        };

        public static CreateIssueDto ToCreateDto(CreateIssueViewModel vm, string currentUserId) => new CreateIssueDto
        {
            Title = vm.Title,
            Description = vm.Description,
            AssignedToId = vm.AssignedToId,
            Priority = Enum.Parse<Priority>(vm.PriorityId)
        };

        public static IssueDetailsViewModel ToDetailsViewModel(IssueDto d) => new IssueDetailsViewModel
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            Created = d.Created,
            Updated = d.Updated,
            CreatedBy = d.CreatedBy,
            AssignedTo = d.AssignedTo,
            Priority = d.Priority.ToString(),
            Status = d.Status.ToString()
        };

        public static EditIssueViewModel ToEditViewModel(IssueDto d, IEnumerable<UserDto> users, IEnumerable<PriorityViewModel> priorities) => new EditIssueViewModel
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            AssignedToId = d.AssignedToId,
            PriorityId = d.Priority.ToString(),
            Users = users,
            Priorities = priorities
        };

        public static EditIssueDto ToEditDto(EditIssueViewModel vm) => new EditIssueDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Description = vm.Description,
            AssignedToId = vm.AssignedToId,
            Priority = Enum.Parse<Priority>(vm.PriorityId)
        };
    }
}
