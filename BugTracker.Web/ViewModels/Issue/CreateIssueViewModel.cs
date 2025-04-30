using BugTracker.Business.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Web.ViewModels.Issue
{
    public class CreateIssueViewModel
    {
        [Required(ErrorMessage = "The {0} of issue is not specified")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The length of the {0} should be from {2} to {1} characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The {0} should be specified")]
        public string Description { get; set; }

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