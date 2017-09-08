using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ModestConstructorInvokerTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ModestConstructorInvoker();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateFromNonTypeRequestWillReturnNull()
        {
            // Fixture setup
            var nonTypeRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(nonTypeRequest, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromTypeRequestWhenContainerCannotSatisfyParameterRequestWillReturnNull()
        {
            // Fixture setup
            var type = typeof(string);
            var container = new DelegatingSpecimenContainer { OnCreate = r => null };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(type, container);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromTypeWithNoPublicConstructorWhenContainerCanSatisfyRequestWillReturnNull()
        {
            // Fixture setup
            var container = new DelegatingSpecimenContainer { OnCreate = r => new object() };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(typeof(AbstractType), container);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromTypeWhenParentCanGenerateOneParameterButNotTheOtherWillReturnCorrectNull()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<string, int>);
            var container = new DelegatingSpecimenContainer { OnCreate = r => typeof(string) == r ? new object() : null };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(requestedType, container);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromTypeWhenParentCanGenerateBothParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedParameterValues = new object[] { 1, 2m };
            var parameterQueue = new Queue<object>(expectedParameterValues);

            var requestedType = typeof(DoubleParameterType<int, decimal>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();

            var container = new DelegatingSpecimenContainer();
            container.OnCreate = r =>
            {
                if (parameters.Any(r.Equals))
                {
                    return parameterQueue.Dequeue();
                }
                return null;
            };

            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(requestedType, container);
            // Verify outcome
            var actual = (DoubleParameterType<int, decimal>)result;
            Assert.AreEqual(expectedParameterValues[0], actual.Parameter1, "Create");
            Assert.AreEqual(expectedParameterValues[1], actual.Parameter2, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromTypeWillInvokeContainerCorrectly()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<long, short>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnCreate = r =>
                {
                    if (parameters.Any(r.Equals))
                    {
                        mockVerified = true;
                        var pType = ((ParameterInfo)r).ParameterType;
                        if (typeof(long) == pType)
                        {
                            return new long();
                        }
                        if (typeof(short) == pType)
                        {
                            return new short();
                        }
                    }
                    throw new AssertFailedException("Unexpected container request.");
                };

            var sut = new ModestConstructorInvoker();
            // Exercise system
            sut.Create(requestedType, containerMock);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
