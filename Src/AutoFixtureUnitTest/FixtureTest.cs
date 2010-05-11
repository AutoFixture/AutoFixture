using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FixtureTest
    {
        public FixtureTest()
        {
        }

        [Fact]
        public void CreateAnonymousWillCreateSimpleObject()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            object result = sut.CreateAnonymous<object>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateUnregisteredAbstractTypeWillThrow()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<AbstractType>());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillCreateSingleParameterType()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            SingleParameterType<object> result = sut.CreateAnonymous<SingleParameterType<object>>();
            // Verify outcome
            Assert.NotNull(result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMapping()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<AbstractType>(() => new ConcreteType());
            // Exercise system
            SingleParameterType<AbstractType> result = sut.CreateAnonymous<SingleParameterType<AbstractType>>();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithDoubleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, AbstractType>((obj1, obj2) => new ConcreteType(obj1, obj2));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisterdMappingWithTripleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithQuadrupleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, object, AbstractType>((obj1, obj2, obj3, obj4) => new ConcreteType(obj1, obj2, obj3, obj4));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            Assert.NotNull(result.Property4);
            // Teardown
        }

        [Fact]
        public void CustomizeInstanceWillReturnFactory()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            ObjectBuilder<object> result = sut.Build<object>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillPrefixName()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.CreateAnonymous(expectedText);
            // Verify outcome
            string actualText = new TextGuidRegex().GetText(result);
            Assert.Equal<string>(expectedText, actualText);
            // Teardown
        }

        [Fact]
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
            Assert.NotEqual<Guid>(Guid.Empty, g);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyWillAssignNonEmptyString()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Property), "Property should be assigned");
            // Teardown
        }

        [Fact]
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
            Assert.Equal<string>(expectedName, text);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<string> ph = sut.CreateAnonymous<PropertyHolder<string>>();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.NotEqual<string>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyWillAssignTrue()
        {
            // Fixture setup
            bool unexpectedBoolean = default(bool);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Verify outcome
            Assert.NotEqual<bool>(unexpectedBoolean, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<bool> ph = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Exercise system
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Verify outcome
            Assert.NotEqual<bool>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            byte unexpectedByte = default(byte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Verify outcome
            Assert.NotEqual<byte>(unexpectedByte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<byte> ph = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Exercise system
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Verify outcome
            Assert.NotEqual<byte>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            sbyte unexpectedSbyte = default(sbyte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.NotEqual<sbyte>(unexpectedSbyte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setpu
            Fixture sut = new Fixture();
            PropertyHolder<sbyte> ph = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Exercise system
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.NotEqual<sbyte>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ushort unexpectedNumber = default(ushort);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.NotEqual<ushort>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ushort> ph = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Exercise system
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.NotEqual<ushort>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            short unexpectedNumber = default(short);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
            // Verify outcome
            Assert.NotEqual<short>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<short> ph = sut.CreateAnonymous<PropertyHolder<short>>();
            // Exercise system
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
            // Verify outcome
            Assert.NotEqual<short>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            uint unexpectedNumber = default(uint);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Verify outcome
            Assert.NotEqual<uint>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<uint> ph = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Exercise system
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Verify outcome
            Assert.NotEqual<uint>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<int> ph = sut.CreateAnonymous<PropertyHolder<int>>();
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.NotEqual<int>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ulong unexpectedNumber = default(ulong);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ulong> ph = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Exercise system
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyWillAssignNonDefaulValue()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<long> ph = sut.CreateAnonymous<PropertyHolder<long>>();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
            // Verify outcome
            Assert.NotEqual<long>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            decimal unexpectedNumber = default(decimal);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.NotEqual<decimal>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<decimal> ph = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Exercise system
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.NotEqual<decimal>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            float unexpectedNumber = default(float);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.NotEqual<float>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<float> ph = sut.CreateAnonymous<PropertyHolder<float>>();
            // Exercise system
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.NotEqual<float>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            double unexpectedNumber = default(double);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
            // Verify outcome
            Assert.NotEqual<double>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<double> ph = sut.CreateAnonymous<PropertyHolder<double>>();
            // Exercise system
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
            // Verify outcome
            Assert.NotEqual<double>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithGuidProperyWillAssignNonDefaultValue()
        {
            // Fixture setup
            Guid unexpectedGuid = default(Guid);
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(unexpectedGuid, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDateTimePropertyWillAssignValueMatchingCurrentTime()
        {
            // Fixture setup
            var sut = new Fixture();
            var before = DateTime.Now;
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            var after = DateTime.Now;
            Assert.True(before <= result.Property && result.Property <= after, "Generated DateTime should match current DateTime.");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDateTimePropertyTwiceWillAssignDifferentValuesIfYouWaitLongEnough()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            Assert.NotEqual<DateTime>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void DefaultRepeatCountIsThree()
        {
            // Fixture setup
            int expectedRepeatCount = 3;
            Fixture sut = new Fixture();
            // Exercise system
            int result = sut.RepeatCount;
            // Verify outcome
            Assert.Equal<int>(expectedRepeatCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++);
            // Verify outcome
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 13;
            Fixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }

        [Fact]
        public void ReplacingStringMappingWillUseNewStringCreationAlgorithm()
        {
            // Fixture setup
            string expectedText = "Anonymous string";
            Fixture sut = new Fixture();
            // Exercise system
            sut.TypeMappings[typeof(string)] = s => expectedText;
            // Verify outcome
            string result = sut.CreateAnonymous<string>();
            Assert.Equal<string>(expectedText, result);
            // Teardown
        }

        [Fact]
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
            Assert.True(expectedList.SequenceEqual(list));
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedItemCount, result);
            // Teardown
        }

        [Fact]
        public void AddManyWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection);
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
        public void AddManyWithRepeatCountWillAddItemsToCollection()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedCount = 24;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection, expectedCount);
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
        public void AddManyWithCreatorWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<object> collection = new LinkedList<object>();
            // Exercise system
            sut.AddManyTo(collection, () => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void RegisterTypeWithPropertyOverrideWillSetPropertyValueCorrectly()
        {
            // Fixture setup
            string expectedValue = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, expectedValue));
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.Equal<string>(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeWithEchoInt32GeneratorWillReturnSeed()
        {
            // Fixture setup
            int expectedValue = 4;
            Fixture sut = new Fixture();
            sut.TypeMappings[typeof(int)] = x => x;
            // Exercise system
            int result = sut.CreateAnonymous(expectedValue);
            // Verify outcome
            Assert.Equal<int>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void CreateNestedTypeWillPopulateNestedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<PropertyHolder<string>>>();
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Property.Property), "Nested property string should not be null or empty.");
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            int expectedNumber = 94;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new CommandMock<int>();
            mock.OnCommand = x => Assert.Equal<int>(expectedNumber, x);
            // Exercise system
            sut.Do((int i) => mock.Command(i));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            double expectedNumber = 25364.37;

            var sut = new Fixture();
            sut.Register<double>(() => expectedNumber);

            var mock = new CommandMock<double, object>();
            mock.OnCommand = (x1, x2) => Assert.Equal<double>(expectedNumber, x1);
            // Exercise system
            sut.Do((double x1, object x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            short expectedNumber = 3734;

            var sut = new Fixture();
            sut.Register<short>(() => expectedNumber);

            var mock = new CommandMock<DateTime, short>();
            mock.OnCommand = (x1, x2) => Assert.Equal<short>(expectedNumber, x2);
            // Exercise system
            sut.Do((DateTime x1, short x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            DateTime expectedDateTime = new DateTime(1004328837);

            var sut = new Fixture();
            sut.Register<DateTime>(() => expectedDateTime);

            var mock = new CommandMock<DateTime, long, short>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<DateTime>(expectedDateTime, x1);
            // Exercise system
            sut.Do((DateTime x1, long x2, short x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            TimeSpan expectedTimeSpan = TimeSpan.FromHours(53);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new CommandMock<uint, TimeSpan, TimeSpan>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<TimeSpan>(expectedTimeSpan, x2);
            // Exercise system
            sut.Do((uint x1, TimeSpan x2, TimeSpan x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new CommandMock<double, uint, string>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<string>(expectedText, x3);
            // Exercise system
            sut.Do((double x1, uint x2, string x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            uint expectedNumber = 294;

            var sut = new Fixture();
            sut.Register<uint>(() => expectedNumber);

            var mock = new CommandMock<uint, bool, string, Guid>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<uint>(expectedNumber, x1);
            // Exercise system
            sut.Do((uint x1, bool x2, string x3, Guid x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            decimal expectedNumber = 92183.28m;

            var sut = new Fixture();
            sut.Register<decimal>(() => expectedNumber);

            var mock = new CommandMock<ushort, decimal, Guid, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<decimal>(expectedNumber, x2);
            // Exercise system
            sut.Do((ushort x1, decimal x2, Guid x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            Guid expectedGuid = Guid.NewGuid();

            var sut = new Fixture();
            sut.Register<Guid>(() => expectedGuid);

            var mock = new CommandMock<bool, string, Guid, string>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<Guid>(expectedGuid, x3);
            // Exercise system
            sut.Do((bool x1, string x2, Guid x3, string x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedOS = new OperatingSystem(PlatformID.Win32NT, new Version(5, 0));

            var sut = new Fixture();
            sut.Register<OperatingSystem>(() => expectedOS);

            var mock = new CommandMock<int?, DateTime, TimeSpan, OperatingSystem>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<OperatingSystem>(expectedOS, x4);
            // Exercise system
            sut.Do((int? x1, DateTime x2, TimeSpan x3, OperatingSystem x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            double? expectedNumber = 23892;

            var sut = new Fixture();
            sut.Register<double?>(() => expectedNumber);

            var mock = new QueryMock<double?, string>();
            mock.OnQuery = x => { Assert.Equal<double?>(expectedNumber, x); return "Anonymous text"; };
            // Exercise system
            sut.Get((double? x) => mock.Query(x));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.Equal<Version>(expectedVersion, result);
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            byte expectedByte = 213;

            var sut = new Fixture();
            sut.Register<byte>(() => expectedByte);

            var mock = new QueryMock<byte, int, double>();
            mock.OnQuery = (x1, x2) => { Assert.Equal<byte>(expectedByte, x1); return 9823829; };
            // Exercise system
            sut.Get((byte x1, int x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            sbyte expectedByte = -29;

            var sut = new Fixture();
            sut.Register<sbyte>(() => expectedByte);

            var mock = new QueryMock<DateTime, sbyte, bool>();
            mock.OnQuery = (x1, x2) => { Assert.Equal<sbyte>(expectedByte, x2); return false; };
            // Exercise system
            sut.Get((DateTime x1, sbyte x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.Equal<byte?>(expectedByte, result);
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            sbyte? expectedByte = -56;

            var sut = new Fixture();
            sut.Register<sbyte?>(() => expectedByte);

            var mock = new QueryMock<sbyte?, bool, string, float>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<sbyte?>(expectedByte, x1); return 3646.77f; };
            // Exercise system
            sut.Get((sbyte? x1, bool x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            float expectedNumber = -927.2f;

            var sut = new Fixture();
            sut.Register<float>(() => expectedNumber);

            var mock = new QueryMock<bool, float, TimeSpan, object>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<float>(expectedNumber, x2); return new object(); };
            // Exercise system
            sut.Get((bool x1, float x2, TimeSpan x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new QueryMock<long, short, string, decimal?>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<string>(expectedText, x3); return 111.11m; };
            // Exercise system
            sut.Get((long x1, short x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.Equal<DateTimeOffset>(expectedDateTime, result);
            // Teardown
        }

        [Fact]
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
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            var expectedTimeSpan = TimeSpan.FromSeconds(23);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new QueryMock<TimeSpan, Version, Random, Guid, EventArgs>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<TimeSpan>(expectedTimeSpan, x1); return EventArgs.Empty; };
            // Exercise system
            sut.Get((TimeSpan x1, Version x2, Random x3, Guid x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            var expectedDateTimeKind = DateTimeKind.Utc;

            var sut = new Fixture();
            sut.Register<DateTimeKind>(() => expectedDateTimeKind);

            var mock = new QueryMock<Random, DateTimeKind, DateTime, string, float>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<DateTimeKind>(expectedDateTimeKind, x2); return 77f; };
            // Exercise system
            sut.Get((Random x1, DateTimeKind x2, DateTime x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedDayOfWeek = DayOfWeek.Friday;

            var sut = new Fixture();
            sut.Register<DayOfWeek>(() => expectedDayOfWeek);

            var mock = new QueryMock<int, float, DayOfWeek, string, LoaderOptimization>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<DayOfWeek>(expectedDayOfWeek, x3); return LoaderOptimization.MultiDomain; };
            // Exercise system
            sut.Get((int x1, float x2, DayOfWeek x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedNumber = 42;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new QueryMock<Version, ushort, string, int, PlatformID>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<int>(expectedNumber, x4); return PlatformID.WinCE; };
            // Exercise system
            sut.Get((Version x1, ushort x2, string x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
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
            Assert.Equal<ConsoleColor>(expectedColor, result);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<string>(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<string>(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<string>(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
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
            Assert.Equal<string>(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeCanDefineConstructor()
        {
            // Fixture setup
            var sut = new Fixture();
            string expectedText = Guid.NewGuid().ToString();
            sut.Customize<SingleParameterType<string>>(ob => ob.WithConstructor(() => new SingleParameterType<string>(expectedText)));
            // Exercise system
            var result = sut.CreateAnonymous<SingleParameterType<string>>();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<IndexedPropertyHolder<object>>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BuildWillReturnBuilderThatCreatesTheCorrectNumberOfInstances()
        {
            // Fixture setup
            int expectedRepeatCount = 242;
            var sut = new Fixture();
            sut.RepeatCount = expectedRepeatCount;
            // Exercise system
            var result = sut.Build<object>().CreateMany();
            // Verify outcome
            Assert.Equal<int>(expectedRepeatCount, result.Count());
            // Teardown
        }

        [Fact]
        public void RegisterInstanceWillCauseSutToReturnInstanceWhenRequested()
        {
            // Fixture setup
            var expectedResult = new PropertyHolder<object>();
            var sut = new Fixture();
            sut.Register(expectedResult);
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<object>>();
            // Verify outcome
            Assert.Equal<PropertyHolder<object>>(expectedResult, result);
            // Teardown
        }

        [Fact]
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
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
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
            Assert.True(resolveWasInvoked, "Resolver");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResolveCallbackWithCorrectType()
        {
            // Fixture setup
            Func<Type, object> resolver = t =>
            {
                Assert.Equal<Type>(typeof(AbstractType), t);
                return new ConcreteType();
            };
            var sut = new Fixture();
            sut.Resolver = resolver;
            // Exercise system
            sut.CreateAnonymous<PropertyHolder<AbstractType>>();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
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
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void FreezeWillCauseCreateAnonymousToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<Guid>();
            // Exercise system
            var result = sut.CreateAnonymous<Guid>();
            // Verify outcome
            Assert.Equal<Guid>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWillCauseFixtureToKeepReturningTheFrozenInstanceEvenAsPropertyOfOtherType()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<DateTime>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>().Property;
            // Verify outcome
            Assert.Equal<DateTime>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWithSeedWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze("Frozen");
            // Exercise system
            var result = sut.CreateAnonymous("Something else");
            // Verify outcome
            Assert.Equal<string>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
            // Exercise system
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, Guid>>();
            // Verify outcome
            Assert.Equal(frozen.Property1, result.Property1);
            Assert.Equal(frozen.Property2, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateManyWithDoCustomizationWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<List<string>>(ob => ob.Do(sut.AddManyTo).OmitAutoProperties());
            // Exercise system
            var result = sut.CreateMany<List<string>>();
            // Verify outcome
            Assert.True(result.All(l => l.Count == sut.RepeatCount), "Customize/Do/CreateMany");
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherProperties()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<object, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1)
                .CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void CreateAnonymousWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<RecursionTestObjectWithReferenceOutA>()
                .UsingRecursionHandler(new ThrowingRecursionHandler())
                .CreateAnonymous());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() => 
                sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()
                .UsingRecursionHandler(new ThrowingRecursionHandler())
                .CreateAnonymous());
        }

        [Fact]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()
                .UsingRecursionHandler(new NullRecursionHandler())
                .CreateAnonymous();
            // Verify outcome
            Assert.Null(result.ReferenceToB.ReferenceToA.ReferenceToB);
        }

        [Fact]
        public void BuildWithNullRecursionHandlerWillCreateNullOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()
                .UsingRecursionHandler(new NullRecursionHandler())
                .CreateAnonymous();
            // Verify outcome
            Assert.Null(result.ReferenceToB.ReferenceToA.ReferenceToB);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredInstanceWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register(item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredParameterlessFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register(() => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredSingleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredDoubleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredTripleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredQuadrupleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3, object obj4) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            Fixture sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoProperties()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            ObjectBuilder<PropertyHolder<object>> builder = sut.Build<PropertyHolder<object>>();
            PropertyHolder<object> result = builder.CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void FreezedFirstCallToCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var expectedResult = sut.Freeze<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void CustomizedBuilderCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            sut.Customize<PropertyHolder<string>>(x => x);
            var expectedResult = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void CustomizedOverrideOfOmitAutoPropertiesWillAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            sut.Customize<PropertyHolder<string>>(x => x.WithAutoProperties());
            var expectedResult = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.NotNull(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void DefaultOmitAutoPropertiesIsFalse()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            bool result = sut.OmitAutoProperties;
            // Verify outcome
            Assert.False(result, "OmitAutoProperties");
            // Teardown
        }

        [Fact]
        public void FromSeedWithNullFuncThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Build<object>().FromSeed(null));
            // Teardown
        }

        [Fact]
        public void BuildFromSeedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = new object();
            // Exercise system
            var result = sut.Build<object>().FromSeed(s => expectedResult).CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void BuildFromSeedWillCreateUsingCorrectSeed()
        {
            // Fixture setup
            var sut = new Fixture();
            var seed = new object();

            var verified = false;
            Func<object, object> mock = s => verified = seed.Equals(s);
            // Exercise system
            sut.Build<object>().FromSeed(mock).CreateAnonymous(seed);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        private class RecursionTestObjectWithReferenceOutA
        {
            public RecursionTestObjectWithReferenceOutB ReferenceToB
            {
                get;
                set;
            }
        }

        private class RecursionTestObjectWithReferenceOutB
        {
            public RecursionTestObjectWithReferenceOutA ReferenceToA
            {
                get;
                set;
            }
        }

        private class RecursionTestObjectWithConstructorReferenceOutA
        {
            public RecursionTestObjectWithConstructorReferenceOutB ReferenceToB
            {
                get;
                private set;
            }

            public RecursionTestObjectWithConstructorReferenceOutA(RecursionTestObjectWithConstructorReferenceOutB b)
            {
                this.ReferenceToB = b;
            }
        }

        private class RecursionTestObjectWithConstructorReferenceOutB
        {
            public RecursionTestObjectWithConstructorReferenceOutA ReferenceToA
            {
                get;
                private set;
            }

            public RecursionTestObjectWithConstructorReferenceOutB(RecursionTestObjectWithConstructorReferenceOutA a)
            {
                this.ReferenceToA = a;
            }
        }
    }
}

