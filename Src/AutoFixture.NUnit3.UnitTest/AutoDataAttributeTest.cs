using System;
using System.Linq;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// This is used in BuildFromYieldsParameterValues for building a unit test method
        /// </summary>
        public void DummyTestMethod(int anyInt, double anyDouble)
        {
        }

        [Test]
        public void CanBeExtendedToTakeAnIFixture()
        {
            var stub = new AutoDataAttributeStub(new ThrowingStubFixture());

            Assert.That(stub, Is.AssignableTo<AutoDataAttribute>());
        }

        [Test]
        public void IfCreateParametersThrowsExceptionThenReturnsNotRunnableTestMethodWithExceptionInfoAsSkipReason()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var autoDataAttributeStub = new AutoDataAttributeStub(new ThrowingStubFixture());

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

        /// <summary>
        /// A <see cref="IFixture"/> for the benefit of creating stubs of <see cref="AutoDataAttribute"/> for unit testing
        /// </summary>
        private class ThrowingStubFixture : IFixture
        {
            public System.Collections.Generic.IList<Kernel.ISpecimenBuilderTransformation> Behaviors
            {
                get { throw new NotImplementedException(); }
            }

            public System.Collections.Generic.IList<Kernel.ISpecimenBuilder> Customizations
            {
                get { throw new NotImplementedException(); }
            }

            public bool OmitAutoProperties
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int RepeatCount
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public System.Collections.Generic.IList<Kernel.ISpecimenBuilder> ResidueCollectors
            {
                get { throw new NotImplementedException(); }
            }

            public Dsl.ICustomizationComposer<T> Build<T>()
            {
                throw new NotImplementedException();
            }

            public IFixture Customize(ICustomization customization)
            {
                throw new NotImplementedException();
            }

            public void Customize<T>(Func<Dsl.ICustomizationComposer<T>, Kernel.ISpecimenBuilder> composerTransformation)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// This throws a unique <see cref="DummyException"/> for testing purpose
            /// </summary>
            /// <param name="request"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public object Create(object request, Kernel.ISpecimenContext context)
            {
                throw new DummyException();
            }

            /// <summary>
            /// A unique exception for the benefit of unit testing
            /// </summary>
            public class DummyException : Exception
            {
            }
        }
    }
}
