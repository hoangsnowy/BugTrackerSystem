using BugTracker.Business.Services;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using BugTracker.TestSupport;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BugTracker.Business.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private GenericRepository<User, ApplicationDbContext> _repo;
        private TestLogger<UserService> _logger;
        private UserService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = Substitute.For<GenericRepository<User, ApplicationDbContext>>((ApplicationDbContext)null);
            _logger = new TestLogger<UserService>();
            _service = new UserService(_repo, _logger);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsMappedUserDtos()
        {
            var users = new List<User> { new User { Id = "1", UserName = "a" }, new User { Id = "2", UserName = "b" } };
            _repo.GetAllObjects().Returns(Task.FromResult((IEnumerable<User>)users));

            var dtos = (await _service.GetAllAsync()).ToList();

            Assert.AreEqual(2, dtos.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_LogsAndReturnsDto_WhenFound()
        {
            var user = new User { Id = "x", UserName = "john" };
            _repo.GetObjectById("x").Returns(Task.FromResult(user));

            var dto = await _service.GetByIdAsync("x");

            Assert.IsNotNull(dto);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task GetByIdAsync_LogsAndReturnsNull_WhenNotFound()
        {
            _repo.GetObjectById("y").Returns(Task.FromResult<User>(null));

            var dto = await _service.GetByIdAsync("y");

            Assert.IsNull(dto);
            Assert.AreEqual(1, _logger.LogEntries.Count(e => e.LogLevel == LogLevel.Information));
        }
    }
}
