using BugTracker.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Data.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class UpdateDateIsEarlierThanAdditionDateAttribute : ValidationAttribute
  {
    public UpdateDateIsEarlierThanAdditionDateAttribute()
    {
      ErrorMessage = "The update date cannot be earlier than the date of addition";
    }

    public override bool IsValid(object? value)
    {
      return value is Models.Issue issue && issue.Created <= issue.Updated;
    }
  }
}