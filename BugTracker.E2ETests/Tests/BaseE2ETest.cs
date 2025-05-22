using Microsoft.Playwright;

namespace BugTracker.E2ETests.Tests
{
    /// <summary>
    /// Base class for all E2E tests, responsible for Playwright lifecycle.
    /// </summary>
    [TestClass]
    public abstract class BaseE2ETest
    {
        /// <summary>The shared Playwright driver instance.</summary>
        protected static IPlaywright Playwright;
        /// <summary>The shared browser instance used across tests.</summary>
        protected static IBrowser Browser;

        /// <summary>
        /// Initializes Playwright and launches the browser once per test assembly.
        /// </summary>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext _)
        {
            // initialize Playwright synchronously for MSTest
            Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            // Note: Ensure you've run 'dotnet tool install --global Microsoft.Playwright.CLI' and 'playwright install' before running tests
            Browser = Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 2000
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Closes the browser and disposes the Playwright driver.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Browser.CloseAsync().GetAwaiter().GetResult();
            Playwright.Dispose();
        }

        /// <summary>
        /// Creates a new page context for each test.
        /// Ensures a clean slate with HTTPS errors ignored.
        /// </summary>
        protected Task<IPage> NewPageAsync()
        {
            return Browser.NewPageAsync(new BrowserNewPageOptions
            {
                IgnoreHTTPSErrors = true
            });
        }
    }
}
