using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DesignPatterns.Tests
{
    [TestClass]
    public class SingletonTests
    {
        private static int maxCount = 132;

        [TestMethod]
        public void Can_Create_SingletonInstance()
        {
            var app1 = Application.Instance;
            var app2 = Application.Instance;

            Debug.WriteLine(app1.Name);
            Debug.WriteLine(app2.Name);

            app1 = null;
            Assert.IsNull(app1);
            Assert.IsNotNull(app2);
        }

        [TestMethod]
        public void Can_Inject_Singletons()
        {
            var logger = ClientLogger.Instance;
            logger.Name = "┬┴┬┴┤ ͜ʖ ͡°) ├┬┴┬┴";
            var logger2 = ClientLogger.Instance;

            var emailService = new EmailService(logger);
            emailService.Logger.Log("Hi, from the email service.");

            Assert.AreEqual(logger2.Name, logger.Name);
        }

        [TestMethod]
        public void Can_Use_SingletonSelector()
        {
            var logger = Multiton.GetInstance<ClientLogger>();
            ILogger emaillogger = Multiton.GetInstance<EmailLogger>();

            Debug.WriteLine(logger.Name);
            Debug.WriteLine(emaillogger.Name);

            emaillogger.Log("hi from email logger");
            emaillogger.Name = "new name";
            logger.Log("hello");
            Debug.WriteLine(logger.Name);
            Debug.WriteLine(emaillogger.Name);
        }

        [TestMethod]
        public void Can_CreateISingleton_of_T()
        {
            LunchBox lunchbox = LunchBox.Instance;

            Debug.WriteLine(lunchbox.OwnerName);
            lunchbox.Open();

            var loggerInstance = lunchbox.Get<ClientLogger>();

            Assert.IsNotNull(loggerInstance);

            Assert.IsTrue(lunchbox is ISingleton);

            string thiefName = "Jack", originalOwner = lunchbox.OwnerName;
            Assert.IsTrue(lunchbox.OwnerName.Equals(originalOwner));

            lunchbox.OwnerName = thiefName;
            Assert.IsTrue(lunchbox.OwnerName.Equals(thiefName));
        }

        [TestMethod]
        public void Can_CreateMultithreadSingletons()
        {
            EmailLogger.Instance.Name = "start";
            var thread = new Thread(Write1);
            thread.Start();
            Write2();
            Debug.WriteLine("Done");
        }

        private static void Write2()
        {
            foreach (int count in Enumerable.Range(1, maxCount))
            {
                var emailLogger = EmailLogger.Instance;

                if (!string.IsNullOrWhiteSpace(emailLogger.Name))
                {
                    emailLogger.Name = "mike";
                }

                emailLogger.Log($"Thread 2, count {count}");
            }
        }

        private static void Write1()
        {
            foreach (int count in Enumerable.Range(1, maxCount))
            {
                var emailLogger = EmailLogger.Instance;
                if (!string.IsNullOrWhiteSpace(emailLogger.Name))
                {
                    emailLogger.Name = "bob";
                }

                emailLogger.Log($"Thread 1, count {count}");
            }
        }

        //[TestMethod]
        //public void CreateSingletonInterfaceInstance()
        //{
        //    throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
        //}
    }
}