using BugTracker.Business.DTOs;
using BugTracker.Business.Mapers;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace BugTracker.Business.Services
{
    public class UserService : IUserService
    {
        private readonly GenericRepository<User, ApplicationDbContext> _repo;
        private readonly ILogger<UserService> _logger;

        public UserService(
            GenericRepository<User, ApplicationDbContext> repo,
            ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllObjects();
            
            return users.Select(UserMapper.ToDto).ToList();
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var u = await _repo.GetObjectById(id);
            _logger.LogInformation("Get User #{UserId} ", id);
            return u is null ? null : UserMapper.ToDto(u);
        }

    }
}
