using BugTracker.Data.Models;

namespace BugTracker.Data.Repositories
{
    public class UserRepository : GenericRepository<User, ApplicationDbContext>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}