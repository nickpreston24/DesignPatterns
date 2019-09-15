using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using static System.PrintHelpers;

namespace System.Tests
{
    [TestClass()]
    public class MaybeExtensionsTests
    {
        private const string successfullyNull = "Success! Nothing found.";
        private const string shouldBeNothing = "Failure! Found some value it should be nothing.";

        private const string success = "Success! Found some value.";
        private const string failure = "Failure! Nothing found!";

        private void Validate(object value, bool valueIsRequired = true)
        {
            var maybe = value.ToMaybe();

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
        public void CanConvertToMaybeTest()
        {
            var value = new object();
            value = null;
            Validate(value, valueIsRequired: false);

            value = new object();
            Validate(value);
        }

        [TestMethod]
        public void CanMapClasses()
        {
            var store = new Store();
            var maybe = store.ToMaybe();

            var zips = maybe.Map(shop => shop.ZipCode).Value;

            var transform = maybe
                .Map(shop => new StoreDto
                {
                    Name = $"renovated {shop.Name}"
                }
                .ToMaybe())
                .Value;

            Print(zips, store, transform);
        }

        //[TestMethod]
        //public void CanConvertNullableToMaybe()
        //{
        //    int? nullableInt = 10;
        //    var maybe = nullableInt.ToMaybe();

        //    //Assert
        //    Assert.IsNotNull(maybe);
        //    Validate(nullableInt, valueIsRequired: true);
        //}

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
        public void CanUserAReturn()
        {
            var store = new Store();

            //Return
            var maybe = store.Return();

            //Bind
            var nestedWage = maybe
                .Bind<Cashier>(s => s.Cashier)
                .Bind<Wage>(cashier => cashier.Wage).Value;

            var amount = nestedWage.Amount;

            Print(nestedWage, amount);
            Assert.IsNotNull(nestedWage);
        }

        [TestMethod]
        public void CanUseMultipleCases()
        {
            var store = new Store();
            var maybe = store.ToMaybe();

            maybe.Case(_ => { Console.WriteLine("Action 1A"); }, () => { Console.WriteLine("Action 1B"); })
                .Case(_ => { Console.WriteLine("Action 2A"); }, () => { Console.WriteLine("Action 2B"); })
                .IfSome(shop => shop.Name = "See's Candy")
                .Case(shop => { Console.WriteLine(shop.Name); }, () => { Console.WriteLine("Action 3B"); });
        }

        [TestMethod]
        public void CanThrowCustomException()
        {
            Store store = null;
            var maybe = store.ToMaybe();
            try
            {
                maybe.ValueOrThrow(new NullReferenceException("drat, no value!"));
            }
            catch (Exception ex)
            {
                ex.Message.Print();
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void CanGetDefaultValueIfNone()
        {
        }

        //Mocks
        private class Store
        {
            public int Id { get; set; } = 107;
            public int ZipCode { get; set; } = 56734;
            public string Name { get; set; } = "Natural Grocers";

            public Cashier Cashier = new Cashier();

            public override string ToString() => $"{Id} {Name} {ZipCode}";
        }

        private class StoreDto
        {
            public int ZipCode { get; set; } = 34562;
            public string Name { get; set; } = "Whole Foods";

            public Cashier Cashier = new Cashier();

            public override string ToString() => $"{Name} {ZipCode}";
        }

        private class Cashier
        {
            public Wage Wage { get; set; } = new Wage();
        }

        public class Wage
        {
            public double Amount { get; set; } = 12.00;
        }
    }
}