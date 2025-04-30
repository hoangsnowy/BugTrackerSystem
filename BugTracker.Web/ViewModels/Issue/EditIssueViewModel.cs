using BugTracker.Business.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Web.ViewModels.Issue
{
    public class EditIssueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} of issue is not specified")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The length of the {0} should be from {2} to {1} characters")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string AssignedToId { get; set; }

        [BindNever]
        [ValidateNever]
        public IEnumerable<UserDto> Users { get; set; }

        [Required(ErrorMessage = "The {0} should be specified")]
        public string PriorityId { get; set; }

        [BindNever]
        [ValidateNever]
        public IEnumerable<PriorityViewModel> Priorities { get; set; }
    }
}