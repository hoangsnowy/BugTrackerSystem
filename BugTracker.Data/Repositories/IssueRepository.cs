using BugTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data.Repositories
{
    public class IssueRepository : GenericRepository<Issue, ApplicationDbContext>
    {
        public IssueRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public override async Task<Issue> Update(Issue item)
        {
            var issue = GetObjectById(item.Id).Result;

            if (issue == null) 
                return null;
            
            issue.Title = string.IsNullOrEmpty(item.Title) ? issue.Title : item.Title;
            issue.Description = string.IsNullOrEmpty(item.Description) ? issue.Description : item.Description;
            issue.Priority = item.Priority;
            issue.Status = item.Status;
            issue.AssignedToId = string.IsNullOrEmpty(item.AssignedToId) ? issue.AssignedToId : item.AssignedToId;
            issue.Updated = DateTime.Now;
            
            Context.Entry(issue).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return issue;
        }
    }
}