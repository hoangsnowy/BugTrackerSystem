using BugTracker.Data.Attributes;
using Microsoft.AspNetCore.Identity;
using System;

namespace BugTracker.Data.Models
{
    [LoginEmailEqual]
    public class User : IdentityUser, IEntity
    {
        public override string Id { get; set; } = null!;

        public string Login { get; set; } = null!;
    }
}