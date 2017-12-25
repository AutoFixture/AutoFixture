using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class EnumGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new EnumGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void RequestNonEnumReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new EnumGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(TriState), 1, TriState.First)]
        [InlineData(typeof(TriState), 2, TriState.Second)]
        [InlineData(typeof(TriState), 3, TriState.Third)]
        [InlineData(typeof(TriState), 4, TriState.First)]
        [InlineData(typeof(TriState), 5, TriState.Second)]
        [InlineData(typeof(DayOfWeek), 1, DayOfWeek.Sunday)]
        [InlineData(typeof(DayOfWeek), 2, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 8, DayOfWeek.Sunday)]
        public void RequestForEnumTypeReturnsCorrectResult(Type enumType, int requestCount, object expectedResult)
        {
            // Arrange
            var sut = new EnumGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount).Select(f => f()).ToArray().Last();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(TriState), TriState.Third)]
        [InlineData(typeof(DayOfWeek), DayOfWeek.Monday)]
        [InlineData(typeof(ConsoleColor), ConsoleColor.Black)]
        public void SutCanCorrectlyInterleaveDifferentEnumTypes(Type enumType, object expectedResult)
        {
            // Arrange
            var sut = new EnumGenerator();
            var dummyContext = new DelegatingSpecimenContext();

            sut.Create(typeof(TriState), dummyContext);
            sut.Create(typeof(TriState), dummyContext);
            sut.Create(typeof(DayOfWeek), dummyContext);
            // Act
            var result = sut.Create(enumType, dummyContext);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(ActivityScope), 1, ActivityScope.Undefined)]
        [InlineData(typeof(ActivityScope), 2, ActivityScope.OnDuty)]
        [InlineData(typeof(ActivityScope), 3, ActivityScope.OffDuty)]
        [InlineData(typeof(ActivityScope), 4, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 5, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 6, ActivityScope.AllInitiatingScopes)]
        [InlineData(typeof(ActivityScope), 7, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 8, ActivityScope.All)]
        [InlineData(typeof(ActivityScope), 9, ActivityScope.Undefined)]
        [InlineData(typeof(ActivityScope), 10, ActivityScope.OnDuty)]
        [InlineData(typeof(ActivityScope), 11, ActivityScope.OffDuty)]
        [InlineData(typeof(ActivityScope), 20, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 21, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 100, ActivityScope.Standalone)]
        public void RequestForFlagEnumTypeReturnsCorrectResult(Type enumType, int requestCount, object expectedResult)
        {
            // Arrange
            var sut = new EnumGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount).Select(f => f()).ToArray().Last();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void RequestForEnumWithNoValuesThrowsObjectCreationException()
        {
            // Arrange
            var sut = new EnumGenerator();
            // Act & assert
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() => sut.Create(typeof(EmptyEnum), dummyContext));
        }
    }
}
