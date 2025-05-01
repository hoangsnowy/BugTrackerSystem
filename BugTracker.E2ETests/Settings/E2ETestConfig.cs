namespace BugTracker.E2ETests.Settings
{
    /// <summary>
    /// Holds configuration values for E2E tests.
    /// <para>Reads the base URL from the environment variable <c>E2E_BASE_URL</c>,
    /// defaulting to <c>http://localhost:5000</c> if not set.</para>
    /// </summary>
    public static class E2ETestConfig
    {
        public static readonly string BaseUrl =
            System.Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:5000";
    }
}
