using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class InstanceGeneratorNodeTest
    {
        [TestMethod]
        public void SutIsInstanceGenerator()
        {
            // Fixture setup
            // Exercise system
            var sut = new TestableInstanceGenerator(new MockInstanceGenerator());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(InstanceGeneratorNode));
            // Teardown
        }

        [TestMethod]
        public void SutImplementsCorrectInterface()
        {
            // Fixture setup
            // Exercise system
            var sut = new TestableInstanceGenerator(new MockInstanceGenerator());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IInstanceGenerator));
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullParentWillThrow()
        {
            // Fixture setup
            // Exercise system
            new TestableInstanceGenerator(null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void ParentIsCorrect()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator();
            var sut = new TestableInstanceGenerator(parent);
            // Exercise system
            IInstanceGenerator result = sut.Parent;
            // Verify outcome
            Assert.AreEqual(parent, result, "Parent");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GenerateWillThrowIfCanGenerateReturnsFalse()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator();
            var dummyMemberInfo = typeof(object);
            var sut = new TestableInstanceGenerator(parent);
            sut.CanGenerateCallback = r => false;
            // Exercise system
            sut.Generate(dummyMemberInfo);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void GenerateWillReturnCorrectResultWhenCanGenerateReturnsTrue()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator();
            var expectedInstance = new object();
            var dummyMemberInfo = typeof(object);

            var sut = new TestableInstanceGenerator(parent);
            sut.CanGenerateCallback = r => true;
            sut.GenerateCallback = r => expectedInstance;
            // Exercise system
            var result = sut.Generate(dummyMemberInfo);
            // Verify outcome
            Assert.AreEqual(expectedInstance, result, "Generate");
            // Teardown
        }

        [TestMethod]
        public void ByDefaultCanGenerateWillReturnFalseIfParentCannotGenerate()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator { CanGenerateCallback = r => false };
            var dummyMemberInfo = typeof(object);

            var sut = new InstanceGeneratorNode(parent);
            // Exercise system
            var result = sut.CanGenerate(dummyMemberInfo);
            // Verify outcome
            Assert.IsFalse(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void ByDefaultCanGenerateWillReturnTrueIfParentCanGenerate()
        {
            // Fixture setup
            var parent = new MockInstanceGenerator { CanGenerateCallback = r => true };
            var dummyMemberInfo = typeof(object);

            var sut = new InstanceGeneratorNode(parent);
            // Exercise system
            var result = sut.CanGenerate(dummyMemberInfo);
            // Verify outcome
            Assert.IsTrue(result, "CanGenerate");
            // Teardown
        }

        [TestMethod]
        public void ByDefaultGenerateWillReturnResultFromParent()
        {
            // Fixture setup
            var expectedResult = new object();
            var parent = new MockInstanceGenerator
            {
                CanGenerateCallback = r => true,
                GenerateCallback = r => expectedResult
            };
            var dummyMemberInfo = typeof(object);

            var sut = new InstanceGeneratorNode(parent);
            // Exercise system
            var result = sut.Generate(dummyMemberInfo);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Generate");
            // Teardown
        }

        private class TestableInstanceGenerator : InstanceGeneratorNode
        {
            internal TestableInstanceGenerator(IInstanceGenerator parent)
                : base(parent)
            {
                this.CanGenerateCallback = r => false;
                this.GenerateCallback = r => new object();
            }

            protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
            {
                return new TestableGeneratorStrategy(this.Parent, request, this.CanGenerateCallback, this.GenerateCallback);
            }

            internal Func<ICustomAttributeProvider, bool> CanGenerateCallback { get; set; }

            internal Func<ICustomAttributeProvider, object> GenerateCallback { get; set; }

            private class TestableGeneratorStrategy : GeneratorStrategy
            {
                private readonly Func<ICustomAttributeProvider, bool> canGenerateCallback;
                private readonly Func<ICustomAttributeProvider, object> generateCallback;

                internal TestableGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request, Func<ICustomAttributeProvider, bool> canGenerateCallback, Func<ICustomAttributeProvider, object> generateCallback)
                    : base(parent, request)
                {
                    if (canGenerateCallback == null)
                    {
                        throw new ArgumentNullException("canGenerateCallback");
                    }
                    if (generateCallback == null)
                    {
                        throw new ArgumentNullException("generateCallback");
                    }

                    this.canGenerateCallback = canGenerateCallback;
                    this.generateCallback = generateCallback;
                }

                public override bool CanGenerate()
                {
                    return this.canGenerateCallback(this.AttributeProvider);
                }

                public override object Generate()
                {
                    return this.generateCallback(this.AttributeProvider);
                }
            }

        }
    }
}
