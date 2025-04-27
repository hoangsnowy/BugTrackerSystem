using BugTracker.Business.DTOs;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

namespace BugTracker.Business.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly EfCoreRepository<Priority, ApplicationDbContext> _repo;

        public PriorityService(EfCoreRepository<Priority, ApplicationDbContext> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PriorityDto>> GetAllAsync()
        {
            var list = await _repo.GetAllObjects();
            return list.Select(p => new PriorityDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public async Task<PriorityDto> GetByIdAsync(int id)
        {
            var p = await _repo.GetObjectById(id);
            return p is null ? null : new PriorityDto
            {
                Id = p.Id,
                Name = p.Name
            };
        }
    }
}
