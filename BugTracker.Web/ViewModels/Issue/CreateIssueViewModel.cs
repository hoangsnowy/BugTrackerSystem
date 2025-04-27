using BugTracker.Business.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Web.ViewModels.Issue
{
    public class CreateIssueViewModel
    {
        [Required(ErrorMessage = "The {0} of issue is not specified")]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "The length of the {0} should be from {2} to {1} characters")]
        public string Title { get; init; } = null!;

        public string Description { get; init; } = null!;

        public string AssignedToId { get; init; }
        public IEnumerable<UserDto> Users { get; init; }

        [Required(ErrorMessage = "The {0} should be specified")]
        public byte PriorityId { get; init; }

        public IEnumerable<PriorityViewModel> Priorities { get; init; }
    }
}