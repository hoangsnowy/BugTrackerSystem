using System;

namespace BugTracker.Data.Models
{
    public class Status : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}