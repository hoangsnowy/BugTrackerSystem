using System;

namespace BugTracker.Data.Models
{
    public class Priority : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}