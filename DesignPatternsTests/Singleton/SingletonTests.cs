using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DesignPatterns.Tests
{
    [TestClass]
    public class SingletonTests
    {
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
            var logger = SingletonSelector.GetInstance<ClientLogger>();
            ILogger emaillogger = SingletonSelector.GetInstance<EmailLogger>();

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
            var selector = lunchbox.Selector;

            Debug.WriteLine(lunchbox.OwnerName);
            lunchbox.Open();

            var loggerInstance = selector.GetInstance<ClientLogger>();

            Assert.IsNotNull(selector);
            Assert.IsNotNull(loggerInstance);

            Assert.IsTrue(lunchbox is ISingleton);
            Assert.IsTrue(lunchbox is ISingleton<Apple>);

            string thiefName = "Jack", originalOwner = lunchbox.OwnerName;
            Assert.IsTrue(lunchbox.OwnerName.Equals(originalOwner));

            lunchbox.OwnerName = thiefName;
            Assert.IsTrue(lunchbox.OwnerName.Equals(thiefName));

        }

        //[TestMethod]
        //public void CreateSingletonInterfaceInstance()
        //{
        //    throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
        //}
    }
}