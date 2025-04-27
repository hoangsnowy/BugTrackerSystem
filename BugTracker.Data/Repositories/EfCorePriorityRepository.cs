using BugTracker.Data.Models;

namespace BugTracker.Data.Repositories
{
    public class EfCorePriorityRepository : EfCoreRepository<Priority, ApplicationDbContext>
    {
        public EfCorePriorityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}