using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Business.Services;
using BugTracker.Data.Models;
using BugTracker.TestSupport;
using BugTracker.Web.Controllers;
using BugTracker.Web.Tests.Fakers;
using BugTracker.Web.ViewModels.Issue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Security.Claims;
namespace BugTracker.Web.Tests.Controllers
{
    [TestClass]
    public class IssueControllerTests
    {
        private IIssueService _issueService;
        private IUserService _userService;
        private UserManager<User> _userManager;
        private TestLogger<IssueController> _logger;
        private IssueController _controller;

        [TestInitialize]
        public void Setup()
        {
            _issueService = Substitute.For<IIssueService>();
            _userService = Substitute.For<IUserService>();
            _logger = new TestLogger<IssueController>();

            var testUser = new User { Id = "u1", UserName = "user1" };
            _userManager = new FakeUserManager(testUser);

            _controller = new IssueController(_logger, _issueService, _userService, _userManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, testUser.Id) }))
                    }
                }
            };
        }

        [TestMethod]
        public async Task Index_NoSearch_ReturnsViewWithModels()
        {
            var now = DateTime.UtcNow;
            var dtos = new List<IssueDto>
            {
                new IssueDto
                {
                    Id = 1,
                    Title = "T1",
                    Description = "D1",
                    Created = now,
                    Updated = now,
                    CreatedBy = "c",
                    AssignedTo = "a",
                    AssignedToId = "u2",
                    Priority = Priority.Major,
                    Status = Status.Open
                }
            };
            _issueService.GetAllAsync(null).Returns(Task.FromResult((IEnumerable<IssueDto>)dtos));

            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<IssueViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("T1", model[0].Title);
        }

        [TestMethod]
        public async Task Index_WithSearch_PassesSearchParameter()
        {
            _issueService.GetAllAsync("search").Returns(Task.FromResult(Enumerable.Empty<IssueDto>()));

            await _controller.Index("search");

            await _issueService.Received(1).GetAllAsync("search");
        }

        [TestMethod]
        public async Task CreateIssue_Get_ReturnsViewWithUsersAndPriorities()
        {
            var users = new List<UserDto> { new UserDto { Id = "u2", UserName = "user2" } };
            _userService.GetAllAsync().Returns(Task.FromResult((IEnumerable<UserDto>)users));

            var result = await _controller.CreateIssue() as ViewResult;
            var model = result?.Model as CreateIssueViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            CollectionAssert.AreEquivalent(users.ToList(), model.Users.ToList());
            Assert.IsTrue(model.Priorities.Any());
        }

        [TestMethod]
        public async Task CreateIssue_Post_InvalidModel_RedirectsToCreate()
        {
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.CreateIssue(new CreateIssueViewModel()) as RedirectToActionResult;

            Assert.AreEqual(nameof(IssueController.CreateIssue), result?.ActionName);
        }

        [TestMethod]
        public async Task CreateIssue_Post_Valid_CreatesAndRedirects()
        {
            var form = new CreateIssueViewModel
            {
                Title = "T",
                Description = "D",
                AssignedToId = "u2",
                PriorityId = Priority.Major.ToString()
            };

            var result = await _controller.CreateIssue(form) as RedirectToActionResult;

            await _issueService.Received(1).CreateAsync(Arg.Any<CreateIssueDto>(), "u1");
            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task DetailIssue_Get_NotFound_RedirectsToIndex()
        {
            _issueService.GetByIdAsync(5).Returns(Task.FromResult<IssueDto>(null));

            var result = await _controller.DetailIssue(5) as RedirectToActionResult;

            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
        }

        [TestMethod]
        public async Task DetailIssue_Get_Found_ReturnsView()
        {
            var dto = new IssueDto
            {
                Id = 5,
                Title = "X",
                Description = "Y",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                CreatedBy = "c",
                AssignedTo = "a",
                AssignedToId = "u2",
                Priority = Priority.Minor,
                Status = Status.Open
            };
            _issueService.GetByIdAsync(5).Returns(Task.FromResult(dto));

            var result = await _controller.DetailIssue(5) as ViewResult;
            var model = result?.Model as IssueDetailsViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(5, model.Id);
        }

        [TestMethod]
        public async Task ChangeIssueStatus_Post_CallsService_AndRedirects()
        {
            var result = await _controller.ChangeIssueStatus(7, Status.Closed.ToString()) as RedirectToActionResult;

            await _issueService.Received(1).ChangeStatusAsync(7, Status.Closed);
            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task EditIssue_Get_NotFound_RedirectsToIndex()
        {
            _issueService.GetByIdAsync(3).Returns(Task.FromResult<IssueDto>(null));

            var result = await _controller.EditIssue(3) as RedirectToActionResult;

            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
        }

        [TestMethod]
        public async Task EditIssue_Get_Found_ReturnsView()
        {
            var dto = new IssueDto { Id = 3, Title = "T3", Description = "D3", Created = DateTime.UtcNow, Updated = DateTime.UtcNow, CreatedBy = "c", AssignedTo = "a", AssignedToId = "u2", Priority = Priority.Major, Status = Status.Open };
            _issueService.GetByIdAsync(3).Returns(Task.FromResult(dto));
            var users = new List<UserDto> { new UserDto { Id = "u2", UserName = "u2" } };
            _userService.GetAllAsync().Returns(Task.FromResult((IEnumerable<UserDto>)users));

            var result = await _controller.EditIssue(3) as ViewResult;
            var model = result?.Model as EditIssueViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Id);
            CollectionAssert.AreEqual(users.ToList(), model.Users.ToList());
            Assert.IsTrue(model.Priorities.Any());
        }

        [TestMethod]
        public async Task EditIssue_Post_InvalidModel_RedirectsToEdit()
        {
            var form = new EditIssueViewModel { Id = 9 };
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.EditIssue(form) as RedirectToActionResult;

            Assert.AreEqual(nameof(IssueController.EditIssue), result?.ActionName);
            Assert.AreEqual(9, result?.RouteValues["issueId"]);
        }

        [TestMethod]
        public async Task EditIssue_Post_Valid_CallsService_AndRedirects()
        {
            var form = new EditIssueViewModel { Id = 9, Title = "T9", Description = "D9", AssignedToId = "u2", PriorityId = Priority.Minor.ToString() };

            var result = await _controller.EditIssue(form) as RedirectToActionResult;

            await _issueService.Received(1).UpdateAsync(Arg.Any<EditIssueDto>());
            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task DeleteIssue_Get_CallsService_AndRedirects()
        {
            var result = await _controller.DeleteIssue(15) as RedirectToActionResult;

            await _issueService.Received(1).DeleteAsync(15);
            Assert.AreEqual(nameof(IssueController.Index), result?.ActionName);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }
    }
}

