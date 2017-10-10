using System.Threading.Tasks;
using Moq;
using Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class StubPropertiesCommandTest
    {
        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void ExecuteDoesNotThrowsWhenSpecimenIsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(validNonMockSpecimen, context)));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Fixture setup
            var request = new object();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(request, context)));
        }

        [Fact]
        public void StubsProperties()
        {
            // Fixture setup
            const string expectedPropertyValue = "a string";
            var request = new Mock<IInterfaceWithProperty>();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Exercise system
            sut.Execute(request, context);
            // Verify outcome
            request.Object.Property = expectedPropertyValue;
            Assert.Equal(expectedPropertyValue, request.Object.Property);
        }

        [Fact]
        public void DoesNotHangIfMockedTypeHasPropertiesWithCircularDependencies()
        {
            // Fixture setup
            var request = new Mock<IInterfaceWithPropertyWithCircularDependency>()
            {
                DefaultValue = DefaultValue.Mock
            };
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Exercise system
            var task = Task.Factory.StartNew(() => sut.Execute(request, context));
            bool ranToCompletion = task.Wait(5000) && task.Status == TaskStatus.RanToCompletion;
            // Verify outcome
            Assert.True(ranToCompletion);
        }

        [Theory]
        [InlineData(DefaultValue.Empty)]
        [InlineData(DefaultValue.Mock)]
        public void DoesNotAffectMockDefaultValueSetting(DefaultValue defaultValue)
        {
            // Fixture setup
            var request = new Mock<IInterfaceWithProperty>()
            {
                DefaultValue = defaultValue
            };
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Exercise system
            sut.Execute(request, context);
            // Verify outcome
            Assert.Equal(defaultValue, request.DefaultValue);
        }
    }
}
