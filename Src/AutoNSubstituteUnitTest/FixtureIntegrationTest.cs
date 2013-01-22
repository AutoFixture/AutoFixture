using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoSubstitutesInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<IInterface>();
            // Verify outcome
            Assert.IsAssignableFrom<IInterface>(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoSubstitutesAbstractType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractType>(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property1);
            Assert.NotEqual(0, result.Property2);
            // Teardown
        }

        [Fact]
        public void FixtureCanFreezeSubstitute()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var dummy = new object();
            var substitute = fixture.Freeze<IInterface>();
            substitute.MakeIt(dummy).Returns(null);
            // Exercise system
            var result = fixture.Create<IInterface>();
            result.MakeIt(dummy);
            // Verify outcome
            result.Received().MakeIt(dummy);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<IList<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<AbstractGenericType<object>>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<Guid>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(ICollection<object>))]
        [InlineData(typeof(IList<object>))]
        public void FixtureDoesNotHijackCollectionInterfacesIfAnotherCustomizationExistsForThem(Type collectionInterface)
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization()).Customize(new AutoNSubstituteCustomization());
            var context = new SpecimenContext(fixture);
            // Exercise system
            var result = context.Resolve(new SeededRequest(collectionInterface, null));
            // Verify outcome
            Assert.NotEmpty((IEnumerable)result);
        }
    }
}
