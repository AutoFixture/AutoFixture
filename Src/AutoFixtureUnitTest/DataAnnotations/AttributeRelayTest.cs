using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class AttributeRelayTest
    {
        [Fact]
        public void ClassIsISpecimenBuilderToParticipateInFixtureCustomization()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(AttributeRelay<>)));
            // Teardown
        }

        [Fact]
        public void ClassIsAbstractBecauseCreationOfRelayedRequestIsDifferentForEachAttribute()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(AttributeRelay<>).IsAbstract);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNull()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<Attribute>();
            var context = new DelegatingSpecimenContext();
            // Exercise system
            object result = sut.Create(null, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotICustomAttributeProvider()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<Attribute>();
            var request = new object();
            var context = new DelegatingSpecimenContext();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var noSpecimen = Assert.IsType<NoSpecimen>(result);
            Assert.Same(request, noSpecimen.Request);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenICustomAttributeProviderDoesNotReturnExpectedAttributeType()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<RangeAttribute>();
            var providedAttribute = new ProvidedAttribute(new RequiredAttribute(), default(bool));
            var request = new FakeCustomAttributeProvider(providedAttribute);
            var context = new DelegatingSpecimenContext();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var noSpecimen = Assert.IsType<NoSpecimen>(result);
            Assert.Same(request, noSpecimen.Request);
            // Teardown
        }

        [Fact]
        public void CreateGetsInheritedAttributesFromProviderBecauseAttributesAreInheritedByDefault()
        {
            // Fixture setup
            bool inheritedAttributeRequested = false;
            var sut = new TestableAttributeRelay<RequiredAttribute>();
            sut.OnCreateRelayedRequest = (r, a) => inheritedAttributeRequested = true;
            var providedAttribute = new ProvidedAttribute(new RequiredAttribute(), inherited: true);
            var request = new FakeCustomAttributeProvider(providedAttribute);
            var context = new DelegatingSpecimenContext();
            // Exercise system
            sut.Create(request, context);
            // Verify outcome
            Assert.True(inheritedAttributeRequested);
            // Teardown
        }

        [Fact]
        public void CreateGetsDerivedAttributesFromProviderToAllowDescendantsHandleAttributeClassHierarchies()
        {
            // Fixture setup
            bool derivedAttributeObtained = false;
            var sut = new TestableAttributeRelay<ValidationAttribute>();
            sut.OnCreateRelayedRequest = (r, a) => derivedAttributeObtained = true;
            var providedAttribute = new ProvidedAttribute(new RequiredAttribute(), inherited: true);
            var request = new FakeCustomAttributeProvider(providedAttribute);
            var context = new DelegatingSpecimenContext();
            // Exercise system
            sut.Create(request, context);
            // Verify outcome
            Assert.True(derivedAttributeObtained);
            // Teardown
        }

        [Fact]
        public void CreatePassesICustomAttributeProviderAndAttributeItReturnsToCreateRelayedRequest()
        {
            // Fixture setup
            ICustomAttributeProvider requestReceivedByCreateRelayedRequest = null;
            RangeAttribute attributeReceivedByCreateRelayedRequest = null;
            var sut = new TestableAttributeRelay<RangeAttribute>();
            sut.OnCreateRelayedRequest = (r, a) =>
            {
                requestReceivedByCreateRelayedRequest = r;
                attributeReceivedByCreateRelayedRequest = a;
                return default(object);
            };
            var attribute = new RangeAttribute(0, 1);
            var request = new FakeCustomAttributeProvider(new ProvidedAttribute(attribute, default(bool)));
            var context = new DelegatingSpecimenContext();
            // Exercise system
            sut.Create(request, context);
            // Verify outcome
            Assert.Same(request, requestReceivedByCreateRelayedRequest);
            Assert.Same(attribute, attributeReceivedByCreateRelayedRequest);
            // Teardown
        }

        [Fact]
        public void CreateResolvesRelayedRequestFromContext()
        {
            // Fixture setup
            var relayedRequest = new object();
            var sut = new TestableAttributeRelay<RequiredAttribute> { OnCreateRelayedRequest = (r, a) => relayedRequest };
            var request = new FakeCustomAttributeProvider(new ProvidedAttribute(new RequiredAttribute(), default(bool)));
            object requestResolvedFromContext = null;
            var context = new DelegatingSpecimenContext { OnResolve = r => requestResolvedFromContext = r };
            // Exercise system
            sut.Create(request, context);
            // Verify outcome
            Assert.Same(relayedRequest, requestResolvedFromContext);
            // Teardown
        }

        [Fact]
        public void CreateReturnsInstanceResolvedFromContext()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<RequiredAttribute>();
            var request = new FakeCustomAttributeProvider(new ProvidedAttribute(new RequiredAttribute(), default(bool)));
            var returnedByContext = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => returnedByContext };
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            Assert.Same(returnedByContext, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsArgumentNullExceptionWhenContextIsNullToFailFast()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<Attribute>();
            var request = new object();
            // Exercise system
            var e = Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Verify outcome
            Assert.Equal("context", e.ParamName);
            // Teardown
        }

        [Fact]
        public void CreateThrowsInvalidOperationExceptionWhenProviderReturnsMultipleAttributesOfExpectedType()
        {
            // Fixture setup
            var sut = new TestableAttributeRelay<RequiredAttribute>();
            var providedAttribute = new ProvidedAttribute(new RequiredAttribute(), default(bool));
            var request = new FakeCustomAttributeProvider(providedAttribute, providedAttribute);
            var context = new DelegatingSpecimenContext();
            // Exercise system
            var e = Assert.Throws<InvalidOperationException>(() => sut.Create(request, context));
            // Verify outcome
            Assert.Contains("multiple", e.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(typeof(RequiredAttribute).FullName, e.Message);
            Assert.Contains(request.ToString(), e.Message);
            Assert.NotNull(e.InnerException);
            // Teardown
        }

        private class TestableAttributeRelay<T> : AttributeRelay<T> where T : Attribute
        {
            public Func<ICustomAttributeProvider, T, object> OnCreateRelayedRequest = (r, a) => default(object);

            protected override object CreateRelayedRequest(ICustomAttributeProvider request, T attribute)
            {
                return this.OnCreateRelayedRequest(request, attribute);
            }
        }
    }
}
