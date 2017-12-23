using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class BindingCommandTest
    {
        [Fact]
        public void InitializeAnonymousValueConstructorWithNullExpressionWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null));
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression));
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithMethodExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression));
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Arrange
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression));
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNullExpressionWillThrow()
        {
            // Arrange
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null, dummyValueCreator));
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, object>> invalidExpression = obj => obj;
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression, dummyValueCreator));
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithMethodExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            Func<ISpecimenContext, string> dummyValueCreator = c => "Anonymous value";
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression, dummyValueCreator));
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Arrange
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression, dummyValueCreator));
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNullCreateWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (Func<ISpecimenContext, object>)null));
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNullExpressionWillThrow()
        {
            // Arrange
            var dummyValue = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null, dummyValue));
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, object>> invalidExpression = obj => obj;
            var dummyValue = new object();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression, dummyValue));
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithMethodExpressionWillThrow()
        {
            // Arrange
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            var dummyValue = "Anonymous value";
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression, dummyValue));
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Arrange
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            var dummyValue = new object();
            // Act & assert
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression, dummyValue));
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNullValueDoesNotThrow()
        {
            // Arrange
            // Act & assert
            Assert.Null(Record.Exception(() =>
                new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (object)null)));
        }

        [Fact]
        public void MemberIsCorrect()
        {
            // Arrange
            var expectedMember = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act
            MemberInfo result = sut.Member;
            // Assert
            Assert.Equal(expectedMember, result);
        }

        [Fact]
        public void ValueCreatorIsCorrect()
        {
            // Arrange
            Func<ISpecimenContext, double> expectedCreator = c => 8;
            var sut = new BindingCommand<PropertyHolder<double>, double>(ph => ph.Property, expectedCreator);
            // Act
            Func<ISpecimenContext, double> result = sut.ValueCreator;
            // Assert
            Assert.Equal(expectedCreator, result);
        }

        [Fact]
        public void ExecuteWithNullItemWillThrow()
        {
            // Arrange
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            // Act & assert
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() => sut.Execute((object)null, dummyContainer));
        }

        [Fact]
        public void ExecuteWithNullContainerWillThrow()
        {
            // Arrange
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            // Act & assert
            var dummySpecimen = new PropertyHolder<string>();
#pragma warning disable 618
            Assert.Throws<ArgumentNullException>(() => sut.Execute(dummySpecimen, null));
#pragma warning restore 618
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            var specimen = new PropertyHolder<object>();
            // Act
#pragma warning disable 618
            sut.Execute(specimen, container);
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }

        [Fact]
        public void ExecuteWillSetCorrectFieldOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

            var sut = new BindingCommand<FieldHolder<object>, object>(ph => ph.Field);
            var specimen = new FieldHolder<object>();
            // Act
#pragma warning disable 618
            sut.Execute(specimen, container);
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedValue, specimen.Field);
        }

        [Fact]
        public void ExecuteWillThrowWhenContainerReturnsIncompatiblePropertyValue()
        {
            // Arrange
            var nonInt = "Anonymous variable";
            var container = new DelegatingSpecimenContext { OnResolve = r => nonInt };

            var sut = new BindingCommand<PropertyHolder<int>, int>(ph => ph.Property);
            // Act & assert
            var dummySpecimen = new PropertyHolder<int>();
#pragma warning disable 618
            Assert.Throws<InvalidOperationException>(testCode: () => sut.Execute(dummySpecimen, container));
#pragma warning restore 618
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenCreatorIsSupplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, c => expectedContainer == c ? expectedValue : new NoSpecimen());
            var specimen = new PropertyHolder<object>();
            // Act
#pragma warning disable 618
            sut.Execute(specimen, expectedContainer);
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenValueIsSupplied()
        {
            // Arrange
            var expectedValue = new object();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, expectedValue);
            var specimen = new PropertyHolder<object>();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
#pragma warning disable 618
            sut.Execute(specimen, dummyContainer);
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }

        [Fact]
        [Obsolete]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Arrange
            // Act
            var sut = new BindingCommand<FieldHolder<DateTimeOffset>, DateTimeOffset>(fh => fh.Field);
            // Assert
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<FieldHolder<DateTimeOffset>>>(sut);
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act & assert
#pragma warning disable 618
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
#pragma warning restore 618
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForAnonymousRequest()
        {
            // Arrange
            var request = new object();
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Act
#pragma warning disable 618
            bool result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForOtherProperty()
        {
            // Arrange
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new BindingCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property2);
            // Act
#pragma warning disable 618
            bool result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForIdentifiedProperty()
        {
            // Arrange
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new BindingCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property1);
            // Act
#pragma warning disable 618
            bool result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForDerivedProperty()
        {
            // Arrange
            var request = typeof(ConcreteType).GetProperty("Property1");
            var sut = new BindingCommand<AbstractType, object>(x => x.Property1);
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutIsSpecimenCommand()
        {
            var request = typeof(ConcreteType).GetProperty("Property1");
            var sut = new BindingCommand<AbstractType, object>(x => x.Property1);
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void ExecuteWithNullSpecimenThrows()
        {
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute((object)null, dummyContext));
        }

        [Fact]
        public void ExecuteWithNullContextThrows()
        {
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            object dummySpecimen = new PropertyHolder<string>();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Execute(dummySpecimen, null));
        }

        [Fact]
        public void ExecuteSetsCorrectPropertyOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen()
            };

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute((object)specimen, context);
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }

        [Fact]
        public void ExecuteSetsCorrectFieldOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen()
            };

            var sut = new BindingCommand<FieldHolder<object>, object>(ph => ph.Field);
            var specimen = new FieldHolder<object>();
            // Act
            sut.Execute((object)specimen, context);
            // Assert
            Assert.Equal(expectedValue, specimen.Field);
        }

        [Fact]
        public void ExecuteThrowsWhenContainerReturnsIncompatiblePropertyValue()
        {
            var nonInt = "Anonymous variable";
            var context = new DelegatingSpecimenContext { OnResolve = r => nonInt };
            var sut = new BindingCommand<PropertyHolder<int>, int>(ph => ph.Property);

            object dummySpecimen = new PropertyHolder<int>();
            Assert.Throws<InvalidOperationException>(() =>
                sut.Execute(dummySpecimen, context));
        }

        [Fact]
        public void ExecuteSetsCorrectPropertyOnSpecimenWhenCreatorIsSupplied()
        {
            // Arrange
            var expectedValue = new object();
            var expectedContext = new DelegatingSpecimenContext();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, c => expectedContext == c ? expectedValue : new NoSpecimen());
            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute((object)specimen, expectedContext);
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }

        [Fact]
        public void ExecuteSetsCorrectPropertyOnSpecimenWhenValueIsSupplied()
        {
            // Arrange
            var expectedValue = new object();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, expectedValue);
            var specimen = new PropertyHolder<object>();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            sut.Execute((object)specimen, dummyContext);
            // Assert
            Assert.Equal(expectedValue, specimen.Property);
        }
    }
}
