using System.Diagnostics;

namespace DesignPatterns.Tests
{
    class EmailService : IEmailService
    {
        public ILogger Logger { get; }

        public EmailService(ILogger logger)
        {
            Logger = logger;
            Debug.WriteLine(logger.Name);
        }
    }
}