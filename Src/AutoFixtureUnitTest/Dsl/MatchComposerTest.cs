using System;
using System.Linq;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class MatchComposerTest
    {
        [Fact]
        public void SutIsMatchComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new MatchComposer<object>(null);
            // Verify outcome
            Assert.IsAssignableFrom<IMatchComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new MatchComposer<object>(expected);
            // Exercise system
            ISpecimenBuilder actual = sut.Builder;
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithoutMatchersAlwaysDelegatesToTheBuilder()
        {
            // Fixture setup
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (request, context) => expected
            };
            var sut = new MatchComposer<object>(builder);
            // Exercise system
            var actual = sut.Create(typeof(object), new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        public void CreateWithMatchingByBaseTypeReturnsSpecimenForRequestsOfSameOfBaseType(Type request)
        {
            // Fixture setup
            var specimen = new ConcreteType();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<ConcreteType>(builder).BaseType();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByBaseTypeReturnsNoSpecimenForRequestsOfIncompatibleTypes()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new ConcreteType()
            };
            var sut = new MatchComposer<ConcreteType>(builder).BaseType();
            // Exercise system
            var otherTypeRequest = typeof(string);
            var actual = sut.Create(otherTypeRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(otherTypeRequest), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByExactTypeReturnsSpecimenForRequestsOfSameType()
        {
            // Fixture setup
            var specimen = new ConcreteType();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<ConcreteType>(builder).ExactType();
            // Exercise system
            var exactTypeRequest = typeof(ConcreteType);
            var actual = sut.Create(exactTypeRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByExactTypeReturnsNoSpecimenForRequestsOfOtherTypes()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new ConcreteType()
            };
            var sut = new MatchComposer<ConcreteType>(builder).ExactType();
            // Exercise system
            var otherTypeRequest = typeof(string);
            var actual = sut.Create(otherTypeRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(otherTypeRequest), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByParameterNameReturnsSpecimenForRequestsOfParameterTypeWithMatchingName()
        {
            // Fixture setup
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).ParameterName("obj");
            // Exercise system
            var matchingParameterRequest = typeof(ConcreteType)
                .GetConstructor(new[] { typeof(object) })
                .GetParameters()
                .First();
            var actual = sut.Create(matchingParameterRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByParameterNameReturnsNoSpecimenForRequestsOfParameterTypeWithOtherName()
        {
            // Fixture setup
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).ParameterName("someOtherName");
            // Exercise system
            var otherParameterRequest = typeof(ConcreteType)
                .GetConstructor(new[] { typeof(object) })
                .GetParameters()
                .First();
            var actual = sut.Create(otherParameterRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(otherParameterRequest), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByPropertyNameReturnsSpecimenForRequestsOfPropertyTypeWithMatchingName()
        {
            // Fixture setup
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).PropertyName("Property");
            // Exercise system
            var matchingPropertyRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var actual = sut.Create(matchingPropertyRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByPropertyNameReturnsNoSpecimenForRequestsOfPropertyTypeWithOtherName()
        {
            // Fixture setup
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).PropertyName("someOtherName");
            // Exercise system
            var otherPropertyRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var actual = sut.Create(otherPropertyRequest, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(otherPropertyRequest), actual);
            // Teardown
        }
    }
}
