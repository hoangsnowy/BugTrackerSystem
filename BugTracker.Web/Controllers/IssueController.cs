using System.Linq;
using System.Threading.Tasks;
using BugTracker.Business.DTOs;
using BugTracker.Business.Services;
using BugTracker.Data.Models;
using BugTracker.Web.ViewModels.Issue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BugTracker.Web.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;
        private readonly IIssueService _issueService;
        private readonly IUserService _userService;
        private readonly IPriorityService _priorityService;
        private readonly UserManager<User> _userManager;

        public IssueController(
            ILogger<IssueController> logger,
            IIssueService issueService,
            IUserService userService,
            IPriorityService priorityService,
            UserManager<User> userManager)
        {
            _logger = logger;
            _issueService = issueService;
            _userService = userService;
            _priorityService = priorityService;
            _userManager = userManager;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(string searchString = null)
        {
            var dtos = await _issueService.GetAllAsync(searchString);
            var model = dtos.Select(d => new IssueViewModel
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                Created = d.Created,
                Updated = d.Updated,
                CreatedBy = d.CreatedBy,
                AssignedTo = d.AssignedTo,
                Priority = d.Priority,
                Status = d.Status
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateIssue()
        {
            var users = await _userService.GetAllAsync();      // returns UserDto
            var priorities = await _priorityService.GetAllAsync(); // returns PriorityDto

            var model = new CreateIssueViewModel
            {
                Users = users,
                Priorities = priorities
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue(CreateIssueViewModel formData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateIssue));

            var currentUser = await _userManager.GetUserAsync(User);
            var dto = new CreateIssueDto
            {
                Title = formData.Title,
                Description = formData.Description,
                AssignedToId = formData.AssignedToId,
                PriorityId = formData.PriorityId
            };

            await _issueService.CreateAsync(dto, currentUser.Id);
            _logger.LogInformation("Created issue by user {User}", currentUser.Id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DetailIssue(int issueId)
        {
            var d = await _issueService.GetByIdAsync(issueId);
            if (d == null)
                return RedirectToAction(nameof(Index));

            var model = new IssueDetailsViewModel
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                Created = d.Created,
                Updated = d.Updated,
                CreatedBy = d.CreatedBy,
                AssignedTo = d.AssignedTo,
                Priority = d.Priority,
                Status = d.Status
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIssueStatus(int issueId, int statusId)
        {
            await _issueService.ChangeStatusAsync(issueId, statusId);
            _logger.LogInformation("Changed status of issue {IssueId} to {StatusId}", issueId, statusId);
            return RedirectToAction(nameof(DetailIssue), new { issueId });
        }

        [HttpGet]
        public async Task<IActionResult> EditIssue(int issueId)
        {
            var d = await _issueService.GetByIdAsync(issueId);
            if (d == null)
                return RedirectToAction(nameof(Index));

            var users = await _userService.GetAllAsync();
            var priorities = await _priorityService.GetAllAsync();

            var model = new EditIssueViewModel
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                AssignedToId = d.AssignedToId,
                PriorityId = d.PriorityId,
                Users = users,
                Priorities = priorities
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditIssue(EditIssueViewModel formData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditIssue), new { issueId = formData.Id });

            var dto = new EditIssueDto
            {
                Id = formData.Id,
                Title = formData.Title,
                Description = formData.Description,
                AssignedToId = formData.AssignedToId,
                PriorityId = formData.PriorityId
            };

            await _issueService.UpdateAsync(dto);
            _logger.LogInformation("Updated issue {IssueId}", dto.Id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            await _issueService.DeleteAsync(issueId);
            _logger.LogInformation("Deleted issue {IssueId}", issueId);
            return RedirectToAction(nameof(Index));
        }
    }
}