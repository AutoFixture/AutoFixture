using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    /// <summary>
    /// Note that the typeName constant MUST be different for each test because ParameterStore is static.
    /// If the typeName is not different in each test, tests will interfere with each other.
    /// </summary>
    public class ParameterStoreTests
    {
        private const string MethodName = "TwoStringParameters";

        private readonly ParameterInfo parameterInfo;
        private readonly ParameterInfo otherParameterInfo;

        public ParameterStoreTests()
        {
            this.parameterInfo = typeof(MyTestClass)
                                    .GetMethod(MethodName)
                                    .GetParameters()
                                    .First();

            this.otherParameterInfo = typeof(MyTestClass)
                                        .GetMethod(MethodName)
                                        .GetParameters()
                                        .Last();
        }


        [Test]
        public void AddOrGetExistingItem_WhenNoItemExists_ReturnsCreatedValue()
        {
            // Fixture setup
            const string typeName = "ParameterStoreTestsType1";
            // Exercise system
            var result = ParameterStore.GetOrAdd(typeName, this.parameterInfo, i => 1);
            // Verify outcome
            Assert.That(result,Is.EqualTo(1));
            // Teardown
        }

        [Test]
        public void AddOrGetExistingItem_WhenExactMatchAlreadyCalled_ReturnsOldValue()
        {
            // Fixture setup
            const string typeName = "ParameterStoreTestsType2";
            // Exercise system
            ParameterStore.GetOrAdd(typeName, this.parameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, this.parameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(1));
            // Teardown
        }

        [Test]
        public void AddOrGetExistingItem_WhenDifferentParamAlreadyCalled_ReturnsNewValue()
        {
            // Fixture setup
            const string typeName = "ParameterStoreTestsType3";
            // Exercise system
            ParameterStore.GetOrAdd(typeName, this.otherParameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, this.parameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(2));
            // Teardown
        }

        [Test]
        public void AddOrGetExistingItem_WhenDifferentTypeNameAlreadyCalled_ReturnsNewValue()
        {
            // Fixture setup
            const string typeName = "ParameterStoreTestsType4";
            const string OtherTypeName = "OtherType";
            // Exercise system
            ParameterStore.GetOrAdd(OtherTypeName, this.parameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, this.parameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(2));
            // Teardown
        }
    }
}
