using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Business.Enums;
using BugTracker.Business.Helpers;
using BugTracker.Business.Services;
using BugTracker.Data.Models;
using BugTracker.Web.Mappers;
using BugTracker.Web.ViewModels;
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
        private readonly UserManager<User> _userManager;
        private static readonly ImmutableHashSet<PriorityViewModel> priorities =
            EnumHelper.GetAllEnumValues<Priority>()
                      .Select(q => new PriorityViewModel(q.ToString(), q.ToString()))
                      .ToImmutableHashSet();

        public IssueController(
            ILogger<IssueController> logger,
            IIssueService issueService,
            IUserService userService,
            UserManager<User> userManager)
        {
            _logger = logger;
            _issueService = issueService;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(string searchString = null)
        {
            var dtos = await _issueService.GetAllAsync(searchString);
            var model = IssueViewModelMapper.ToListViewModels(dtos);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateIssue()
        {
            var users = await _userService.GetAllAsync();
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
            var dto = IssueViewModelMapper.ToCreateDto(formData, currentUser.Id);
            await _issueService.CreateAsync(dto, currentUser.Id);
            _logger.LogInformation("Created issue by user {User}", currentUser.Id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DetailIssue(int issueId)
        {
            var dto = await _issueService.GetByIdAsync(issueId);
            if (dto == null)
                return RedirectToAction(nameof(Index));

            var model = IssueViewModelMapper.ToDetailsViewModel(dto);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIssueStatus(int issueId, string newStatus)
        {
            await _issueService.ChangeStatusAsync(issueId, Enum.Parse<Status>(newStatus));
            _logger.LogInformation("Changed status of issue {IssueId} to {StatusId}", issueId, newStatus);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditIssue(int issueId)
        {
            var dto = await _issueService.GetByIdAsync(issueId);
            if (dto == null)
                return RedirectToAction(nameof(Index));

            var users = await _userService.GetAllAsync();
            var model = IssueViewModelMapper.ToEditViewModel(dto, users, priorities);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditIssue(EditIssueViewModel formData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditIssue), new { issueId = formData.Id });

            var dto = IssueViewModelMapper.ToEditDto(formData);
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