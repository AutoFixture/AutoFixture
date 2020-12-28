using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RestrictedEnumGeneratorTest
    {
        [Fact]
        public void InitializeWithNullRestrictedEnumValueSelectionThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new RestrictedEnumGenerator<TriState>(null));
        }

        [Theory]
        [InlineData(typeof(DayOfWeek))]
        [InlineData(typeof(EnumType))]
        public void RequestDifferentTypeOfEnumReturnsCorrectResult(object request)
        {
            // Arrange
            var values = RestrictedEnum.ExcludeValues(TriState.First);
            var sut = new RestrictedEnumGenerator<TriState>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(DayOfWeek), 1, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 2, DayOfWeek.Tuesday)]
        [InlineData(typeof(DayOfWeek), 3, DayOfWeek.Wednesday)]
        [InlineData(typeof(DayOfWeek), 4, DayOfWeek.Thursday)]
        [InlineData(typeof(DayOfWeek), 5, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 6, DayOfWeek.Tuesday)]
        public void RequestForEnumTypeExcludeValuesReturnsCorrectResult(
            Type enumType,
            int requestCount,
            object expectedResult)
        {
            // Arrange
            var values = RestrictedEnum.ExcludeValues(DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday);
            var sut = new RestrictedEnumGenerator<DayOfWeek>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount)
                .Select(f => f())
                .ToArray()
                .Last();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(DayOfWeek), 1, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 2, DayOfWeek.Tuesday)]
        [InlineData(typeof(DayOfWeek), 3, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 4, DayOfWeek.Tuesday)]
        [InlineData(typeof(DayOfWeek), 5, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 6, DayOfWeek.Tuesday)]
        public void RequestForEnumTypeIncludeValuesReturnsCorrectResult(
            Type enumType,
            int requestCount,
            object expectedResult)
        {
            // Arrange
            var values = RestrictedEnum.IncludeValues(DayOfWeek.Monday, DayOfWeek.Tuesday);
            var sut = new RestrictedEnumGenerator<DayOfWeek>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount)
                .Select(f => f())
                .ToArray()
                .Last();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(ActivityScope), 1, ActivityScope.OnDuty)]
        [InlineData(typeof(ActivityScope), 2, ActivityScope.OffDuty)]
        [InlineData(typeof(ActivityScope), 3, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 4, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 5, ActivityScope.AllInitiatingScopes)]
        [InlineData(typeof(ActivityScope), 6, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 7, ActivityScope.All)]
        [InlineData(typeof(ActivityScope), 8, ActivityScope.OnDuty)]
        [InlineData(typeof(ActivityScope), 9, ActivityScope.OffDuty)]
        [InlineData(typeof(ActivityScope), 10, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 11, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 20, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 21, ActivityScope.All)]
        [InlineData(typeof(ActivityScope), 100, ActivityScope.OffDuty)]
        public void RequestForFlagEnumTypeExcludeValuesReturnsCorrectResult(
            Type enumType,
            int requestCount,
            object expectedResult)
        {
            // Arrange
            var values = RestrictedEnum.ExcludeValues(ActivityScope.Undefined);
            var sut = new RestrictedEnumGenerator<ActivityScope>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount)
                .Select(f => f())
                .ToArray()
                .Last();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(ActivityScope), 1, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 2, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 3, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 4, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 5, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 6, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 7, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 8, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 9, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 10, ActivityScope.Standalone)]
        [InlineData(typeof(ActivityScope), 11, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 20, ActivityScope.Parent)]
        [InlineData(typeof(ActivityScope), 21, ActivityScope.Child)]
        [InlineData(typeof(ActivityScope), 100, ActivityScope.Standalone)]
        public void RequestForFlagEnumTypeIncludeValuesReturnsCorrectResult(
            Type enumType,
            int requestCount,
            object expectedResult)
        {
            // Arrange
            var values = RestrictedEnum.IncludeValues(
                ActivityScope.Standalone,
                ActivityScope.Parent,
                ActivityScope.Child);

            var sut = new RestrictedEnumGenerator<ActivityScope>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(enumType, dummyContext), requestCount)
                .Select(f => f())
                .ToArray()
                .Last();

            // Assert
            Assert.Equal(expectedResult, result);
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
            var values = RestrictedEnum.ExcludeValues(TriState.First);
            var sut = new RestrictedEnumGenerator<TriState>(values);

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var values = RestrictedEnum.ExcludeValues(TriState.First);
            var sut = new RestrictedEnumGenerator<TriState>(values);

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }
    }
}
