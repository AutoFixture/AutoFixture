using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

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

        [Fact]
        public void CreateWithMatchingByBaseTypeReturnsSpecimenForRequestsOfBaseType()
        {
            // Fixture setup
            var request = typeof(AbstractType);
            var specimen = new ConcreteType();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<ConcreteType>(builder).ByBaseType();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByBaseTypeReturnsSpecimenForRequestsOfSameType()
        {
            // Fixture setup
            var request = typeof(ConcreteType);
            var specimen = new ConcreteType();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<ConcreteType>(builder).ByBaseType();
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
            var request = typeof(string);
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new ConcreteType()
            };
            var sut = new MatchComposer<ConcreteType>(builder).ByBaseType();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByImplementedInterfacesReturnsSpecimenForRequestsOfImplementedInterface()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var specimen = new NoopInterfaceImplementer();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<NoopInterfaceImplementer>(builder).ByInterfaces();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByImplementedInterfacesReturnsNoSpecimenForRequestsOfNotImplementedInterface()
        {
            // Fixture setup
            var request = typeof(IDisposable);
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new NoopInterfaceImplementer()
            };
            var sut = new MatchComposer<NoopInterfaceImplementer>(builder).ByInterfaces();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByImplementedInterfacesReturnsNoSpecimenForRequestsOfNonInterfaces()
        {
            // Fixture setup
            var request = typeof(string);
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<NoopInterfaceImplementer>(builder).ByInterfaces();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByExactTypeReturnsSpecimenForRequestsOfSameType()
        {
            // Fixture setup
            var request = typeof(ConcreteType);
            var specimen = new ConcreteType();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => specimen
            };
            var sut = new MatchComposer<ConcreteType>(builder).ByExactType();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByExactTypeReturnsNoSpecimenForRequestsOfOtherTypes()
        {
            // Fixture setup
            var request = typeof(string);
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new ConcreteType()
            };
            var sut = new MatchComposer<ConcreteType>(builder).ByExactType();
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByParameterNameReturnsSpecimenForRequestsOfParameterTypeWithMatchingName()
        {
            // Fixture setup
            var request = Parameter<object>("parameter");
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).ByParameterName("parameter");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByParameterNameReturnsNoSpecimenForRequestsOfParameterWithIncompatibleType()
        {
            // Fixture setup
            var request = Parameter<int>("parameter");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByParameterName("parameter");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByArgumentNameReturnsNoSpecimenForRequestsOfParameterWithOtherName()
        {
            // Fixture setup
            var request = Parameter<object>("parameter");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByParameterName("someOtherName");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByPropertyReturnsSpecimenForRequestsOfPropertyWithCompatibleTypeAndMatchingName()
        {
            // Fixture setup
            var request = Property<object>("Property");
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).ByPropertyName("Property");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByPropertyReturnsNoSpecimenForRequestsOfPropertyWithIncompatibleType()
        {
            // Fixture setup
            var request = Property<string>("Property");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByPropertyName("Property");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByPropertyReturnsNoSpecimenForRequestsOfPropertyWithOtherName()
        {
            // Fixture setup
            var request = Property<object>("Property");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByPropertyName("someOtherName");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByFieldNameReturnsSpecimenForRequestsOfFieldWithCompatibleTypeAndMatchingName()
        {
            // Fixture setup
            var request = Field<object>("Field");
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder).ByFieldName("Field");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByFieldNameReturnsNoSpecimenForRequestsOfFieldWithIncompatibleType()
        {
            // Fixture setup
            var request = Field<int>("Field");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByFieldName("Field");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByFieldNameReturnsNoSpecimenForRequestsOfFieldWithOtherName()
        {
            // Fixture setup
            var request = Field<object>("Field");
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new object()
            };
            var sut = new MatchComposer<object>(builder).ByFieldName("someOtherName");
            // Exercise system
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), actual);
            // Teardown
        }

        [Fact]
        public void CreateWithMatchingByAnyOfMultipleMemberNamesReturnsSpecimenForMatchingRequests()
        {
            // Fixture setup
            var expected = new object();
            var context = new DelegatingSpecimenContext();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expected
            };
            var sut = new MatchComposer<object>(builder)
                .ByParameterName("parameter")
                .Or.ByPropertyName("Property")
                .Or.ByFieldName("Field");
            // Exercise system
            // Verify outcome
            Assert.Same(expected, sut.Create(Parameter<object>("parameter"), context));
            Assert.Same(expected, sut.Create(Property<object>("Property"), context));
            Assert.Same(expected, sut.Create(Field<object>("Field"), context));
            // Teardown
        }

        private static ParameterInfo Parameter<T>(string parameterName)
        {
            return typeof(SingleParameterType<T>)
                .GetConstructor(new[] { typeof(T) })
                .GetParameters()
                .Single(p => p.Name == parameterName);
        }

        private static PropertyInfo Property<T>(string propertyName)
        {
            return typeof(PropertyHolder<T>)
                   .GetProperty(propertyName);
        }

        private static FieldInfo Field<T>(string fieldName)
        {
            return typeof(FieldHolder<T>)
                   .GetField(fieldName);
        }
    }
}
