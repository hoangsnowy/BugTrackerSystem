using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Web.Mappers;
using BugTracker.Web.ViewModels;
using BugTracker.Web.ViewModels.Issue;

namespace BugTracker.Web.Tests.Mappers
{
    [TestClass]
    public class IssueViewModelMapperTests
    {
        [TestMethod]
        public void ToListViewModel_MapsFieldsCorrectly()
        {
            var dto = new IssueDto
            {
                Id = 10,
                Title = "Test Title",
                Description = "Desc",
                Created = new DateTime(2025, 5, 1),
                Updated = new DateTime(2025, 5, 2),
                CreatedBy = "creator",
                AssignedTo = "assignee",
                AssignedToId = "u2",
                Priority = Priority.Major,
                Status = Status.Open
            };

            var vm = IssueViewModelMapper.ToListViewModel(dto);

            Assert.AreEqual(10, vm.Id);
            Assert.AreEqual("Test Title", vm.Title);
            Assert.AreEqual("Desc", vm.Description);
            Assert.AreEqual(dto.Created, vm.Created);
            Assert.AreEqual(dto.Updated, vm.Updated);
            Assert.AreEqual("creator", vm.CreatedBy);
            Assert.AreEqual("assignee", vm.AssignedTo);
            Assert.AreEqual(Priority.Major.ToString(), vm.Priority);
            Assert.AreEqual(Status.Open.ToString(), vm.Status);
        }

        [TestMethod]
        public void ToListViewModels_MapsEnumerableCorrectly()
        {
            var dtos = new[]
            {
                new IssueDto { Id =1, Title ="A", Description="D", Created=DateTime.UtcNow, Updated=DateTime.UtcNow, CreatedBy="c", AssignedTo="a", Priority=Priority.Minor, Status=Status.Closed }
            };

            var vms = IssueViewModelMapper.ToListViewModels(dtos);
            Assert.AreEqual(1, vms.Count);
            Assert.AreEqual(1, vms[0].Id);
        }

        [TestMethod]
        public void ToDetailsViewModel_MapsFieldsCorrectly()
        {
            var dto = new IssueDto
            {
                Id = 5,
                Title = "T",
                Description = "Desc",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                CreatedBy = "u1",
                AssignedTo = "u2",
                Priority = Priority.Medium,
                Status = Status.Closed
            };

            var vm = IssueViewModelMapper.ToDetailsViewModel(dto);

            Assert.AreEqual(dto.Id, vm.Id);
            Assert.AreEqual(dto.Title, vm.Title);
            Assert.AreEqual(dto.Description, vm.Description);
            Assert.AreEqual(dto.Created, vm.Created);
            Assert.AreEqual(dto.Updated, vm.Updated);
            Assert.AreEqual(dto.CreatedBy, vm.CreatedBy);
            Assert.AreEqual(dto.AssignedTo, vm.AssignedTo);
            Assert.AreEqual(dto.Priority.ToString(), vm.Priority);
            Assert.AreEqual(dto.Status.ToString(), vm.Status);
        }

        [TestMethod]
        public void ToCreateDto_MapsViewModelToDtoCorrectly()
        {
            var vm = new CreateIssueViewModel
            {
                Title = "New",
                Description = "Desc",
                AssignedToId = "u3",
                PriorityId = Priority.Major.ToString()
            };
            var currentUserId = "u1";

            var dto = IssueViewModelMapper.ToCreateDto(vm, currentUserId);

            Assert.AreEqual(vm.Title, dto.Title);
            Assert.AreEqual(vm.Description, dto.Description);
            Assert.AreEqual(vm.AssignedToId, dto.AssignedToId);
            Assert.AreEqual(Priority.Major, dto.Priority);
        }

        [TestMethod]
        public void ToEditViewModel_AndBack_ToEditDto_Works()
        {
            var dto = new IssueDto
            {
                Id = 7,
                Title = "T7",
                Description = "D7",
                AssignedToId = "u8",
                Priority = Priority.Medium
            };
            var users = new List<UserDto> { new UserDto { Id = "u8", UserName = "user8" } };
            var priorities = new List<PriorityViewModel>
            {
                new PriorityViewModel(Priority.Minor.ToString(), Priority.Minor.ToString()),
                new PriorityViewModel(Priority.Medium.ToString(), Priority.Medium.ToString())
            };

            var vm = IssueViewModelMapper.ToEditViewModel(dto, users, priorities);
            Assert.AreEqual(dto.Id, vm.Id);
            Assert.AreEqual(dto.Title, vm.Title);
            Assert.AreEqual(dto.Description, vm.Description);
            Assert.AreEqual(dto.AssignedToId, vm.AssignedToId);
            Assert.AreEqual(dto.Priority.ToString(), vm.PriorityId);
            Assert.AreSame(users, vm.Users);
            Assert.AreSame(priorities, vm.Priorities);

            var dto2 = IssueViewModelMapper.ToEditDto(vm);
            Assert.AreEqual(vm.Id, dto2.Id);
            Assert.AreEqual(vm.Title, dto2.Title);
            Assert.AreEqual(vm.Description, dto2.Description);
            Assert.AreEqual(vm.AssignedToId, dto2.AssignedToId);
            Assert.AreEqual(Priority.Medium, dto2.Priority);
        }
    }
}
