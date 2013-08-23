using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    class ParameterStoreTests
    {
        [TestCase("AddOrGetExistingItem_WhenNoItemExists_ReturnsCreatedValue", "ParameterStoreTestsType1")]
        public void AddOrGetExistingItem_WhenNoItemExists_ReturnsCreatedValue(string methodName, string typeName)
        {

            var parameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            var result = ParameterStore.GetOrAdd(typeName, parameterInfo, i => 1);
            
            Assert.That(result,Is.EqualTo(1));
        }

        [TestCase( "AddOrGetExistingItem_WhenExactMatchAlreadyCalled_ReturnsOldValue", "ParameterStoreTestsType2")]
        public void AddOrGetExistingItem_WhenExactMatchAlreadyCalled_ReturnsOldValue(string methodName, string typeName)
        {

            var parameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            ParameterStore.GetOrAdd(typeName, parameterInfo, i => 1);

            var result = ParameterStore.GetOrAdd(typeName, parameterInfo, i => 2);

            Assert.That(result, Is.EqualTo(1));
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentParamAlreadyCalled_ReturnsNewValue", "ParameterStoreTestsType3", 0)]
        public void AddOrGetExistingItem_WhenDifferentParamAlreadyCalled_ReturnsNewValue(string methodName, string typeName, int otherParam)
        {

            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            ParameterStore.GetOrAdd(typeName, firstParameterInfo, i => 1);

            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().Last();
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);

            Assert.That(result, Is.EqualTo(2));
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentTypeNameAlreadyCalled_ReturnsNewValue", "ParameterStoreTestsType4", "foo")]
        public void AddOrGetExistingItem_WhenDifferentTypeNameAlreadyCalled_ReturnsNewValue(string methodName, string typeName, string otherTypeName)
        {

            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            ParameterStore.GetOrAdd(otherTypeName, firstParameterInfo, i => 1);

            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().Last();
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);

            Assert.That(result, Is.EqualTo(2));
        }

        [TestCase("AddOrGetExistingItem_WhenDifferentMethodAlreadyCalled_ReturnsNewValue", "ParameterStoreTestsType5", "OtherMethod")]
        public void AddOrGetExistingItem_WhenDifferentMethodAlreadyCalled_ReturnsNewValue(string methodName, string typeName, string otherMethodName)
        {
            var firstParameterInfo = typeof(ParameterStoreTests).GetMethod(methodName).GetParameters().First();
            ParameterStore.GetOrAdd(typeName, firstParameterInfo, i => 1);

            var lastParameterInfo = typeof(ParameterStoreTests).GetMethod(otherMethodName).GetParameters().First();
            var result = ParameterStore.GetOrAdd(typeName, lastParameterInfo, i => 2);

            Assert.That(result, Is.EqualTo(2));
        }

        public void OtherMethod(string methodName)
        {
        }
    }
}
