using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Business.Services;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using BugTracker.TestSupport;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BugTracker.Business.Tests.Services
{
    [TestClass]
    public class IssueServiceTests
    {
        private IIssueRepository _repo;
        private TestLogger<IssueService> _logger;
        private IssueService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = Substitute.For<IIssueRepository>();
            _logger = new TestLogger<IssueService>();
            _service = new IssueService(_repo, _logger);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllMappedDtos_WhenNoSearch()
        {
            var now = DateTime.UtcNow;
            var issues = new List<Issue>
            {
                new Issue { Id = 1, Title = "A", Description = "DescA", Created = now, Updated = now, CreatedById = "u1", AssignedToId = "u2", Priority = (byte)Priority.Major, Status = (byte)Status.Open },
                new Issue { Id = 2, Title = "B", Description = "DescB", Created = now, Updated = now, CreatedById = "u3", AssignedToId = "u4", Priority = (byte)Priority.Minor, Status = (byte)Status.Closed }
            };
            // assign navigation props to avoid null reference in mapping
            foreach (var issue in issues)
            {
                issue.CreatedBy = new User { Login = issue.CreatedById };
                issue.AssignedTo = new User { Login = issue.AssignedToId };
            }
            _repo.GetAllObjects().Returns(Task.FromResult((IEnumerable<Issue>)issues));

            var dtos = (await _service.GetAllAsync()).ToList();

            Assert.AreEqual(2, dtos.Count);
            Assert.AreEqual(Priority.Major, dtos[0].Priority);
            Assert.AreEqual(Priority.Minor, dtos[1].Priority);
        }


        [TestMethod]
        public async Task GetAllAsync_FiltersBySearchTerm_CaseInsensitive()
        {
            var now = DateTime.UtcNow;
            var issues = new List<Issue>
            {
                new Issue { Id = 1, Title = "Alpha", Description = "Desc", Created = now, Updated = now, CreatedById = "u1", AssignedToId = "u2", Priority = (byte)Priority.Minor, Status = (byte)Status.Open },
                new Issue { Id = 2, Title = "Beta",  Description = "Desc", Created = now, Updated = now, CreatedById = "u1", AssignedToId = "u2", Priority = (byte)Priority.Minor, Status = (byte)Status.Open }
            };
            // assign navigation props to avoid null reference
            foreach (var issue in issues)
            {
                issue.CreatedBy = new User { Login = issue.CreatedById };
                issue.AssignedTo = new User { Login = issue.AssignedToId };
            }
            _repo.GetAllObjects().Returns(Task.FromResult((IEnumerable<Issue>)issues));

            var result = (await _service.GetAllAsync("alp")).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Alpha", result[0].Title);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsDto_WhenFound()
        {
            var now = DateTime.UtcNow;
            var issue = new Issue { Id = 5, Title = "X", Description = "Y", Created = now, Updated = now, CreatedById = "u1", AssignedToId = "u2", Priority = (byte)Priority.Medium, Status = (byte)Status.Open };
            issue.CreatedBy = new User { UserName = "creator" };
            issue.AssignedTo = new User { UserName = "assignee" };
            _repo.GetObjectById(5).Returns(Task.FromResult(issue));

            var dto = await _service.GetByIdAsync(5);

            Assert.IsNotNull(dto);
            Assert.AreEqual("creator", dto.CreatedBy);
            Assert.AreEqual("assignee", dto.AssignedTo);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repo.GetObjectById(123).Returns(Task.FromResult<Issue>(null));

            var dto = await _service.GetByIdAsync(123);

            Assert.IsNull(dto);
        }

        [TestMethod]
        public async Task CreateAsync_CreatesIssueWithCorrectFields_AndLogs()
        {
            var form = new CreateIssueDto { Title = "New", Description = "Test", AssignedToId = "u2", Priority = Priority.Minor };
            await _service.CreateAsync(form, "u1");

            await _repo.Received(1).Create(Arg.Any<Issue>());
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesIssueWithCorrectId_AndLogs()
        {
            var form = new EditIssueDto { Id = 20, Title = "Up", Description = "Desc", AssignedToId = "u3", Priority = Priority.Major };
            await _service.UpdateAsync(form);

            await _repo.Received(1).Update(Arg.Is<Issue>(i => i.Id == 20));
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task ChangeStatusAsync_ChangesStatusAndLogs()
        {
            await _service.ChangeStatusAsync(30, Status.Resolved);

            await _repo.Received(1).ChangeStatus(30, (byte)Status.Resolved);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesIssueAndLogs()
        {
            await _service.DeleteAsync(40);

            await _repo.Received(1).Delete(40);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }
    }
}
