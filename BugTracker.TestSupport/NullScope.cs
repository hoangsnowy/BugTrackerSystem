namespace BugTracker.TestSupport
{
    // Minimal NullScope implementation
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        private NullScope() { }
        public void Dispose() { }
    }
}
