using Microsoft.Extensions.Logging;

namespace FuncCountdown.Utilities
{
    /// <summary>
    /// NOT RECOMMENDED. THIS NEEDS TO BE CHANGED
    /// </summary>
    public static class ApplicationLogger
    {
        public static ILogger logger;

        static ApplicationLogger()
        {
            logger = new LoggerFactory().CreateLogger("ApplicationLogger");
        }
    }
}
