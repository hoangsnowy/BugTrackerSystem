﻿using BugTracker.Business.DTOs;
using BugTracker.Business.Enums;

namespace BugTracker.Business.Services
{
    public interface IIssueService
    {
        Task<IEnumerable<IssueDto>> GetAllAsync(string search = null);
        Task<IssueDto> GetByIdAsync(int id);
        Task CreateAsync(CreateIssueDto form, string creatorId);
        Task UpdateAsync(EditIssueDto form);
        Task ChangeStatusAsync(int issueId, Status status);
        Task DeleteAsync(int issueId);
    }
}