using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Tests
{
    [TestClass()]
    public class MaybeExtensionsTests
    {
        private const string successfullyNull = "Success! Nothing found.";
        private const string shouldBeNothing = "Failure! Found some value it should be nothing.";

        private const string success = "Success! Found some value.";
        private const string failure = "Failure! Nothing found!";

        private void Validate(object obj, bool valueIsRequired = true)
        {
            var maybe = obj.ToMaybe();

            if (!valueIsRequired)
                maybe.Case(_ => Assert.Fail(shouldBeNothing),
                () =>
                {
                    Debug.WriteLine(successfullyNull);
                    Assert.IsTrue(true);
                });
            else
                maybe.Case(_ =>
                {
                    Debug.WriteLine(success);
                    Assert.IsTrue(true);
                },
                () => Assert.Fail(failure));
        }

        [TestMethod()]
        public void ToMaybeTest()
        {
            var obj = new object();
            obj = null;
            Validate(obj, valueIsRequired: false);

            obj = new object();
            Validate(obj);
        }

        [TestMethod()]
        public void CanConvertNullables()
        {
            int? number = null;
            number = 1;

            Validate(number);

            number = null;
            Validate(number, valueIsRequired: false);
        }

        [TestMethod()]
        public void NoneIfEmptyTest()
        {
            var empty = new string[] { };
            var maybe = empty.ToMaybe();
            maybe.Case(_ => Assert.IsTrue(true, success), () => Assert.Fail(failure));

            // TODO: pattern match to check for Enumerable Maybes and Some/None
            // don't just check if the string[] itself it non-null:
            Validate(empty, valueIsRequired: true);
        }

        [TestMethod()]
        public void CanGetFirstValueOrNothing()
        {
            var list = new List<object> { "string", 'c', null, new object(), 6.75m };
            var maybe = list.FirstOrNone();
            list = null;
            Assert.IsTrue(maybe.HasValue);
        }

        [TestMethod]
        public void CanConvertImplicitly()
        {
            var obj = new object().ToString();
            Maybe<string> maybe = obj;
            Validate(maybe);
        }

        [TestMethod()]
        public void ReturnTest()
        {
            //TODO: You left this blank; figure out what you meant by Return and implement it.
        }
    }
}