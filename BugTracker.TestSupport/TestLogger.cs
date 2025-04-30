using Microsoft.Extensions.Logging;

namespace BugTracker.TestSupport
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<LogEntry> LogEntries { get; } = new List<LogEntry>();

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            LogEntries.Add(new LogEntry
            {
                LogLevel = logLevel,
                Message = formatter(state, exception),
                Exception = exception
            });
        }
    }

}
