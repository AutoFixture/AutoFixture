using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class StringSeedUnwrapperTest
    {
        [TestMethod]
        public void SutIsInstanceGneeratorNode()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            // Exercise system
            var sut = new StringSeedUnwrapper(dummyParent);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNullProviderWillReturnCorrectResult()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            var sut = new StringSeedUnwrapper(dummyParent);
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForAnonymousProviderWillReturnCorrectResult()
        {
            // Fixture setup
            var anonymousProvider = typeof(GenericUriParser);
            var sut = new StringSeedUnwrapper(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(anonymousProvider);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForSeedWithWrongTypeWillReturnCorrectResult()
        {
            // Fixture setup
            var seed = new Seed(typeof(HttpStyleUriParser), new object());
            var sut = new StringSeedUnwrapper(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(seed);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenereateForSeedWithWrongValueTypeWillReturnCorrectResult()
        {
            // Fixture setup
            var seed = new Seed(typeof(string), new ModuleHandle());
            var sut = new StringSeedUnwrapper(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(seed);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForCorrectSeedWhenParentCannotGenerateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var seed = new Seed(typeof(string), "Anonymous value");
            var sut = new StringSeedUnwrapper(new MockInstanceGenerator { CanGenerateCallback = ap => false });
            // Exercise system
            var result = sut.CanGenerate(seed);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForCorrectSeedWhenParentCanGenerateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var correctType = typeof(string);
            var seed = new Seed(correctType, "Anonymous value");
            var sut = new StringSeedUnwrapper(new MockInstanceGenerator { CanGenerateCallback = correctType.Equals });
            // Exercise system
            var result = sut.CanGenerate(seed);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnCorrectResult()
        {
            // Fixture setup
            var seed = new Seed(typeof(string), "Anonymous seed");
            var parentString = "Anonymous parent-generated text";
            var expectedInstance = seed.Value + parentString;

            var parentStub = new MockInstanceGenerator();
            parentStub.CanGenerateCallback = ap => true;
            parentStub.GenerateCallback = ap => parentString;

            var sut = new StringSeedUnwrapper(parentStub);
            // Exercise system
            var result = sut.Generate(seed);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeParentCorectly()
        {
            // Fixture setup
            var seed = new Seed(typeof(string), "Anonymous seed");

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = seed.TargetType.Equals;
            parentMock.GenerateCallback = ap =>
                {
                    Assert.AreEqual(seed.TargetType, ap, "Generate");
                    return "Anonymous result";
                };

            var sut = new StringSeedUnwrapper(parentMock);
            // Exercise system
            sut.Generate(seed);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
