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
    public class ParameterBasedInstanceGeneratorTest
    {
        [TestMethod]
        public void SutIsInstanceGeneratorNode()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            // Exercise system
            var sut = new ParameterBasedInstanceGenerator(dummyParent);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNullMemberWillReturnFalse()
        {
            // Fixture setup
            var sut = new ParameterBasedInstanceGenerator(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNonParameterWillReturnFalse()
        {
            // Fixture setup
            var fieldInfo = typeof(FieldHolder<int>).GetField("Field");
            var sut = new ParameterBasedInstanceGenerator(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(fieldInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForParameterWhenParentCannotGenerateValueWillReturnFalse()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var parent = new MockInstanceGenerator { CanGenerateCallback = ap => false };
            var sut = new ParameterBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.CanGenerate(parameterInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForParameterWhenParentCanGenerateValueWillReturnTrue()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var parent = new MockInstanceGenerator { CanGenerateCallback = ap => true };
            var sut = new ParameterBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.CanGenerate(parameterInfo);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnCorrectResult()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var expectedInstance = new object();

            var parent = new MockInstanceGenerator();
            parent.CanGenerateCallback = ap => true;
            parent.GenerateCallback = ap => expectedInstance;

            var sut = new ParameterBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.Generate(parameterInfo);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeParentCorrectly()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var expectedSeed = new Seed(parameterInfo.ParameterType, parameterInfo.Name);

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = expectedSeed.Equals;
            parentMock.GenerateCallback = ap =>
                {
                    Assert.AreEqual(expectedSeed, ap, "Generate");
                    return new object();
                };

            var sut = new ParameterBasedInstanceGenerator(parentMock);
            // Exercise system
            sut.Generate(parameterInfo);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
