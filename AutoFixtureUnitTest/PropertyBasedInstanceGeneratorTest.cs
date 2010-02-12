using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class PropertyBasedInstanceGeneratorTest
    {
        [TestMethod]
        public void SutIsInstanceGenerator()
        {
            // Fixture setup
            // Exercise system
            var sut = new PropertyBasedInstanceGenerator(new MockInstanceGenerator());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNullMemberWillReturnFalse()
        {
            // Fixture setup
            var sut = new PropertyBasedInstanceGenerator(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(null);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForNonPropertyWillReturnFalse()
        {
            // Fixture setup
            var fieldInfo = typeof(FieldHolder<int>).GetField("Field");
            var sut = new PropertyBasedInstanceGenerator(new MockInstanceGenerator());
            // Exercise system
            var result = sut.CanGenerate(fieldInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForPropertyWhenParentCannotGenerateValueWillReturnFalse()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var parent = new MockInstanceGenerator { CanGenerateCallback = ap => false };
            var sut = new PropertyBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.CanGenerate(propertyInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void CanGenerateForPropertyWhenParentCanGenerateValueWillReturnTrue()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var parent = new MockInstanceGenerator { CanGenerateCallback = ap => true };
            var sut = new PropertyBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.CanGenerate(propertyInfo);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnCorrectResult()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var expectedInstance = new object();
            var parent = new MockInstanceGenerator
            {
                CanGenerateCallback = ap => true,
                GenerateCallback = ap => expectedInstance
            };

            var sut = new PropertyBasedInstanceGenerator(parent);
            // Exercise system
            var result = sut.Generate(propertyInfo);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void GenerateWillInvokeParentCorrectly()
        {
            // Fixture setup
            var propertyName = "Property";
            var propertyInfo = typeof(PropertyHolder<string>).GetProperty(propertyName);
            var expectedSeed = new Seed(propertyInfo.PropertyType, propertyName);

            var parent = new MockInstanceGenerator
            {
                CanGenerateCallback = expectedSeed.Equals,
                GenerateCallback = ap =>
                    {
                        Assert.AreEqual(expectedSeed, ap, "Generate");
                        return new object();
                    }
            };

            var sut = new PropertyBasedInstanceGenerator(parent);
            // Exercise system
            sut.Generate(propertyInfo);
            // Verify outcome (done by mock)
            // Teardown
        }
    }
}
