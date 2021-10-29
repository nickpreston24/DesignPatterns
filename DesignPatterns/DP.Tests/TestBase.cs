using System;
using Xunit.Abstractions;

namespace DP.Tests
{
    // Base for all unit tests
    public abstract class UnitTestBase
    {
    }

    // Enforce all XUnitTests to have Print
    public interface IUnitTest
    {
        void Print(string message);
    }

    /// <summary>
    /// XUnitUnitTest base class
    /// </summary>
    public class XUnitUnitTest : UnitTestBase
    {
        Action<string> print;

        // Alias for XUnit's output
        public void Print(string message) => print(message);
        protected XUnitUnitTest(ITestOutputHelper output) => print = output.WriteLine;
    }

    #region Old attempt at an adapter

    /**
     * The following won't work b/c Xunit does parallel output, but Debug and Console don't.
     */
    

    // public class XUnitTestAdapter : ITestOutputHelper
    // {
    //     public IUnitTest currentTest;
    //     ITestOutputHelper helper;
    //
    //     public XUnitTestAdapter(IUnitTest unitTest, ITestOutputHelper helper)
    //         : this(helper)
    //     {
    //         // Console.WriteLine("Inside 1st cotr");
    //         // Debug.WriteLine("Inside 1st cotr");
    //
    //         currentTest = unitTest;
    //
    //         this.helper = helper;
    //         this.helper?.WriteLine("Inside 1st cotr");
    //
    //         currentTest.SetPrintAction(Console.WriteLine);
    //     }
    //
    //     public XUnitTestAdapter(ITestOutputHelper helper)
    //     {
    //         this.helper = helper;
    //         helper.WriteLine("Inside 2nd cotr");
    //         // Console.WriteLine("Inside 2nd cotr");
    //         // Debug.WriteLine("Inside 2nd cotr");
    //     }
    //
    //     public void WriteLine(string message)
    //     {
    //         // currentTest.Print(message);
    //     }
    //
    //     public void WriteLine(string format, params object[] args) =>
    //         throw new NotImplementedException("Not implementing args[] yet!");
    // }

    // public class XUnitUnitTest : UnitTestBase
    // {
    //     XUnitTestAdapter adapter;
    //
    //     protected XUnitUnitTest(ITestOutputHelper dummy)
    //     {
    //         adapter = new XUnitTestAdapter(new UnitTest(), dummy);
    //         adapter.currentTest.Print("From adaptee!");
    //         // adaptee.SetPrintAction(Console.WriteLine);
    //     }
    // }
    //
    // public class UnitTest : UnitTestBase
    // {
    // }
    #endregion Old attempt at an adapter
}
