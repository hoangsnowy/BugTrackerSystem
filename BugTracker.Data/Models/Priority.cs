using System;

namespace BugTracker.Data.Models
{
    public class Priority : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Priority) obj);
        }

        protected bool Equals(Priority other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}