using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class InlineAutoDataAttributeTest
    {
        [Test]
        public void IfArguementsIsNullThenThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataAttribute(null));
        }

        [Test]
        public void IfExtendedWithNullFixtureThenThrows()
        {
#pragma warning disable 612
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataAttributeStub((IFixture)null));
#pragma warning restore 612
        }
        
        [Test]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoDataAttributeStub((Func<IFixture>) null));
            // Teardown
        }
        
        [Test]
        public void FixtureFactoryIsNotInvokedImmediately()
        {
            // Fixture setup
            bool wasInvoked = false;
            Func<IFixture> fixtureFactory = () =>
            {
                wasInvoked = true;
                return null;
            };

            // Exercise system
            var sut = new AutoDataAttributeStub(fixtureFactory);
            
            // Verify outcome
            Assert.False(wasInvoked);
            // Teardown
        }
        

        [Test]
        public void IfCreateParametersThrowsExceptionThenReturnsNotRunnableTestMethodWithExceptionInfoAsSkipReason()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var inlineAutoDataAttributeStub = new InlineAutoDataAttributeStub(() => new ThrowingStubFixture());

            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod(nameof(this.DummyTestMethod)));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = inlineAutoDataAttributeStub.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.NotRunnable);

            Assert.That(testMethod.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo(ExceptionHelper.BuildMessage(new ThrowingStubFixture.DummyException())));
        }
        
        [Test]
        public void BuildFromDontActivateFixtureIfArgsValuesAreNotUsedByTestBuilder()
        {
            // Fixture setup
            bool wasActivated = false;
            
            var sut = new AutoDataAttributeStub(() =>
            {
                wasActivated = true;
                return null;
            });
            sut.TestMethodBuilder = new TestMethodBuilderWithoutParametersUsage();
            
            var methodWrapper = new MethodWrapper(this.GetType(), nameof(this.DummyTestMethod));
            var testSuite = new TestSuite(this.GetType());
            
            // Excercise system
            var dummy = sut.BuildFrom(methodWrapper, testSuite).ToArray();

            // Verify outcome
            Assert.IsFalse(wasActivated);
            // Teardown
        }

        [Test]
        public void InitializedWithArgumentsHasCorrectArguments()
        {
            // Fixture setup
            var expectedArguments = new object[] { };
            var sut = new InlineAutoDataAttribute(expectedArguments);
            // Exercise system
            var result = sut.Arguments;
            // Verify outcome
            Assert.AreSame(expectedArguments, result);
            // Teardown
        }
        
        [TestCase("CreateWithFrozenAndFavorArrays")]
        [TestCase("CreateWithFavorArraysAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorEnumerables")]
        [TestCase("CreateWithFavorEnumerablesAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorLists")]
        [TestCase("CreateWithFavorListsAndFrozen")]
        [TestCase("CreateWithFrozenAndGreedy")]
        [TestCase("CreateWithGreedyAndFrozen")]
        [TestCase("CreateWithFrozenAndModest")]
        [TestCase("CreateWithModestAndFrozen")]
        [TestCase("CreateWithFrozenAndNoAutoProperties")]
        [TestCase("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Fixture setup
            var method = new MethodWrapper(typeof(TypeWithCustomizationAttributes), methodName);
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new InlineAutoDataAttributeStub(() => fixture);
            // Exercise system
            sut.BuildFrom(method, new TestSuite(this.GetType())).Single();
            // Verify outcome
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
            // Teardown
        }

        /// <summary>
        /// This is used in BuildFromYieldsParameterValues for building a unit test method
        /// </summary>
        public void DummyTestMethod(int anyInt, double anyDouble)
        {
        }
        
        private class TestMethodBuilderWithoutParametersUsage: ITestMethodBuilder
        {
            public TestMethod Build(
                IMethodInfo method, Test suite, IEnumerable<object> parameterValues, int autoDataStartIndex)
            {
                return new TestMethod(method);
            }
        }

        private class TypeWithIParameterCustomizationSourceUsage
        {
            public void DecoratedMethod(int dummy, [CustomizationSource] int customizedArg)
            {
            }

            public class CustomizationSourceAttribute : Attribute, IParameterCustomizationSource
            {
                public ICustomization GetCustomization(ParameterInfo parameter)
                {
                    return new Customization();
                }
            }

            public class Customization : ICustomization
            {
                public void Customize(IFixture fixture)
                {
                }
            }
        }

        [Test]
        public void ShouldRecognizeAttributesImplementingIParameterCustomizationSource()
        {
            // Fixture setup
            var method = new MethodWrapper(
                typeof(TypeWithIParameterCustomizationSourceUsage),
                nameof(TypeWithIParameterCustomizationSourceUsage.DecoratedMethod));

            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new InlineAutoDataAttributeStub(() => fixture, new[] {42});

            // Exercise system
            sut.BuildFrom(method, new TestSuite(this.GetType())).ToArray();
            // Verify outcome
            Assert.True(customizationLog[0] is TypeWithIParameterCustomizationSourceUsage.Customization);
            // Teardown
        }
    }
}
