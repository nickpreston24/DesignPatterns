using ChainOfResponsibility.Conceptual;
using Xunit;
using Xunit.Abstractions;

namespace DP.Tests.ChainOfResponsibility
{
    public class CoRUnitTests : XUnitUnitTest
    {
        [Fact]
        void CanChooseProperHandler()
        {
            var monkey = new MonkeyHandler();
            var squirrel = new SquirrelHandler();
            var dog = new DogHandler();

            monkey.SetNext(squirrel).SetNext(dog);

            // The client should be able to send a request to any handler, not
            // just the first one in the chain.
            Print("Chain: Monkey > Squirrel > Dog\n");
            Client.ClientCode(monkey);
            Print("Subchain: Squirrel > Dog\n");
            Client.ClientCode(squirrel);
        }

        public CoRUnitTests(ITestOutputHelper output) : base(output)
        {
        }
    }
}