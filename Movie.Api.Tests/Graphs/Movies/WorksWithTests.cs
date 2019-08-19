using Moq;
using Movies.Api.Graphs;
using NUnit.Framework;

namespace MoviesApi.Tests
{
    [TestFixture]
    public class WorksWithTests
    {
        private MockRepository mockRepository;

        private Mock<Actor> mockActor;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockActor = mockRepository.Create<Actor>();
        }

        [TearDown]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }

        private WorksWith CreateWorksWith()
        {
            return new WorksWith(mockActor.Object);
        }

        [Test]
        public void Condition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var worksWith = CreateWorksWith();

            // Act
            var result = worksWith.Condition();

            // Assert
            Assert.Fail();
        }
    }
}