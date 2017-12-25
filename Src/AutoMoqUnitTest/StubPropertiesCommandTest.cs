using System.Threading.Tasks;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class StubPropertiesCommandTest
    {
        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void ExecuteDoesNotThrowsWhenSpecimenIsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Arrange
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(validNonMockSpecimen, context)));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Arrange
            var request = new object();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(request, context)));
        }

        [Fact]
        public void StubsProperties()
        {
            // Arrange
            const string expectedPropertyValue = "a string";
            var request = new Mock<IInterfaceWithProperty>();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Act
            sut.Execute(request, context);
            // Assert
            request.Object.Property = expectedPropertyValue;
            Assert.Equal(expectedPropertyValue, request.Object.Property);
        }

        [Fact]
        public void DoesNotHangIfMockedTypeHasPropertiesWithCircularDependencies()
        {
            // Arrange
            var request = new Mock<IInterfaceWithPropertyWithCircularDependency>()
            {
                DefaultValue = DefaultValue.Mock
            };
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Act
            var task = Task.Factory.StartNew(() => sut.Execute(request, context));
            bool ranToCompletion = task.Wait(5000) && task.Status == TaskStatus.RanToCompletion;
            // Assert
            Assert.True(ranToCompletion);
        }

        [Theory]
        [InlineData(DefaultValue.Empty)]
        [InlineData(DefaultValue.Mock)]
        public void DoesNotAffectMockDefaultValueSetting(DefaultValue defaultValue)
        {
            // Arrange
            var request = new Mock<IInterfaceWithProperty>()
            {
                DefaultValue = defaultValue
            };
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new StubPropertiesCommand();
            // Act
            sut.Execute(request, context);
            // Assert
            Assert.Equal(defaultValue, request.DefaultValue);
        }
    }
}
