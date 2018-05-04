using Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;

namespace DesignPatterns.Tests
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void CreateSingletonInstance()
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
        public void InjectSingletons()
        {
            var logger = ClientLogger.Instance;
            logger.Name = "┬┴┬┴┤ ͜ʖ ͡°) ├┬┴┬┴";
            var logger2 = ClientLogger.Instance;

            var emailService = new EmailService(logger);
            emailService.Logger.Log("Hi, from the email service.");

            Assert.AreEqual(logger2.Name, logger.Name);
        }

        [TestMethod]
        public void CanUseSingletonSelector()
        {
            var logger = SingletonSelector.GetInstance<ClientLogger>();
            ILogger emaillogger = SingletonSelector.GetInstance<EmailLogger>();

            Debug.WriteLine(logger.Name);
            Debug.WriteLine(emaillogger.Name);

            emaillogger.Log("hi");
            emaillogger.Name = "new name";
            logger.Log("hello");
            Debug.WriteLine(logger.Name);
            Debug.WriteLine(emaillogger.Name);

        }

        [TestMethod]
        public void CreateSingletonInterfaceInstance()
        {
            throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
        }
    }
}