using System.Diagnostics;
using System.Linq;
using System.Threading;
using DesignPatterns.Singletons;
using DP.Tests.Singletons.Classes;
using Xunit;

namespace DesignPatterns.Tests
{
    public class SingletonTests
    {
        static int maxCount = 132;

        [Fact]
        public void CanCreateSingletonInstance()
        {
            var app1 = Application.Instance;
            var app2 = Application.Instance;

            Debug.WriteLine(app1.Name);
            Debug.WriteLine(app2.Name);

            app1 = null;
            Assert.Null(app1);
            Assert.NotNull(app2);
        }

        [Fact]
        public void CanInjectSingletons()
        {
            ClientLogger logger = ClientLogger.Instance;
            logger.Provider = "┬┴┬┴┤ ͜ʖ ͡°) ├┬┴┬┴";
            var logger2 = ClientLogger.Instance;

            var emailService = new EmailService(logger);
            emailService.Logger.Log("Hi, from the email service.");

            Assert.Equal(logger2.Provider, logger.Provider);
        }

        [Fact]
        public void Can_Use_SingletonSelector()
        {
            var logger = Multiton.GetInstance<ClientLogger>();
            ILogger emaillogger = Multiton.GetInstance<EmailLogger>();

            Debug.WriteLine(logger.Provider);
            Debug.WriteLine(emaillogger.Provider);

            emaillogger.Log("hi from email logger");
            emaillogger.Provider = "new name";
            logger.Log("hello");
            Debug.WriteLine(logger.Provider);
            Debug.WriteLine(emaillogger.Provider);
        }

        [Fact]
        public void Can_CreateISingleton_of_T()
        {
            LunchBox lunchbox = LunchBox.Instance;

            Debug.WriteLine(lunchbox.OwnerName);
            lunchbox.Open();

            var loggerInstance = lunchbox.Get<ClientLogger>();

            Assert.NotNull(loggerInstance);

            Assert.True(lunchbox is ISingleton);

            string thiefName = "Jack", originalOwner = lunchbox.OwnerName;
            Assert.Equal(lunchbox.OwnerName,originalOwner);

            lunchbox.OwnerName = thiefName;
            Assert.Equal(lunchbox.OwnerName,thiefName);
        }

        [Fact]
        public void Can_CreateMultithreadSingletons()
        {
            EmailLogger.Instance.Provider = "start";
            var thread = new Thread(Write1);
            thread.Start();
            Write2();
            Debug.WriteLine("Done");
        }

        static void Write2()
        {
            foreach (int count in Enumerable.Range(1, maxCount))
            {
                var emailLogger = EmailLogger.Instance;

                if (!string.IsNullOrWhiteSpace(emailLogger.Provider))
                {
                    emailLogger.Provider = "mike";
                }

                emailLogger.Log($"Thread 2, count {count}");
            }
        }

        static void Write1()
        {
            foreach (int count in Enumerable.Range(1, maxCount))
            {
                var emailLogger = EmailLogger.Instance;
                if (!string.IsNullOrWhiteSpace(emailLogger.Provider))
                {
                    emailLogger.Provider = "bob";
                }

                emailLogger.Log($"Thread 1, count {count}");
            }
        }
    }
}