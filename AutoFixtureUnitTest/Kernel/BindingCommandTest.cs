using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using System.Linq.Expressions;

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
            Func<ISpecimenContainer, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(null, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            Func<ISpecimenContainer, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            Func<ISpecimenContainer, string> dummyValueCreator = c => "Anonymous value";
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            Func<ISpecimenContainer, object> dummyValueCreator = c => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression, dummyValueCreator));
            // Teardown
        }

        [Fact]
        public void InitializeDelegateValueConstructorWithNullCreateWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (Func<ISpecimenContainer, object>)null));
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
        public void InitializeInstanceValueConstructorWithNullCreateWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingCommand<PropertyHolder<object>, object>(ph => ph.Property, (object)null));
            // Teardown
        }

        [Fact]
        public void ExecuteWithNullItemWillThrow()
        {
            // Fixture setup
            var sut = new BindingCommand<PropertyHolder<string>, string>(ph => ph.Property);
            // Exercise system and verify outcome
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

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
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedRequest.Equals(r) ? expectedValue : new NoSpecimen() };

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
            var container = new DelegatingSpecimenContainer { OnCreate = r => nonInt };

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
            var expectedContainer = new DelegatingSpecimenContainer();

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
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Execute(specimen, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedValue, specimen.Property);
            // Teardown
        }
    }
}
