using Microsoft.AspNetCore.Identity;

namespace BugTracker.Data.Models
{
    public class User : IdentityUser, IEntity
    {
        public override string Id { get; set; } = null!;

        public string Login { get; set; } = null!;
    }
}