using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class CoalescingInstanceGeneratorTest
    {
        [TestMethod]
        public void SutIsInstanceGenerator()
        {
            // Fixture setup
            // Exercise system
            var sut = new CoalescingInstanceGenerator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IInstanceGenerator));
            // Teardown
        }

        [TestMethod]
        public void GeneratorsIsInstance()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            // Exercise system
            IList<IInstanceGenerator> result = sut.Generators;
            // Verify outcome
            Assert.IsNotNull(result, "Generators");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateWillReturnFalseWhenNoChildrenCanGenerate()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });

            var dummyRequest = typeof(object);
            // Exercise system
            var result = sut.CanGenerate(dummyRequest);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateWillReturnTrueWhenAtLeastOneChildCanGenerate()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => true });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });

            var dummyRequest = typeof(object);
            // Exercise system
            var result = sut.CanGenerate(dummyRequest);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateWillInvokeChildWithCorrectRequest()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            var expectedRequest = typeof(object).GetConstructor(Type.EmptyTypes);

            var generatorMock = new MockInstanceGenerator();
            generatorMock.CanGenerateCallback = r =>
                {
                    Assert.AreEqual(expectedRequest, r, "CanGenerate");
                    return true;
                };
            sut.Generators.Add(generatorMock);
            // Exercise system
            sut.CanGenerate(expectedRequest);
            // Verify outcome (done by mock)
            // Teardown
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GenerateWhenNoChildreCanGenerateWillThrow()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            sut.Generators.Add(new MockInstanceGenerator { GenerateCallback = r => false });

            var dummyRequest = typeof(object);
            // Exercise system
            sut.Generate(dummyRequest);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnFirstResultFromChild()
        {
            // Fixture setup
            var request = typeof(List<decimal>).GetConstructor(Type.EmptyTypes);
            var expectedInstance = new object();

            var sut = new CoalescingInstanceGenerator();
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = r => false });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = request.Equals, GenerateCallback = r => expectedInstance });
            sut.Generators.Add(new MockInstanceGenerator { CanGenerateCallback = request.Equals, GenerateCallback = r => new object() });
            // Exercise system
            var result = sut.Generate(request);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeChildWithCorrectRequest()
        {
            // Fixture setup
            var sut = new CoalescingInstanceGenerator();
            var expectedRequest = typeof(string);

            var generatorMock = new MockInstanceGenerator();
            generatorMock.CanGenerateCallback = expectedRequest.Equals;
            generatorMock.GenerateCallback = r =>
                {
                    Assert.AreEqual(expectedRequest, r, "Generate");
                    return new object();
                };
            sut.Generators.Add(generatorMock);
            // Exercise system
            sut.Generate(expectedRequest);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
