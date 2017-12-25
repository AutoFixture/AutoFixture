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
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AutoDataAttributeStub((Func<IFixture>) null));
        }
        
        [Test]
        public void FixtureFactoryIsNotInvokedImmediately()
        {
            // Arrange
            bool wasInvoked = false;
            Func<IFixture> fixtureFactory = () =>
            {
                wasInvoked = true;
                return null;
            };

            // Act
            var sut = new AutoDataAttributeStub(fixtureFactory);
            
            // Assert
            Assert.False(wasInvoked);
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
            // Arrange
            bool wasActivated = false;
            
            var sut = new AutoDataAttributeStub(() =>
            {
                wasActivated = true;
                return null;
            });
            sut.TestMethodBuilder = new TestMethodBuilderWithoutParametersUsage();
            
            var methodWrapper = new MethodWrapper(this.GetType(), nameof(this.DummyTestMethod));
            var testSuite = new TestSuite(this.GetType());
            
            // Assert
            var dummy = sut.BuildFrom(methodWrapper, testSuite).ToArray();

            // Assert
            Assert.IsFalse(wasActivated);
        }

        [Test]
        public void InitializedWithArgumentsHasCorrectArguments()
        {
            // Arrange
            var expectedArguments = new object[] { };
            var sut = new InlineAutoDataAttribute(expectedArguments);
            // Act
            var result = sut.Arguments;
            // Assert
            Assert.AreSame(expectedArguments, result);
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
            // Arrange
            var method = new MethodWrapper(typeof(TypeWithCustomizationAttributes), methodName);
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new InlineAutoDataAttributeStub(() => fixture);
            // Act
            sut.BuildFrom(method, new TestSuite(this.GetType())).Single();
            // Assert
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
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
            // Arrange
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

            // Act
            sut.BuildFrom(method, new TestSuite(this.GetType())).ToArray();
            // Assert
            Assert.True(customizationLog[0] is TypeWithIParameterCustomizationSourceUsage.Customization);
        }
    }
}
