using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.Attributes
{
    public class UpdateDateIsEarlierThanAdditionDateAttribute : ValidationAttribute
    {
        public UpdateDateIsEarlierThanAdditionDateAttribute()
        {
            ErrorMessage = "The update date cannot be earlier than the date of addition";
        }
        
        public override bool IsValid(object value)
        {
            return value is BugTracker.Data.Models.Issue issue && issue.Created <= issue.Updated;
        }
    }
}