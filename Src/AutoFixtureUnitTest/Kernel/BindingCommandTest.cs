using System;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class BindingCommandTest
    {
        [Fact]
        public void InitializeAnonymousValueConstructorWithNullExpressionWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null));
            // Teardown
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression));
            // Teardown
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression));
            // Teardown
        }

        [Fact]
        public void InitializeAnonymousValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNullExpressionWillThrow()
        {
            // Fixture setup
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            Func<ISpecimenContext, string> dummyValueCreator = c => "Anonymous value";
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            Func<ISpecimenContext, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNullCreateWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (Func<ISpecimenContext, object>)null));
            // Teardown
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNullExpressionWillThrow()
        {
            // Fixture setup
            var dummyValue = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null, dummyValue));
            // Teardown
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            var dummyValue = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression, dummyValue));
            // Teardown
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            var dummyValue = "Anonymous value";
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression, dummyValue));
            // Teardown
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            var dummyValue = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression, dummyValue));
            // Teardown
        }

        [Fact]
        public void InitializeInstanceValueConstructorWithNullValueDoesNotThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => 
                new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (object)null)));
            // Teardown
        }

        [Fact]
        public void MemberIsCorrect()
        {
            // Fixture setup
            var expectedMember = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system
            MemberInfo result = sut.Member;
            // Verify outcome
            Assert.Equal(expectedMember, result);
            // Teardown
        }

        [Fact]
        public void ValueCreatorIsCorrect()
        {
            // Fixture setup
            Func<ISpecimenContext, double> expectedCreator = c => 8;
            var sut = new BindingCommand<PropertyHolder<double>, double>(ph => ph.Property, expectedCreator);
            // Exercise system
            Func<ISpecimenContext, double> result = sut.ValueCreator;
            // Verify outcome
            Assert.Equal(expectedCreator, result);
            // Teardown
        }

        [Fact]
        public void ExecuteWithNullItemWillThrow()
        {
            // Fixture setup
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            // Exercise system and verify outcome
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, dummyContainer));
            // Teardown
        }

        [Fact]
        public void ExecuteWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            // Exercise system and verify outcome
            var dummySpecimen = new PropertyHolder<string>();
            Assert.Throws<ArgumentNullException>(() => sut.Execute(dummySpecimen, null));
            // Teardown
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Fixture setup
            var expectedValue = new object();
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteWillSetCorrectFieldOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Fixture setup
            var expectedValue = new object();
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

            var sut = new BindingCommand<FieldHolder<object>, object>(ph => ph.Field);
            var specimen = new FieldHolder<object>();
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Field);
            // Teardown
        }

        [Fact]
        public void ExecuteWillThrowWhenContainerReturnsIncompatiblePropertyValue()
        {
            // Fixture setup
            var nonInt = "Anonymous variable";
            var container = new DelegatingSpecimenContext { OnResolve = r => nonInt };

            var sut = new BindingCommand<PropertyHolder<int>, int>(ph => ph.Property);
            // Exercise system and verify outcome
            var dummySpecimen = new PropertyHolder<int>();
            Assert.Throws<InvalidOperationException>(() => sut.Execute(dummySpecimen, container));
            // Teardown
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenCreatorIsSupplied()
        {
            // Fixture setup
            var expectedValue = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, c => expectedContainer == c ? expectedValue : new NoSpecimen());
            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, expectedContainer);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteWillSetCorrectPropertyOnSpecimenWhenValueIsSupplied()
        {
            // Fixture setup
            var expectedValue = new object();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, expectedValue);
            var specimen = new PropertyHolder<object>();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }

#pragma warning disable 618
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new BindingCommand<FieldHolder<DateTimeOffset>, DateTimeOffset>(fh => fh.Field);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<FieldHolder<DateTimeOffset>>>(sut);
            // Teardown
        }
#pragma warning restore 618

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForAnonymousRequest()
        {
            // Fixture setup
            var request = new object();
            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system
            bool result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForOtherProperty()
        {
            // Fixture setup
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new BindingCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property2);
            // Exercise system
            bool result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForIdentifiedProperty()
        {
            // Fixture setup
            var request = typeof(DoublePropertyHolder<object, object>).GetProperty("Property1");
            var sut = new BindingCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property1);
            // Exercise system
            bool result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForDerivedProperty()
        {
            // Fixture setup
            var request = typeof(ConcreteType).GetProperty("Property1");
            var sut = new BindingCommand<AbstractType, object>(x => x.Property1);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
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
            // Fixture setup
            var expectedValue = new object();
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() 
            };

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property);
            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute((object)specimen, context);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteSetsCorrectFieldOnSpecimenWhenAnonymousValueIsImplied()
        {
            // Fixture setup
            var expectedValue = new object();
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() 
            };

            var sut = new BindingCommand<FieldHolder<object>, object>(ph => ph.Field);
            var specimen = new FieldHolder<object>();
            // Exercise system
            sut.Execute((object)specimen, context);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Field);
            // Teardown
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
            // Fixture setup
            var expectedValue = new object();
            var expectedContext = new DelegatingSpecimenContext();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, c => expectedContext == c ? expectedValue : new NoSpecimen());
            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute((object)specimen, expectedContext);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteSetsCorrectPropertyOnSpecimenWhenValueIsSupplied()
        {
            // Fixture setup
            var expectedValue = new object();

            var sut = new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, expectedValue);
            var specimen = new PropertyHolder<object>();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            sut.Execute((object)specimen, dummyContext);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }
    }
}
