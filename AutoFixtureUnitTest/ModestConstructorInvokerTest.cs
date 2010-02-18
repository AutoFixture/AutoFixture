using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ModestConstructorInvokerTest
    {
        [TestMethod]
        public void SutIsInstanceGeneratorNode()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            // Exercise system
            var sut = new ModestConstructorInvoker(dummyParent);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            var sut = new ModestConstructorInvoker(dummyParent);
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNonTypeWillReturnCorrectResult()
        {
            // Fixture setup
            var anonymousPropertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var dummyParent = new MockInstanceGenerator();
            var sut = new ModestConstructorInvoker(dummyParent);
            // Exercise system
            var result = sut.CanGenerate(anonymousPropertyInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForTypeWhenParentCannotGenerateWillReturnCorrectResult()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator();
            parent.CanGenerateCallback = r => false;
            var sut = new ModestConstructorInvoker(parent);
            // Exercise system
            var result = sut.CanGenerate(typeof(SingleParameterType<object>));
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForTypeWithNoPuplicConstructorWillReturnCorrectResult()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator();
            parent.GenerateCallback = r => true;
            var sut = new ModestConstructorInvoker(parent);
            // Exercise system
            var result = sut.CanGenerate(typeof(AbstractType));
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForTypeWhenParentCanGenerateOneParameterButNotTheOtherWillReturnCorrectResult()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<string, int>);

            var parent = new MockInstanceGenerator();
            parent.CanGenerateCallback = r => typeof(string) == r;

            var sut = new ModestConstructorInvoker(parent);
            // Exercise system
            var result = sut.CanGenerate(requestedType);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateWhenParentCanGenerateBothParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<int, decimal>);

            var parent = new MockInstanceGenerator();
            parent.CanGenerateCallback = r => typeof(int) == r || typeof(decimal) == r;

            var sut = new ModestConstructorInvoker(parent);
            // Exercise system
            var result = sut.CanGenerate(requestedType);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnCorrectResult()
        {
            // Fixture setup
            var requestedType = typeof(SingleParameterType<ConcreteType>);
            var expectedParameter = new ConcreteType();

            var parent = new MockInstanceGenerator();
            parent.CanGenerateCallback = r => true;
            parent.GenerateCallback = r => expectedParameter;

            var sut = new ModestConstructorInvoker(parent);
            // Exercise system
            var result = sut.Generate(requestedType);
            // Verify outcome
            Assert.AreEqual(expectedParameter, ((SingleParameterType<ConcreteType>)result).Parameter, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeParentCorrectly()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<long, short>);

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = r => typeof(long) == r || typeof(short) == r;
            parentMock.GenerateCallback = r =>
                {
                    if (typeof(long) == r)
                    {
                        return new long();
                    }
                    if (typeof(short) == r)
                    {
                        return new short();
                    }
                    throw new AssertFailedException("Unexpected parent request.");
                };

            var sut = new ModestConstructorInvoker(parentMock);
            // Exercise system
            sut.Generate(requestedType);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
