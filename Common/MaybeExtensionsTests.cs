using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Tests
{
    [TestClass()]
    public class MaybeExtensionsTests
    {
        [TestMethod()]
        public void ToMaybeTest()
        {
            var obj = new object();
            obj = null;
            var maybe = obj.ToMaybe();
            maybe.Case(_ => Debug.WriteLine("Found some"),
                       () => Debug.WriteLine("Nullable found"));
        }

        [TestMethod()]
        public void NullableToMaybeTest()
        {
            int? s = null;
            var maybe = s.ToMaybe();
            maybe.Case(_ => Debug.WriteLine("Found some"),
                       () => Debug.WriteLine("Nullable found"));
        }

        [TestMethod()]
        public void NoneIfEmptyTest()
        {
            var empty = new string[] { };
            var maybe = empty.ToMaybe();
            maybe.Case(_ => Debug.WriteLine("Found some"), () => Assert.Fail());
        }

        [TestMethod()]
        public void FirstOrNoneTest()
        {
            var list = new List<object> { "string", 'c', null };
            var maybe = list.FirstOrNone();
            Assert.IsTrue(maybe.HasValue);
        }

        [TestMethod()]
        public void ReturnTest()
        {
            //TODO: You left this blank; figure out what you meant by Return and implement it.
        }
    }
}