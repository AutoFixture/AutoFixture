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
        public void InitializeWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, object>(invalidExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<object, string>(methodExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression));
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
    }
}
