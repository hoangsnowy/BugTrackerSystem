using BugTracker.Business.DTOs;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

namespace BugTracker.Business.Services
{
    public class UserService : IUserService
    {
        private readonly GenericRepository<User, ApplicationDbContext> _repo;

        public UserService(GenericRepository<User, ApplicationDbContext> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllObjects();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Login = u.Login
            }).ToList();
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var u = await _repo.GetObjectById(id);
            return u is null ? null : new UserDto
            {
                Id = u.Id,
                Login = u.Login
            };
        }
    }
}
