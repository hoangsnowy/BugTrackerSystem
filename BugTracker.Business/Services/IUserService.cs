using BugTracker.Business.DTOs;

namespace BugTracker.Business.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(string id);
    }
}
