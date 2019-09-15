using DynamicXmlTests.TestClasses;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustMock = Telerik.JustMock;

namespace DynamicXmlTests
{
    [TestClass]
    internal class SampleMockingTests
    {
        [TestMethod]
        public void EasyMockTest()
        {
            //could not find tutorial!
        }

        [TestMethod]
        public void MoqTest()
        {
            var mockContainer = new Moq.Mock<IContainer>();
            var mockView = new Moq.Mock<ICustomerView>();
        }

        [TestMethod]
        public void JustMockTest()
        {
            ///Arrange:
            ICurrencyService currencyService = JustMock.Mock.Create<ICurrencyService>();
            JustMock.Mock.Arrange(() => currencyService.GetConversionRate("GBP", "CAD")).Returns(2.20m).MustBeCalled();

            var accountService = new AccountService(currencyService);

            var canadianAccount = new Account(0, "CAD");
            var britishAccount = new Account(0, "GBP");
            britishAccount.Deposit(100);
            accountService.TransferFunds(britishAccount, canadianAccount, 100);

            Assert.AreEqual(0, britishAccount.Balance);
            Assert.AreEqual(220m, canadianAccount.Balance);
            JustMock.Mock.Assert(currencyService);
        }

        [TestMethod]
        //from: http://fakeiteasy.readthedocs.io/en/stable/quickstart/
        public void FakeItEasyTest()
        {
            // make some fakes for the test
            var lollipop = A.Fake<ICandy>();
            var shop = A.Fake<ICandyShop>();

            // set up a call to return a value
            A.CallTo(() => shop.GetTopSellingCandy()).Returns(lollipop);

            // use the fake as an actual instance of the faked type
            var developer = A.Fake<SweetTooth>();
            //var developer = new SweetTooth();
            developer.BuyTastiestCandy(shop);

            // asserting uses the exact same syntax as when configuring calls—
            // no need to learn another syntax

            //both fail:
            //A.CallTo(() => shop.BuyCandy(lollipop)).MustHaveHappened();
            //A.CallTo(() => developer.BuyTastiestCandy(shop)).MustHaveHappened();
        }

        [TestMethod]
        public void FakeItAgain()
        {
            var candy = A.Fake<ICandy>();
            //candy.Dump();
        }
    }
}