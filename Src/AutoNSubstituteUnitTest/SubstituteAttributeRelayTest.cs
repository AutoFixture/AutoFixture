using System;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteAttributeRelayTest
    {
        [Fact]
        public void SutInheritsCommonLogicFromAttributeRelayWhichIsTestedSeparately()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(AttributeRelay<SubstituteAttribute>).IsAssignableFrom(typeof(SubstituteAttributeRelay)));
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestReturnsSubstituteRequestWithTypeOfGivenParameter()
        {
            // Fixture setup
            var sut = new TestableSubstituteAttributeRelay();
            Type parameterType = typeof(IInterface);
            var parameter = Substitute.For<ParameterInfo>();
            parameter.ParameterType.Returns(parameterType);
            // Exercise system
            object result = sut.CreateRelayedRequest(parameter, new SubstituteAttribute());
            // Verify outcome
            var substituteRequest = Assert.IsType<SubstituteRequest>(result);
            Assert.Same(parameterType, substituteRequest.TargetType);
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestReturnsSubstituteRequestWithTypeOfGivenProperty()
        {
            // Fixture setup
            var sut = new TestableSubstituteAttributeRelay();
            Type propertyType = typeof(IInterface);
            var property = Substitute.For<PropertyInfo>();
            property.PropertyType.Returns(propertyType);
            // Exercise system
            object result = sut.CreateRelayedRequest(property, new SubstituteAttribute());
            // Verify outcome
            var substituteRequest = Assert.IsType<SubstituteRequest>(result);
            Assert.Same(propertyType, substituteRequest.TargetType);
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestReturnsSubstiteRequestWithTypeOfGivenField()
        {
            // Fixture setup
            var sut = new TestableSubstituteAttributeRelay();
            Type fieldType = typeof(IInterface);
            var field = Substitute.For<FieldInfo>();
            field.FieldType.Returns(fieldType);
            // Exercise system
            object result = sut.CreateRelayedRequest(field, new SubstituteAttribute());
            // Verify outcome
            var substituteRequest = Assert.IsType<SubstituteRequest>(result);
            Assert.Same(fieldType, substituteRequest.TargetType);
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestThrowsNotSupportedExceptionWhenAttributeIsAppliedToUnexpectedCodeElement()
        {
            // Fixture setup
            var sut = new TestableSubstituteAttributeRelay();
            var @event = Substitute.For<EventInfo>();
            var attribute = new SubstituteAttribute();
            // Exercise system
            var e = Assert.Throws<NotSupportedException>(() => sut.CreateRelayedRequest(@event, attribute));
            // Verify outcome
            Assert.Contains(attribute.ToString(), e.Message);
            Assert.Contains(@event.ToString(), e.Message);
            // Teardown
        }

        private class TestableSubstituteAttributeRelay : SubstituteAttributeRelay
        {
            public new object CreateRelayedRequest(ICustomAttributeProvider request, SubstituteAttribute attribute)
            {
                return base.CreateRelayedRequest(request, attribute);
            }
        }
    }
}
