using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;
using BugTracker.Business.Mapers;
using BugTracker.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace BugTracker.Business.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issuesRepository;
        private readonly ILogger<IssueService> _logger;

        public IssueService(IIssueRepository issuesRepository,
            ILogger<IssueService> logger)
        {
            _issuesRepository = issuesRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<IssueDto>> GetAllAsync(string search = null)
        {
            var dtos = (await _issuesRepository.GetAllObjects())
                           .Select(IssueMapper.ToDto);
            return string.IsNullOrWhiteSpace(search)
                ? dtos
                : dtos.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IssueDto> GetByIdAsync(int id)
        {
            var issue = await _issuesRepository.GetObjectById(id);
            return issue == null ? null : IssueMapper.ToDto(issue);
        }

        public async Task CreateAsync(CreateIssueDto form, string creatorId)
        {
            var issue = IssueMapper.Create(form, creatorId);
            await _issuesRepository.Create(issue);
            _logger.LogInformation("Issue #{IssueId} created", issue.Id);
        }

        public async Task UpdateAsync(EditIssueDto form)
        {
            var issue = IssueMapper.Update(form);
            await _issuesRepository.Update(issue);
            _logger.LogInformation("Issue #{IssueId} updated", issue.Id);
        }

        public async Task ChangeStatusAsync(int issueId, Status status)
        {
            var issue = IssueMapper.ChangeStatus(issueId, status);
            await _issuesRepository.ChangeStatus(issueId, (byte)status);
            _logger.LogInformation("Issue #{IssueId} status changed to #{Status}", issueId, status);
        }

        public async Task DeleteAsync(int issueId)
        {
            await _issuesRepository.Delete(issueId);
            _logger.LogInformation("Issue #{IssueId} deleted", issueId);
        }
    }
}
