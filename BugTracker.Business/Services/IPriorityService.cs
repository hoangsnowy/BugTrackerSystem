using BugTracker.Business.DTOs;

namespace BugTracker.Business.Services
{
    public interface IPriorityService
    {
        Task<IEnumerable<PriorityDto>> GetAllAsync();
        Task<PriorityDto> GetByIdAsync(int id);
    }
}
