using BugTracker.Business.DTOs;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace BugTracker.Business.Services
{
    public class IssueService : IIssueService
    {
        private readonly EfCoreRepository<Issue, ApplicationDbContext> _issues;
        private readonly ILogger<IssueService> _logger;

        public IssueService(
            EfCoreRepository<Issue, ApplicationDbContext> issues,
            ILogger<IssueService> logger)
        {
            _issues = issues;
            _logger = logger;
        }

        public async Task<IEnumerable<IssueDto>> GetAllAsync(string search = null)
        {
            var entities = await _issues.GetAllObjects();
            var dtos = entities.Select(i => new IssueDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Created = i.Created,
                Updated = i.Updated,
                CreatedBy = i.CreatedBy.Login,
                AssignedTo = i.AssignedTo?.Login,
                AssignedToId = i.AssignedToId,
                PriorityId = i.PriorityId,
                Priority = i.Priority.Name,
                StatusId = i.StatusId,
                Status = i.Status.Name
            });

            if (string.IsNullOrWhiteSpace(search))
                return dtos;

            return dtos.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IssueDto> GetByIdAsync(int id)
        {
            var i = await _issues.GetObjectById(id);
            if (i == null) return null;
            return new IssueDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Created = i.Created,
                Updated = i.Updated,
                CreatedBy = i.CreatedBy.Login,
                AssignedTo = i.AssignedTo?.Login,
                AssignedToId = i.AssignedToId,
                PriorityId = i.PriorityId,
                Priority = i.Priority.Name,
                StatusId = i.StatusId,
                Status = i.Status.Name
            };
        }

        public async Task CreateAsync(CreateIssueDto form, string creatorId)
        {
            var issue = new Issue
            {
                Title = form.Title,
                Description = form.Description,
                Created = DateTime.UtcNow,
                CreatedById = creatorId,
                AssignedToId = form.AssignedToId,
                PriorityId = form.PriorityId,
                StatusId = 1 // default
            };
            await _issues.Create(issue);
            _logger.LogInformation("Issue #{IssueId} created", issue.Id);
        }

        public async Task UpdateAsync(EditIssueDto form)
        {
            var issue = new Issue
            {
                Id = form.Id,
                Title = form.Title,
                Description = form.Description,
                Updated = DateTime.UtcNow,
                AssignedToId = form.AssignedToId,
                PriorityId = form.PriorityId
            };
            await _issues.Update(issue);
            _logger.LogInformation("Issue #{IssueId} updated", issue.Id);
        }

        public async Task ChangeStatusAsync(int issueId, int statusId)
        {
            var issue = new Issue
            {
                Id = issueId,
                StatusId = statusId,
                Updated = DateTime.UtcNow
            };
            await _issues.Update(issue);
            _logger.LogInformation("Issue #{IssueId} status changed to #{StatusId}", issueId, statusId);
        }

        public async Task DeleteAsync(int issueId)
        {
            await _issues.Delete(issueId);
            _logger.LogInformation("Issue #{IssueId} deleted", issueId);
        }
    }
}
