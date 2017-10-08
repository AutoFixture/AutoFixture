using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTest
    {
        [Test]
        public void ExtendsAttribute()
        {
            Assert.That(new AutoDataAttribute(), Is.InstanceOf<Attribute>());
        }

        [Test]
        public void IfExtendedWithNullFixtureThenThrows()
        {
#pragma warning disable 612
            Assert.Throws<ArgumentNullException>(() => new AutoDataAttributeStub((IFixture)null));
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
        public void ImplementsITestBuilder()
        {
            var autoDataFixture = new AutoDataAttribute();
            Assert.That(autoDataFixture, Is.InstanceOf<ITestBuilder>());
        }

        [Test]
        public void BuildFromYieldsRunnableTestMethod()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod("DummyTestMethod"));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = autoDataAttribute.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.Runnable);
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

        /// <summary>
        /// This is used in BuildFromYieldsParameterValues for building a unit test method
        /// </summary>
        public void DummyTestMethod(int anyInt, double anyDouble)
        {
        }

        [Test]
        public void CanBeExtendedToTakeAnIFixture()
        {
#pragma warning disable 612
            var stub = new AutoDataAttributeStub(new ThrowingStubFixture());
#pragma warning restore 612

            Assert.That(stub, Is.AssignableTo<AutoDataAttribute>());
        }

        [Test]
        public void IfCreateParametersThrowsExceptionThenReturnsNotRunnableTestMethodWithExceptionInfoAsSkipReason()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var autoDataAttributeStub = new AutoDataAttributeStub(() => new ThrowingStubFixture());

            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod("DummyTestMethod"));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = autoDataAttributeStub.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.NotRunnable);

            Assert.That(testMethod.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo(ExceptionHelper.BuildMessage(new ThrowingStubFixture.DummyException())));
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
            var sut = new AutoDataAttributeStub(() => fixture);
            // Exercise system
            sut.BuildFrom(method, new TestSuite(this.GetType())).Single();
            // Verify outcome
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
            // Teardown
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
            public void DecoratedMethod([CustomizationSourceAttribute] int arg)
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
            var sut = new AutoDataAttributeStub(() => fixture);

            // Exercise system
            sut.BuildFrom(method, new TestSuite(this.GetType())).ToArray();
            // Verify outcome
            Assert.True(customizationLog[0] is TypeWithIParameterCustomizationSourceUsage.Customization);
            // Teardown
        }
    }
}
