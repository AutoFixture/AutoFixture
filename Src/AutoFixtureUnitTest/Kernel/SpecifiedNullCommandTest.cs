using System;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    public class SpecifiedNullCommandTest
    {
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<PropertyHolder<object>>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullExpressionThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecifiedNullCommand<PropertyHolder<object>, object>(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<object, object>(invalidExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithMethodExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<object, string>(methodExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new SpecifiedNullCommand<SingleParameterType<object>, object>(readOnlyPropertyExpression));
            // Teardown
        }

        [Fact]
        public void MemberIsCorrect()
        {
            // Fixture setup
            var expectedMember = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system
            MemberInfo result = sut.Member;
            // Verify outcome
            Assert.Equal(expectedMember, result);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotThrow()
        {
            // Fixture setup
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(null, null)));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForAnonymousRequest()
        {
            // Fixture setup
            var request = new object();
            var sut = new SpecifiedNullCommand<PropertyHolder<object>, object>(ph => ph.Property);
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
            var sut = new SpecifiedNullCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property2);
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
            var sut = new SpecifiedNullCommand<DoublePropertyHolder<object, object>, object>(ph => ph.Property1);
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
            var sut = new SpecifiedNullCommand<AbstractType, object>(x => x.Property1);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
#pragma warning restore 618
}
