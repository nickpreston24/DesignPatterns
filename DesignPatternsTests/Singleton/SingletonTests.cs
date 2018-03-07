using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DesignPatterns.Tests
{
    [TestClass()]
    public class SingletonTests
    {
        [TestMethod]
        public void SingletonInstanceTest()
        {
            var app1 = Application.Instance;
            var app2 = Application.Instance;

            Debug.WriteLine(app1.Name);
            Debug.WriteLine(app2.Name);

            app1 = null;
            Assert.IsNull(app1);
            Assert.IsNotNull(app2);
        }
    }
}