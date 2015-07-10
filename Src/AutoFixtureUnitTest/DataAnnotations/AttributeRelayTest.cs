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
        public void ConstructorThrowsArgumentNullExceptionWhenCreateRelayedRequestIsNullToFailFast()
        {
            // Fixture setup
            Func<ICustomAttributeProvider, Attribute, object> createRelayedRequest = null;
            // Exercise system
            // Verify outcome
            var e = Assert.Throws<ArgumentNullException>(() => new AttributeRelay<Attribute>(createRelayedRequest));
            Assert.Equal("createRelayedRequest", e.ParamName);
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestMethodReturnsMethodInfoOfDelegateSpecifiedInConstructorToAllowInspectionTests()
        {
            // Fixture setup
            Func<ICustomAttributeProvider, Attribute, object> createRelayedRequest = (r, a) => new object();
            var sut = new AttributeRelay<Attribute>(createRelayedRequest);
            // Exercise system
            MethodInfo method = sut.CreateRelayedRequestMethod;
            // Verify outcome
            Assert.Same(createRelayedRequest.Method, method);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNull()
        {
            // Fixture setup
            var sut = new AttributeRelay<Attribute>((r, a) => new object());
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
            var sut = new AttributeRelay<Attribute>((r, a) => new object());
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
            var sut = new AttributeRelay<RangeAttribute>((r, a) => new object());
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
            var sut = new AttributeRelay<RequiredAttribute>((r, a) => inheritedAttributeRequested = true);
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
            var sut = new AttributeRelay<ValidationAttribute>((r, a) => derivedAttributeObtained = true);
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
            Func<ICustomAttributeProvider, RangeAttribute, object> createRelayedRequest = (r, a) => 
            {
                requestReceivedByCreateRelayedRequest = r;
                attributeReceivedByCreateRelayedRequest = a;
                return default(object);
            };
            var sut = new AttributeRelay<RangeAttribute>(createRelayedRequest);
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
            var sut = new AttributeRelay<RequiredAttribute>((r, a) => relayedRequest);
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
            var sut = new AttributeRelay<RequiredAttribute>((r, a) => new object());
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
            var sut = new AttributeRelay<Attribute>((r, a) => new object());
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
            var sut = new AttributeRelay<RequiredAttribute>((element, attribute) => new object());
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
    }
}
