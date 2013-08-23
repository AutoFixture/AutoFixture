using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    class ParameterStoreTests
    {
        [TestCase("AddOrGetExistingItem_WhenNoItemExists_ReturnsCreatedValue", "ParameterStoreTestsType1")]
        public void AddOrGetExistingItem_WhenNoItemExists_ReturnsCreatedValue(string methodName, string typeName)
        {
            // Fixture setup
            var parameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            // Exercise system
            var result = ParameterStore.GetOrAdd(typeName, parameterInfo, i => 1);
            // Verify outcome
            Assert.That(result,Is.EqualTo(1));
            // Teardown
        }

        [TestCase( "AddOrGetExistingItem_WhenExactMatchAlreadyCalled_ReturnsOldValue", "ParameterStoreTestsType2")]
        public void AddOrGetExistingItem_WhenExactMatchAlreadyCalled_ReturnsOldValue(
            string methodName, 
            string typeName)
        {
            // Fixture setup
            var parameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            // Exercise system
            ParameterStore.GetOrAdd(typeName, parameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, parameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(1));
            // Teardown
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentParamAlreadyCalled_ReturnsNewValue", 
            "ParameterStoreTestsType3", 
            0)]
        public void AddOrGetExistingItem_WhenDifferentParamAlreadyCalled_ReturnsNewValue(
            string methodName, 
            string typeName, 
            int otherParam)
        {
            // Fixture setup
            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().Last();
            // Exercise system
            ParameterStore.GetOrAdd(typeName, firstParameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(2));
            // Teardown
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentTypeNameAlreadyCalled_ReturnsNewValue", 
            "ParameterStoreTestsType4", 
            "foo")]
        public void AddOrGetExistingItem_WhenDifferentTypeNameAlreadyCalled_ReturnsNewValue(
            string methodName, 
            string typeName, 
            string otherTypeName)
        {
            // Fixture setup
            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().Last();
            // Exercise system
            ParameterStore.GetOrAdd(otherTypeName, firstParameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(2));
            // Teardown
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentMethodAlreadyCalled_ReturnsNewValue", 
            "ParameterStoreTestsType5", 
            "OtherMethod")]
        public void AddOrGetExistingItem_WhenDifferentMethodAlreadyCalled_ReturnsNewValue(
            string methodName, 
            string typeName, 
            string otherMethodName)
        {
            // Fixture setup
            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(otherMethodName).GetParameters().First();
            // Exercise system
            ParameterStore.GetOrAdd(typeName, firstParameterInfo, i => 1);
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);
            // Verify outcome
            Assert.That(result, Is.EqualTo(2));
            // Teardown
        }

        public void OtherMethod(string methodName)
        {
        }
    }
}
