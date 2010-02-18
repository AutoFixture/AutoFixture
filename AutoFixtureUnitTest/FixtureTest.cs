using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class FixtureTest
    {
        public FixtureTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillCreateSimpleObject()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            object result = sut.CreateAnonymous<object>();
            // Verify outcome
            Assert.IsNotNull(result, "Creation of object");
            // Teardown
        }

        [ExpectedException(typeof(ObjectCreationException))]
        [TestMethod]
        public void CreateUnregisteredAbstractTypeWillThrow()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            sut.CreateAnonymous<AbstractType>();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillCreateSingleParameterType()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            SingleParameterType<object> result = sut.CreateAnonymous<SingleParameterType<object>>();
            // Verify outcome
            Assert.IsNotNull(result.Parameter, "Creation of type that takes a single parameter in its constructor");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillUseRegisteredMapping()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<AbstractType>(() => new ConcreteType());
            // Exercise system
            SingleParameterType<AbstractType> result = sut.CreateAnonymous<SingleParameterType<AbstractType>>();
            // Verify outcome
            Assert.IsInstanceOfType(result.Parameter, typeof(ConcreteType), "Creation of type that takes abstract type as parameter in its constructor");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.IsNotNull(result.Property1, "An anonymous instance was created for Property1.");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillUseRegisteredMappingWithDoubleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, AbstractType>((obj1, obj2) => new ConcreteType(obj1, obj2));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.IsNotNull(result.Property1, "An anonymous instance was created for Property1.");
            Assert.IsNotNull(result.Property2, "An anonymous instance was created for Property2.");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillUseRegisterdMappingWithTripleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.IsNotNull(result.Property1, "An anonymous instance was created for Property1.");
            Assert.IsNotNull(result.Property2, "An anonymous instance was created for Property2.");
            Assert.IsNotNull(result.Property3, "An anonymous instance was created for Property3.");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillUseRegisteredMappingWithQuadrupleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, object, AbstractType>((obj1, obj2, obj3, obj4) => new ConcreteType(obj1, obj2, obj3, obj4));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.IsNotNull(result.Property1, "An anonymous instance was created for Property1.");
            Assert.IsNotNull(result.Property2, "An anonymous instance was created for Property2.");
            Assert.IsNotNull(result.Property3, "An anonymous instance was created for Property3.");
            Assert.IsNotNull(result.Property4, "An anonymous instance was created for Property4.");
            // Teardown
        }

        [TestMethod]
        public void CustomizeInstanceWillReturnFactory()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            ObjectBuilder<object> result = sut.Build<object>();
            // Verify outcome
            Assert.IsNotNull(result, "Factory for object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousStringWillPrefixName()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.CreateAnonymous(expectedText);
            // Verify outcome
            string actualText = new TextGuidRegex().GetText(result);
            Assert.AreEqual<string>(expectedText, actualText, "Text part");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousStringWillAppendGuid()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.CreateAnonymous(anonymousText);
            // Verify outcome
            string guidString = new TextGuidRegex().GetGuid(result);
            Guid g = new Guid(guidString);
            Assert.AreNotEqual<Guid>(Guid.Empty, g, "Guid part");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithStringPropertyWillAssignNonEmptyString()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.IsFalse(string.IsNullOrEmpty(result.Property), "Property should be assigned");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithStringPropertyWillAppendPropertyNameToString()
        {
            // Fixture setup
            string expectedName = "Property";
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            string propertyValue = result.Property;
            string text = new TextGuidRegex().GetText(propertyValue);
            Assert.AreEqual<string>(expectedName, text, "Text part");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<string> ph = sut.CreateAnonymous<PropertyHolder<string>>();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.AreNotEqual<string>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithBooleanPropertyWillAssignTrue()
        {
            // Fixture setup
            bool unexpectedBoolean = default(bool);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Verify outcome
            Assert.AreNotEqual<bool>(unexpectedBoolean, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<bool> ph = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Exercise system
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Verify outcome
            Assert.AreNotEqual<bool>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            byte unexpectedByte = default(byte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Verify outcome
            Assert.AreNotEqual<byte>(unexpectedByte, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<byte> ph = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Exercise system
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Verify outcome
            Assert.AreNotEqual<byte>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithSignedBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            sbyte unexpectedSbyte = default(sbyte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.AreNotEqual<sbyte>(unexpectedSbyte, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setpu
            Fixture sut = new Fixture();
            PropertyHolder<sbyte> ph = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Exercise system
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.AreNotEqual<sbyte>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ushort unexpectedNumber = default(ushort);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.AreNotEqual<ushort>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ushort> ph = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Exercise system
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.AreNotEqual<ushort>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            short unexpectedNumber = default(short);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
            // Verify outcome
            Assert.AreNotEqual<short>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<short> ph = sut.CreateAnonymous<PropertyHolder<short>>();
            // Exercise system
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
            // Verify outcome
            Assert.AreNotEqual<short>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            uint unexpectedNumber = default(uint);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Verify outcome
            Assert.AreNotEqual<uint>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<uint> ph = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Exercise system
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Verify outcome
            Assert.AreNotEqual<uint>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.AreNotEqual<int>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<int> ph = sut.CreateAnonymous<PropertyHolder<int>>();
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.AreNotEqual<int>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt64PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ulong unexpectedNumber = default(ulong);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.AreNotEqual<ulong>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ulong> ph = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Exercise system
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.AreNotEqual<ulong>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt64PropertyWillAssignNonDefaulValue()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
            // Verify outcome
            Assert.AreNotEqual<long>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<long> ph = sut.CreateAnonymous<PropertyHolder<long>>();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
            // Verify outcome
            Assert.AreNotEqual<long>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDecimalPropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            decimal unexpectedNumber = default(decimal);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.AreNotEqual<decimal>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<decimal> ph = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Exercise system
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.AreNotEqual<decimal>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithSinglePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            float unexpectedNumber = default(float);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.AreNotEqual<float>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<float> ph = sut.CreateAnonymous<PropertyHolder<float>>();
            // Exercise system
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.AreNotEqual<float>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDoublePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            double unexpectedNumber = default(double);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
            // Verify outcome
            Assert.AreNotEqual<double>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<double> ph = sut.CreateAnonymous<PropertyHolder<double>>();
            // Exercise system
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
            // Verify outcome
            Assert.AreNotEqual<double>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithGuidProperyWillAssignNonDefaultValue()
        {
            // Fixture setup
            Guid unexpectedGuid = default(Guid);
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.AreNotEqual<Guid>(unexpectedGuid, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.AreNotEqual<Guid>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDateTimePropertyWillAssignValueMatchingCurrentTime()
        {
            // Fixture setup
            var sut = new Fixture();
            var before = DateTime.Now;
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            var after = DateTime.Now;
            Assert.IsTrue(before <= result.Property && result.Property <= after, "Generated DateTime should match current DateTime.");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWithDateTimePropertyTwiceWillAssignDifferentValuesIfYouWaitLongEnough()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            Assert.AreNotEqual<DateTime>(ph.Property, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void DefaultRepeatCountIsThree()
        {
            // Fixture setup
            int expectedRepeatCount = 3;
            Fixture sut = new Fixture();
            // Exercise system
            int result = sut.RepeatCount;
            // Verify outcome
            Assert.AreEqual<int>(expectedRepeatCount, result, "RepeatCount");
            // Teardown
        }

        [TestMethod]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result, "Repeat");
            // Teardown
        }

        [TestMethod]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result.Count(), "Repeat");
            // Teardown
        }

        [TestMethod]
        public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
        {
            // Fixture setup
            int expectedCount = 2;
            Fixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result, "Repeat");
            // Teardown
        }

        [TestMethod]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 13;
            Fixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, result.Count(), "Repeat");
            // Teardown
        }

        [TestMethod]
        public void ReplacingStringMappingWillUseNewStringCreationAlgorithm()
        {
            // Fixture setup
            string expectedText = "Anonymous string";
            Fixture sut = new Fixture();
            // Exercise system
            sut.TypeMappings[typeof(string)] = s => expectedText;
            // Verify outcome
            string result = sut.CreateAnonymous<string>();
            Assert.AreEqual<string>(expectedText, result, "The created string uses the user-defined algorithm.");
            // Teardown
        }

        [TestMethod]
        public void AddManyWillAddItemsToListUsingCreator()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            IEnumerable<int> expectedList = Enumerable.Range(1, sut.RepeatCount);
            List<int> list = new List<int>();
            // Exercise system
            int i = 0;
            sut.AddManyTo(list, () => ++i);
            // Verify outcome
            CollectionAssert.AreEqual(expectedList.ToList(), list, "AddManyTo");
            // Teardown
        }

        [TestMethod]
        public void AddManyWillAddItemsToListUsingAnonymousCreator()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            List<string> list = new List<string>();
            // Exercise system
            sut.AddManyTo(list);
            // Verify outcome
            int result = (from s in list
                          where !string.IsNullOrEmpty(s)
                          select s).Count();
            Assert.AreEqual<int>(expectedItemCount, result, "AddManyTo");
            // Teardown
        }

        [TestMethod]
        public void AddManyWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, collection.Count, "AddManyTo");
            // Teardown
        }

        [TestMethod]
        public void AddManyWithRepeatCountWillAddItemsToCollection()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedCount = 24;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection, expectedCount);
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, collection.Count, "AddManyTo");
            // Teardown
        }

        [TestMethod]
        public void AddManyWithCreatorWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<object> collection = new LinkedList<object>();
            // Exercise system
            sut.AddManyTo(collection, () => new object());
            // Verify outcome
            Assert.AreEqual<int>(expectedCount, collection.Count, "AddManyTo");
            // Teardown
        }

        [TestMethod]
        public void CreateManyWillCreateManyAnonymousItems()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany<string>();
            // Verify outcome
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.AreEqual<int>(expectedItemCount, nonDefaultCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
        public void CreateManyWillCreateCorrectNumberOfAnonymousItems()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedItemCount = 248;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany<string>(expectedItemCount);
            // Verify outcome
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.AreEqual<int>(expectedItemCount, nonDefaultCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
        public void CreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.AreEqual<int>(expectedItemCount, actualCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
        public void CreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 73;
            var sut = new Fixture();
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.AreEqual<int>(expectedItemCount, actualCount, "CreateMany");
            // Teardown
        }

        [TestMethod]
        public void RegisterTypeWithPropertyOverrideWillSetPropertyValueCorrectly()
        {
            // Fixture setup
            string expectedValue = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, expectedValue));
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.AreEqual<string>(expectedValue, result.Property, "Register property override");
            // Teardown
        }

        [TestMethod]
        public void CustomizeWithEchoInt32GeneratorWillReturnSeed()
        {
            // Fixture setup
            int expectedValue = 4;
            Fixture sut = new Fixture();
            sut.TypeMappings[typeof(int)] = x => x;
            // Exercise system
            int result = sut.CreateAnonymous(expectedValue);
            // Verify outcome
            Assert.AreEqual<int>(expectedValue, result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateNestedTypeWillPopulateNestedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<PropertyHolder<string>>>();
            // Verify outcome
            Assert.IsFalse(string.IsNullOrEmpty(result.Property.Property), "Nested property string should not be null or empty.");
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithSingleParameterWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string>();
            mock.OnCommand = x => methodInvoked = true;
            // Exercise system
            sut.Do((string s) => mock.Command(s));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Command method invoked");
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            int expectedNumber = 94;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new CommandMock<int>();
            mock.OnCommand = x => Assert.AreEqual<int>(expectedNumber, x, "Do");
            // Exercise system
            sut.Do((int i) => mock.Command(i));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithTwoParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string, long>();
            mock.OnCommand = (x1, x2) => methodInvoked = true;
            // Exercise system
            sut.Do((string x1, long x2) => mock.Command(x1, x2));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Command method invoked");
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            double expectedNumber = 25364.37;

            var sut = new Fixture();
            sut.Register<double>(() => expectedNumber);

            var mock = new CommandMock<double, object>();
            mock.OnCommand = (x1, x2) => Assert.AreEqual<double>(expectedNumber, x1, "Do");
            // Exercise system
            sut.Do((double x1, object x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            short expectedNumber = 3734;

            var sut = new Fixture();
            sut.Register<short>(() => expectedNumber);

            var mock = new CommandMock<DateTime, short>();
            mock.OnCommand = (x1, x2) => Assert.AreEqual<short>(expectedNumber, x2, "Do");
            // Exercise system
            sut.Do((DateTime x1, short x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithThreeParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<object, object, object>();
            mock.OnCommand = (x1, x2, x3) => methodInvoked = true;
            // Exercise system
            sut.Do((object x1, object x2, object x3) => mock.Command(x1, x2, x3));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Command method invoked");
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            DateTime expectedDateTime = new DateTime(1004328837);

            var sut = new Fixture();
            sut.Register<DateTime>(() => expectedDateTime);

            var mock = new CommandMock<DateTime, long, short>();
            mock.OnCommand = (x1, x2, x3) => Assert.AreEqual<DateTime>(expectedDateTime, x1, "Do");
            // Exercise system
            sut.Do((DateTime x1, long x2, short x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            TimeSpan expectedTimeSpan = TimeSpan.FromHours(53);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new CommandMock<uint, TimeSpan, TimeSpan>();
            mock.OnCommand = (x1, x2, x3) => Assert.AreEqual<TimeSpan>(expectedTimeSpan, x2, "Do");
            // Exercise system
            sut.Do((uint x1, TimeSpan x2, TimeSpan x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new CommandMock<double, uint, string>();
            mock.OnCommand = (x1, x2, x3) => Assert.AreEqual<string>(expectedText, x3, "Do");
            // Exercise system
            sut.Do((double x1, uint x2, string x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithFourParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<uint, ushort, int, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => methodInvoked = true;
            // Exercise system
            sut.Do((uint x1, ushort x2, int x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Command method invoked");
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            uint expectedNumber = 294;

            var sut = new Fixture();
            sut.Register<uint>(() => expectedNumber);

            var mock = new CommandMock<uint, bool, string, Guid>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.AreEqual<uint>(expectedNumber, x1, "Do");
            // Exercise system
            sut.Do((uint x1, bool x2, string x3, Guid x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            decimal expectedNumber = 92183.28m;

            var sut = new Fixture();
            sut.Register<decimal>(() => expectedNumber);

            var mock = new CommandMock<ushort, decimal, Guid, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.AreEqual<decimal>(expectedNumber, x2, "Do");
            // Exercise system
            sut.Do((ushort x1, decimal x2, Guid x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            Guid expectedGuid = Guid.NewGuid();

            var sut = new Fixture();
            sut.Register<Guid>(() => expectedGuid);

            var mock = new CommandMock<bool, string, Guid, string>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.AreEqual<Guid>(expectedGuid, x3, "Do");
            // Exercise system
            sut.Do((bool x1, string x2, Guid x3, string x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedOS = new OperatingSystem(PlatformID.Win32NT, new Version(5, 0));

            var sut = new Fixture();
            sut.Register<OperatingSystem>(() => expectedOS);

            var mock = new CommandMock<int?, DateTime, TimeSpan, OperatingSystem>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.AreEqual<OperatingSystem>(expectedOS, x4, "Do");
            // Exercise system
            sut.Do((int? x1, DateTime x2, TimeSpan x3, OperatingSystem x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithSingleParameterWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<ulong, bool>();
            mock.OnQuery = x => { methodInvoked = true; return true; };
            // Exercise system
            sut.Get((ulong s) => mock.Query(s));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Query method invoked");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            double? expectedNumber = 23892;

            var sut = new Fixture();
            sut.Register<double?>(() => expectedNumber);

            var mock = new QueryMock<double?, string>();
            mock.OnQuery = x => { Assert.AreEqual<double?>(expectedNumber, x, "Get"); return "Anonymous text"; };
            // Exercise system
            sut.Get((double? x) => mock.Query(x));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithSingleParameterWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedVersion = new Version(2, 45);
            var sut = new Fixture();

            var mock = new QueryMock<int?, Version>();
            mock.OnQuery = x => expectedVersion;
            // Exercise system
            var result = sut.Get((int? x) => mock.Query(x));
            // Verify outcome
            Assert.AreEqual<Version>(expectedVersion, result, "Get");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithTwoParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<string, int, long>();
            mock.OnQuery = (x1, x2) => { methodInvoked = true; return 148; };
            // Exercise system
            sut.Get((string x1, int x2) => mock.Query(x1, x2));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Query method invoked");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            byte expectedByte = 213;

            var sut = new Fixture();
            sut.Register<byte>(() => expectedByte);

            var mock = new QueryMock<byte, int, double>();
            mock.OnQuery = (x1, x2) => { Assert.AreEqual<byte>(expectedByte, x1, "Get"); return 9823829; };
            // Exercise system
            sut.Get((byte x1, int x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            sbyte expectedByte = -29;

            var sut = new Fixture();
            sut.Register<sbyte>(() => expectedByte);

            var mock = new QueryMock<DateTime, sbyte, bool>();
            mock.OnQuery = (x1, x2) => { Assert.AreEqual<sbyte>(expectedByte, x2, "Get"); return false; };
            // Exercise system
            sut.Get((DateTime x1, sbyte x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithTwoParametersWillReturnCorrectResult()
        {
            // Fixture setup
            byte? expectedByte = 198;
            var sut = new Fixture();

            var mock = new QueryMock<DateTime, TimeSpan, byte?>();
            mock.OnQuery = (x1, x2) => expectedByte;
            // Exercise system
            var result = sut.Get((DateTime x1, TimeSpan x2) => mock.Query(x1, x2));
            // Verify outcome
            Assert.AreEqual<byte?>(expectedByte, result, "Get");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithThreeParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object>();
            mock.OnQuery = (x1, x2, x3) => { methodInvoked = true; return new object(); };
            // Exercise system
            sut.Get((object x1, object x2, object x3) => mock.Query(x1, x2, x3));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Query method invoked");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            sbyte? expectedByte = -56;

            var sut = new Fixture();
            sut.Register<sbyte?>(() => expectedByte);

            var mock = new QueryMock<sbyte?, bool, string, float>();
            mock.OnQuery = (x1, x2, x3) => { Assert.AreEqual<sbyte?>(expectedByte, x1, "Get"); return 3646.77f; };
            // Exercise system
            sut.Get((sbyte? x1, bool x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            float expectedNumber = -927.2f;

            var sut = new Fixture();
            sut.Register<float>(() => expectedNumber);

            var mock = new QueryMock<bool, float, TimeSpan, object>();
            mock.OnQuery = (x1, x2, x3) => { Assert.AreEqual<float>(expectedNumber, x2, "Get"); return new object(); };
            // Exercise system
            sut.Get((bool x1, float x2, TimeSpan x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new QueryMock<long, short, string, decimal?>();
            mock.OnQuery = (x1, x2, x3) => { Assert.AreEqual<string>(expectedText, x3, "Get"); return 111.11m; };
            // Exercise system
            sut.Get((long x1, short x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithThreeParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedDateTime = new DateTimeOffset(2839327192831219387, TimeSpan.FromHours(-2));
            var sut = new Fixture();

            var mock = new QueryMock<short, long, Guid, DateTimeOffset>();
            mock.OnQuery = (x1, x2, x3) => expectedDateTime;
            // Exercise system
            var result = sut.Get((short x1, long x2, Guid x3) => mock.Query(x1, x2, x3));
            // Verify outcome
            Assert.AreEqual<DateTimeOffset>(expectedDateTime, result, "Get");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object, object>();
            mock.OnQuery = (x1, x2, x3, x4) => { methodInvoked = true; return new object(); };
            // Exercise system
            sut.Get((object x1, object x2, object x3, object x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome
            Assert.IsTrue(methodInvoked, "Query method invoked");
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            var expectedTimeSpan = TimeSpan.FromSeconds(23);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new QueryMock<TimeSpan, Version, Random, Guid, EventArgs>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.AreEqual<TimeSpan>(expectedTimeSpan, x1, "Get"); return EventArgs.Empty; };
            // Exercise system
            sut.Get((TimeSpan x1, Version x2, Random x3, Guid x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            var expectedDateTimeKind = DateTimeKind.Utc;

            var sut = new Fixture();
            sut.Register<DateTimeKind>(() => expectedDateTimeKind);

            var mock = new QueryMock<Random, DateTimeKind, DateTime, string, float>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.AreEqual<DateTimeKind>(expectedDateTimeKind, x2, "Get"); return 77f; };
            // Exercise system
            sut.Get((Random x1, DateTimeKind x2, DateTime x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedDayOfWeek = DayOfWeek.Friday;

            var sut = new Fixture();
            sut.Register<DayOfWeek>(() => expectedDayOfWeek);

            var mock = new QueryMock<int, float, DayOfWeek, string, LoaderOptimization>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.AreEqual<DayOfWeek>(expectedDayOfWeek, x3, "Get"); return LoaderOptimization.MultiDomain; };
            // Exercise system
            sut.Get((int x1, float x2, DayOfWeek x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedNumber = 42;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new QueryMock<Version, ushort, string, int, PlatformID>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.AreEqual<int>(expectedNumber, x4, "Get"); return PlatformID.WinCE; };
            // Exercise system
            sut.Get((Version x1, ushort x2, string x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [TestMethod]
        public void GetOnQueryWithFourParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedColor = ConsoleColor.DarkGray;
            var sut = new Fixture();

            var mock = new QueryMock<int, int, int, int, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) => expectedColor;
            // Exercise system
            var result = sut.Get((int x1, int x2, int x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome
            Assert.AreEqual<ConsoleColor>(expectedColor, result, "Get");
            // Teardown
        }

        [TestMethod]
        public void WithConstructorWithOneParameterWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .WithConstructor((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Parameter.Property, "WithConstructor based on customization");
            // Teardown
        }

        [TestMethod]
        public void WithConstructorWithTwoParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .WithConstructor((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Parameter.Property, "WithConstructor based on customization");
            // Teardown
        }

        [TestMethod]
        public void WithConstructorWithThreeParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .WithConstructor((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Parameter.Property, "WithConstructor based on customization");
            // Teardown
        }

        [TestMethod]
        public void WithConstructorWithFourParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .WithConstructor((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Parameter.Property, "WithConstructor based on customization");
            // Teardown
        }

        [TestMethod]
        public void CustomizeCanDefineConstructor()
        {
            // Fixture setup
            var sut = new Fixture();
            string expectedText = Guid.NewGuid().ToString();
            sut.Customize<SingleParameterType<string>>(ob => ob.WithConstructor(() => new SingleParameterType<string>(expectedText)));
            // Exercise system
            var result = sut.CreateAnonymous<SingleParameterType<string>>();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Parameter, "Customize");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<IndexedPropertyHolder<object>>();
            // Verify outcome
            Assert.IsNotNull(result, "Creating an anonymous instance of a type with an indexed property should simply ignore the indexed property.");
            // Teardown
        }

        [TestMethod]
        public void BuildWillReturnBuilderThatCreatesTheCorrectNumberOfInstances()
        {
            // Fixture setup
            int expectedRepeatCount = 242;
            var sut = new Fixture();
            sut.RepeatCount = expectedRepeatCount;
            // Exercise system
            var result = sut.Build<object>().CreateMany();
            // Verify outcome
            Assert.AreEqual<int>(expectedRepeatCount, result.Count(), "RepeatCount is passed correctly to builder");
            // Teardown
        }

        [TestMethod]
        public void RegisterInstanceWillCauseSutToReturnInstanceWhenRequested()
        {
            // Fixture setup
            var expectedResult = new PropertyHolder<object>();
            var sut = new Fixture();
            sut.Register(expectedResult);
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<object>>();
            // Verify outcome
            Assert.AreEqual<PropertyHolder<object>>(expectedResult, result, "Register instance");
            // Teardown
        }

        [TestMethod]
        public void RegisterInstanceWillCauseSutToReturnInstanceWithoutAutoPropertiesWhenRequested()
        {
            // Fixture setup
            var item = new PropertyHolder<object>();
            item.Property = null;

            var sut = new Fixture();
            sut.Register(item);
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<object>>();
            // Verify outcome
            Assert.IsNull(result.Property, "Register instance");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillInvokeResolver()
        {
            // Fixture setup
            bool resolveWasInvoked = false;
            Func<Type, object> resolver = t =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };
            var sut = new Fixture();
            sut.Resolver = resolver;
            // Exercise system
            sut.CreateAnonymous<PropertyHolder<AbstractType>>();
            // Verify outcome
            Assert.IsTrue(resolveWasInvoked, "Resolver");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResolveCallbackWithCorrectType()
        {
            // Fixture setup
            Func<Type, object> resolver = t =>
            {
                Assert.AreEqual<Type>(typeof(AbstractType), t, "Resolve");
                return new ConcreteType();
            };
            var sut = new Fixture();
            sut.Resolver = resolver;
            // Exercise system
            sut.CreateAnonymous<PropertyHolder<AbstractType>>();
            // Verify outcome (done by callback)
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResolveCallback()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();
            Func<Type, object> resolver = t => expectedValue;
            var sut = new Fixture();
            sut.Resolver = resolver;
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<AbstractType>>().Property;
            // Verify outcome
            Assert.AreEqual<AbstractType>(expectedValue, result, "Resolver");
            // Teardown
        }

        [TestMethod]
        public void FreezeWillCauseCreateAnonymousToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<Guid>();
            // Exercise system
            var result = sut.CreateAnonymous<Guid>();
            // Verify outcome
            Assert.AreEqual<Guid>(expectedResult, result, "Freeze");
            // Teardown
        }

        [TestMethod]
        public void FreezeWillCauseFixtureToKeepReturningTheFrozenInstanceEvenAsPropertyOfOtherType()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<DateTime>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>().Property;
            // Verify outcome
            Assert.AreEqual<DateTime>(expectedResult, result, "Freeze");
            // Teardown
        }

        [TestMethod]
        public void FreezeWithSeedWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze("Frozen");
            // Exercise system
            var result = sut.CreateAnonymous("Something else");
            // Verify outcome
            Assert.AreEqual<string>(expectedResult, result, "Freeze");
            // Teardown
        }

        [TestMethod]
        public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
            // Exercise system
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, Guid>>();
            // Verify outcome
            Assert.AreEqual(frozen.Property1, result.Property1, "Freeze");
            Assert.AreEqual(frozen.Property2, result.Property2, "Freeze");
            // Teardown
        }

        [TestMethod]
        public void CreateManyWithDoCustomizationWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<List<string>>(ob => ob.Do(sut.AddManyTo).OmitAutoProperties());
            // Exercise system
            var result = sut.CreateMany<List<string>>();
            // Verify outcome
            Assert.IsTrue(result.All(l => l.Count == sut.RepeatCount), "Customize/Do/CreateMany");
            // Teardown
        }
    }
}
