using System;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TestDataProviderTest
    {
        private readonly MethodInfo methodWithOneParameter = new Action<object>(MethodWithOneParameter).Method;
        private readonly MethodInfo methodWithTwoParameters = new Action<object, object>(MethodWithTwoParameters).Method;

        [Fact]
        public void SutIsITestDataProvider()
        {
            // Fixture setup
            var sut = new TestDataProvider(new Fixture());
            // Exercise system and verify outcome
            Assert.IsAssignableFrom<ITestDataProvider>(sut);
            // Teardown
        }

        [Fact]
        public void ConsturctorThrowsArgumentNullExceptionsWhenFixtureIsNull()
        {
            // Fixture setup, exercise system and verify outcome
            var e = Assert.Throws<ArgumentNullException>(() => new TestDataProvider(null));
            Assert.Equal("fixture", e.ParamName);
            // Teardown
        }

        [Fact]
        public void GetDataThrowsArgumentNullExceptionWhenTestMethodIsNull()
        {
            // Fixture setup
            var sut = new TestDataProvider(new Fixture());
            // Exercise system and verify outcome
            var e = Assert.Throws<ArgumentNullException>(() => sut.GetData(null));
            Assert.Equal("testMethod", e.ParamName);
            // Teardown
        }

        [Fact]
        public void GetDataAppliesCustomizationsFromAllParameterAttributes()
        {
            // Fixture setup
            var customizationTypes = new List<Type>();
            IFixture fixture = CreateTestFixture(customizationTypes);
            var sut = new TestDataProvider(fixture);
            // Exercise system
            sut.GetData(this.methodWithOneParameter);
            // Verify outcome
            Assert.Equal(new[] { typeof(ParameterCustomization), typeof(ParameterCustomization) }, customizationTypes);
            // Teardown
        }

        [Fact]
        public void GetDataAppliesCustomizationsFromAllParameters()
        {
            // Fixture setup
            var customizationTypes = new List<Type>();
            IFixture fixture = CreateTestFixture(customizationTypes);
            var sut = new TestDataProvider(fixture);
            // Exercise system
            sut.GetData(this.methodWithTwoParameters);
            // Verify outcome
            Assert.Equal(new[] { typeof(ParameterCustomization), typeof(ParameterCustomization) }, customizationTypes);
            // Teardown
        }

        [Fact]
        public void GetDataResolvesParameterWithCorrectSpecimenContext()
        {
            // Fixture setup
            ISpecimenContext createContext = null;
            var fixture = new DelegatingFixture();
            fixture.OnCreate = (request, context) =>
            {
                createContext = context;
                return new object();
            };
            var sut = new TestDataProvider(fixture);
            // Excercise system
            sut.GetData(this.methodWithOneParameter);
            // Verify outcome
            var specimenContext = Assert.IsType<SpecimenContext>(createContext);
            Assert.Same(fixture, specimenContext.Builder);
            // Teardown
        }

        [Fact]
        public void GetDataResolvesAllParametersUsingFixture()
        {
            // Fixture setup
            var createRequests = new List<object>();
            var fixture = new DelegatingFixture();
            fixture.OnCreate = (request, context) =>
            {
                createRequests.Add(request);
                return new object();
            };
            var sut = new TestDataProvider(fixture);
            // Excercise system
            sut.GetData(this.methodWithTwoParameters);
            // Verify outcome
            Assert.Equal(this.methodWithTwoParameters.GetParameters(), createRequests);
            // Teardown
        }

        [Fact]
        public void GetDataReturnsSpecimensGeneratedByFixtureForAllParameters()
        {
            // Fixture setup
            var specimens = new List<object>();
            var fixture = new DelegatingFixture();
            fixture.OnCreate = (request, context) =>
            {
                var specimen = new object();
                specimens.Add(specimen);
                return specimen;
            };
            var sut = new TestDataProvider(fixture);
            // Excercise system
            IEnumerable<object> arguments = sut.GetData(this.methodWithTwoParameters);
            // Verify outcome
            Assert.Equal(specimens, arguments);
            // Teardown
        }

        private static IFixture CreateTestFixture(ICollection<Type> customizationTypes)
        {
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = customization =>
            {
                customizationTypes.Add(customization.GetType());
                return fixture;
            };
            return fixture;
        }

        private static void MethodWithOneParameter([ParameterCustomization, ParameterCustomization] object parameter)
        {
        }

        private static void MethodWithTwoParameters([ParameterCustomization] object parameter1, [ParameterCustomization] object parameter2)
        {
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
        private class ParameterCustomizationAttribute : Attribute, IParameterCustomizationProvider
        {
            public ICustomization GetCustomization(ParameterInfo parameter)
            {
                return new ParameterCustomization();
            }
        }

        private class ParameterCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
            }
        }
    }
}
