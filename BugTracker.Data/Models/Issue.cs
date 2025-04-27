namespace BugTracker.Data.Models
{
    public class Issue : IEntity
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        
        public string CreatedById { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        
        public string? AssignedToId { get; set; }
        public virtual User? AssignedTo { get; set; }
        
        public byte Priority { get; set; } 
        
        public byte Status { get; set; }
    }
}