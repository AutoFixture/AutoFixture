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
    public class ValueIgnoringSeedUnwrapperTest
    {
        [TestMethod]
        public void SutIsInstanceGeneratorNode()
        {
            // Fixture setup
            var dummyParent = new MockInstanceGenerator();
            // Exercise system
            var sut = new ValueIgnoringSeedUnwrapper(dummyParent);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void CanGeneratorForNullProviderWillReturnFalse()
        {
            // Fixture setup
            var sut = new ValueIgnoringSeedUnwrapper(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForAnonymousProviderWillReturnFalse()
        {
            // Fixture setup
            var anonymousProvider = typeof(PropertyHolder<string>).GetProperty("Property");
            var sut = new ValueIgnoringSeedUnwrapper(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(anonymousProvider);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForSeedWhenParentCannotGenerateSeedTypeWillReturnFalse()
        {
            // Fixture setup
            var seed = new Seed(typeof(ASCIIEncoding), new object());
            var sut = new ValueIgnoringSeedUnwrapper(new MockInstanceGenerator { CanGenerateCallback = ap => false });
            // Exercise system
            var result = sut.CanGenerate(seed);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateWhenParentCanGenerateTargetTypeWillReturnFalse()
        {
            // Fixture setup
            var seed = new Seed(typeof(ConcreteType), new ConcreteType());

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = seed.TargetType.Equals;

            var sut = new ValueIgnoringSeedUnwrapper(parentMock);
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
            var seed = new Seed(typeof(ConcreteType), null);
            var expectedInstance = new object();

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = ap => true;
            parentMock.GenerateCallback = ap => expectedInstance;

            var sut = new ValueIgnoringSeedUnwrapper(parentMock);
            // Exercise system
            var result = sut.Generate(seed);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeParentCorrectly()
        {
            // Fixture setup
            var seed = new Seed(typeof(FtpStyleUriParser), TimeSpan.FromDays(1));

            var parentMock = new MockInstanceGenerator();
            parentMock.CanGenerateCallback = seed.TargetType.Equals;
            parentMock.GenerateCallback = ap =>
                {
                    Assert.AreEqual(seed.TargetType, ap, "Generate");
                    return new object();
                };

            var sut = new ValueIgnoringSeedUnwrapper(parentMock);
            // Exercise system
            sut.Generate(seed);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
