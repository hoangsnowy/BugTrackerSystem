using BugTracker.Data.Models;
using BugTracker.TestSupport;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Security.Claims;

namespace BugTracker.Web.Tests.Fakers
{
    // A stub UserManager that always returns the provided user
    // Stub UserManager that returns a fixed user
    public class FakeUserManager : UserManager<User>
    {
        private readonly User _user;
        public FakeUserManager(User user)
            : base(
                Substitute.For<IUserStore<User>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<User>>(),
                new List<IUserValidator<User>>(),
                new List<IPasswordValidator<User>>(),
                Substitute.For<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Substitute.For<IServiceProvider>(),
                new TestLogger<UserManager<User>>())
        {
            _user = user;
        }

        public override Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(_user);
        }
    }

}
