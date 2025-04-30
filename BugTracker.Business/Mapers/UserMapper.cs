using BugTracker.Business.DTOs;
using BugTracker.Data.Models;

namespace BugTracker.Business.Mapers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User u) => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName
        };
    }
}
