using Microsoft.Extensions.Logging;
namespace BugTracker.TestSupport
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
