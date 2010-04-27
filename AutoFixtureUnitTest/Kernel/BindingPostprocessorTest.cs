using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.TestTypeFoundation;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class BindingPostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Expression<Func<PropertyHolder<string>, string>> dummyExpression = ph => ph.Property;
            // Exercise system
            var sut = new BindingPostprocessor<PropertyHolder<string>, string>(dummyBuilder, dummyExpression);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<int>, int>> dummyExpression = ph => 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingPostprocessor<PropertyHolder<int>, int>(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPropertyPickerThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BindingPostprocessor<FieldHolder<decimal>, decimal>(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContainer { OnCreate = r => 0m };

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        verified = r == expectedRequest && c == expectedContainer;
                        return new PropertyHolder<decimal>();
                    }
            };

            Expression<Func<PropertyHolder<decimal>, decimal>> dummyPropertyPicker = ph => ph.Property;
            var sut = new BindingPostprocessor<PropertyHolder<decimal>, decimal>(builderMock, dummyPropertyPicker);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new PropertyHolder<decimal>();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            Expression<Func<PropertyHolder<decimal>, decimal>> dummyPropertyPicker = ph => ph.Property;
            var sut = new BindingPostprocessor<PropertyHolder<decimal>, decimal>(builder, dummyPropertyPicker);

            var container = new DelegatingSpecimenContainer { OnCreate = r => 0m };
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWillMakeCorrectPropertyRequestOfContainer()
        {
            // Fixture setup
            var expectedRequest = typeof(PropertyHolder<bool>).GetProperty("Property");
            var verified = false;
            var containerMock = new DelegatingSpecimenContainer { OnCreate = r => verified = expectedRequest.Equals(r) };

            var dummyBuilder = new DelegatingSpecimenBuilder { OnCreate = (c, r) => new PropertyHolder<bool>() };
            var sut = new BindingPostprocessor<PropertyHolder<bool>, bool>(dummyBuilder, ph => ph.Property);
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, containerMock);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void InitializeWithNonMemberExpressionWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Expression<Func<object, object>> invalidExpression = obj => obj;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingPostprocessor<object, object>(dummyBuilder, invalidExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithMethodExpressionWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Expression<Func<object, string>> methodExpression = obj => obj.ToString();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingPostprocessor<object, string>(dummyBuilder, methodExpression));
            // Teardown
        }

        [Fact]
        public void InitializeWithReadOnlyPropertyExpressionWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Expression<Func<SingleParameterType<object>, object>> readOnlyPropertyExpression = sp => sp.Parameter;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new BindingPostprocessor<SingleParameterType<object>, object>(dummyBuilder, readOnlyPropertyExpression));
            // Teardown
        }

        [Fact]
        public void CreateWillAssignCorrectPropertyValue()
        {
            // Fixture setup
            var expectedProperty = new object();
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedProperty };
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new PropertyHolder<object>() };
            var sut = new BindingPostprocessor<PropertyHolder<object>, object>(builder, ph => ph.Property);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            var actual = Assert.IsAssignableFrom<PropertyHolder<object>>(result);
            Assert.Equal(actual.Property, expectedProperty);
            // Teardown
        }

        [Fact]
        public void CreateWillAssignCorrectFieldValue()
        {
            // Fixture setup
            var expectedField = new object();
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedField };
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new FieldHolder<object>() };
            var sut = new BindingPostprocessor<FieldHolder<object>, object>(builder, fh => fh.Field);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            var actual = Assert.IsAssignableFrom<FieldHolder<object>>(result);
            Assert.Equal(actual.Field, expectedField);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenBuilderReturnsIncompatibleType()
        {
            // Fixture setup
            var nonInt = "Anonymous variable";
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => nonInt };

            Expression<Func<PropertyHolder<int>, int>> dummyExpression = ph => ph.Property;
            var sut = new BindingPostprocessor<PropertyHolder<int>, int>(builder, dummyExpression);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, dummyContainer));
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenContainerReturnsIncompatibleTypeForProperty()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new PropertyHolder<decimal>() };
            var sut = new BindingPostprocessor<PropertyHolder<decimal>, decimal>(builder, ph => ph.Property);

            var container = new DelegatingSpecimenContainer { OnCreate = r => "Not a decimal" };
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, container));
            // Teardown
        }
    }
}
