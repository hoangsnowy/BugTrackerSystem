using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Business.Mapers;
using BugTracker.Data.Models;

namespace BugTracker.Business.Tests.Mappers
{
    [TestClass]
    public class IssueMapperTests
    {
        [TestMethod]
        public void ToDto_ShouldMapAllFields()
        {
            var issue = new Issue
            {
                Id = 1,
                Title = "T",
                Description = "D",
                Created = new DateTime(2025, 1, 1),
                Updated = new DateTime(2025, 1, 2),
                CreatedBy = new User { UserName = "creator" },
                AssignedTo = new User { UserName = "assignee" },
                AssignedToId = "a1",
                Priority = (byte)Priority.Medium,
                Status = (byte)Status.Open
            };

            var dto = IssueMapper.ToDto(issue);

            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("T", dto.Title);
            Assert.AreEqual("D", dto.Description);
            Assert.AreEqual(issue.Created, dto.Created);
            Assert.AreEqual(issue.Updated, dto.Updated);
            Assert.AreEqual("creator", dto.CreatedBy);
            Assert.AreEqual("assignee", dto.AssignedTo);
            Assert.AreEqual("a1", dto.AssignedToId);
            Assert.AreEqual(Priority.Medium, dto.Priority);
            Assert.AreEqual(Status.Open, dto.Status);
        }

        [TestMethod]
        public void Create_ShouldSetFields()
        {
            var form = new CreateIssueDto
            {
                Title = "T",
                Description = "D",
                AssignedToId = "u2",
                Priority = Priority.Minor
            };
            var creatorId = "u1";

            var issue = IssueMapper.Create(form, creatorId);

            Assert.AreEqual("T", issue.Title);
            Assert.AreEqual("D", issue.Description);
            Assert.AreEqual("u1", issue.CreatedById);
            Assert.AreEqual("u2", issue.AssignedToId);
            Assert.AreEqual((byte)Priority.Minor, issue.Priority);
            Assert.AreEqual((byte)Status.Open, issue.Status);
            Assert.IsTrue((DateTime.UtcNow - issue.Created).TotalSeconds < 5);
        }

        [TestMethod]
        public void Update_ShouldSetUpdatedFields()
        {
            var form = new EditIssueDto
            {
                Id = 2,
                Title = "T2",
                Description = "D2",
                AssignedToId = "u3",
                Priority = Priority.Medium
            };

            var issue = IssueMapper.Update(form);

            Assert.AreEqual(2, issue.Id);
            Assert.AreEqual("T2", issue.Title);
            Assert.AreEqual("D2", issue.Description);
            Assert.AreEqual("u3", issue.AssignedToId);
            Assert.AreEqual((byte)Priority.Medium, issue.Priority);
            Assert.IsTrue((DateTime.UtcNow - issue.Updated)?.TotalSeconds < 5);
        }

        [TestMethod]
        public void ChangeStatus_ShouldSetStatusAndUpdated()
        {
            var issue = IssueMapper.ChangeStatus(3, Status.Closed);

            Assert.AreEqual(3, issue.Id);
            Assert.AreEqual((byte)Status.Closed, issue.Status);
            Assert.IsTrue((DateTime.UtcNow - issue.Updated)?.TotalSeconds < 5);
        }
    }
}
