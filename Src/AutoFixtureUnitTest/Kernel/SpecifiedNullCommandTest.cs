using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class SpecifiedNullCommandTest
    {
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Arrange
            // Act
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Assert
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<PropertyHolder<object>>>(sut);
        }

        [Fact]
        public void InitializeWithNullExpressionThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecifiedNullCommand<PropertyHolder<object>, object>(null));
        }

        [Fact]
        public void InitializeWithNonMemberExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Act & assert
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<object, object>(invalidExpression));
        }

        [Fact]
        public void InitializeWithMethodExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<object, string>(methodExpression));
        }

        [Fact]
        public void InitializeWithReadOnlyPropertyExpressionWillThrow()
        {
            // Arrange
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Act & assert
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression));
        }

        [Fact]
        public void MemberIsCorrect()
        {
            // Arrange
            var expectedMember = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act
            MemberInfo result = sut.Member;
            // Assert
            Assert.Equal(expectedMember, result);
        }

        [Fact]
        public void ExecuteDoesNotThrow()
        {
            // Arrange
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Execute(null, null)));
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForAnonymousRequest()
        {
            // Arrange
            var request = new object();
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act
            bool result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForOtherProperty()
        {
            // Arrange
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new SpecifiedNullCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property2);
            // Act
            bool result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForIdentifiedProperty()
        {
            // Arrange
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new SpecifiedNullCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property1);
            // Act
            bool result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForDerivedProperty()
        {
            // Arrange
            var request = typeof(ConcreteType).GetProperty("Property1");
            var sut = new SpecifiedNullCommand<AbstractType, object>(x => x.Property1);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
