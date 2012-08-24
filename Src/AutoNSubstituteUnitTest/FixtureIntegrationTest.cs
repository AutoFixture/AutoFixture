using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Exceptions;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class FixtureIntegrationTest
    {
//        [Fact]
//        public void FixtureDoesNotMockConcreteType()
//        {
//            // Fixture setup
//            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
//            // Exercise system
//            var result = fixture.CreateAnonymous<string>();
//            // Verify outcome
//            Assert.Throws<InvalidOperationException>( () => result.GetMockRepository());
//            // Teardown
//        }

        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksInterfaceCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.IsAssignableFrom<IInterface>(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Guid>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructorRequiringGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<Guid>>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<NSubstituteTestTypes.AnotherAbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.NotNull(result);
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureDoesNotMockNestedConcreteTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<NSubstituteTestTypes.ConcreteGenericType<int>>>();
            // Verify outcome
            Assert.NotNull(result);
            Assert.Throws<NotASubstituteException>(() => result.Property.Received(1));
            // Teardown
        }

        [Fact]
        public void FixtureDoesNotMockParentOfNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<NSubstituteTestTypes.ConcreteGenericType<AbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.Throws<NotASubstituteException>(() => result.Received(1));
            Assert.NotNull(result.Value);
            // Teardown
        }

        [Fact]
        public void FixtureMocksDoubleGenericTypeCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<NSubstituteTestTypes.ConcreteDoublyGenericType<ConcreteType, AbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.Throws<NotASubstituteException>(() => result.Received(1));
            Assert.Throws<NotASubstituteException>(() => result.Value1.Received(1));
            Assert.NotNull(result.Value2);
            // Teardown
        }

        [Fact]
        public void CreateWithAbstractTypeReturnsMockedResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractGenericType<object>>();
            // Verify outcome
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorReturnsCorrectType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractGenericType<object>>();
            // Verify outcome
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAbstractGenericTypeWithConcreteGenericParameterIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractGenericType<object>>();
            // Verify outcome
            Assert.Throws<NotASubstituteException>(() => result.Value.Received(1));
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<List<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateStack()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Stack<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateHashSet()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<HashSet<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
