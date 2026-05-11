using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void CreateAnonymousWithStringPropertyWillAssignNonEmptyString()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.False(string.IsNullOrEmpty(result.Property), "Property should be assigned");
    }

    [Fact]
    public void CreateAnonymousWithStringPropertyWillAppendPropertyNameToString()
    {
        // Arrange
        string expectedName = "Property";
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
        // Assert
        string propertyValue = result.Property;
        string text = new TextGuidRegex().GetText(propertyValue);
        Assert.Equal(expectedName, text);
    }

    [Fact]
    public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<string> ph = sut.Create<PropertyHolder<string>>();
        // Act
        PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.NotEqual<string>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithBooleanPropertyWillAssignTrue()
    {
        // Arrange
        bool unexpectedBoolean = default(bool);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
        // Assert
        Assert.NotEqual<bool>(unexpectedBoolean, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithCharPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<char> ph = sut.Create<PropertyHolder<char>>();
        // Act
        PropertyHolder<char> result = sut.Create<PropertyHolder<char>>();
        // Assert
        Assert.NotEqual<char>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<bool> ph = sut.Create<PropertyHolder<bool>>();
        // Act
        PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
        // Assert
        Assert.NotEqual<bool>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithBytePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        byte unexpectedByte = default(byte);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
        // Assert
        Assert.NotEqual<byte>(unexpectedByte, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<byte> ph = sut.Create<PropertyHolder<byte>>();
        // Act
        PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
        // Assert
        Assert.NotEqual<byte>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithSignedBytePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        sbyte unexpectedSbyte = default(sbyte);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
        // Assert
        Assert.NotEqual<sbyte>(unexpectedSbyte, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<sbyte> ph = sut.Create<PropertyHolder<sbyte>>();
        // Act
        PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
        // Assert
        Assert.NotEqual<sbyte>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt16PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        ushort unexpectedNumber = default(ushort);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
        // Assert
        Assert.NotEqual<ushort>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<ushort> ph = sut.Create<PropertyHolder<ushort>>();
        // Act
        PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
        // Assert
        Assert.NotEqual<ushort>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt16PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        short unexpectedNumber = default(short);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
        // Assert
        Assert.NotEqual<short>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<short> ph = sut.Create<PropertyHolder<short>>();
        // Act
        PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
        // Assert
        Assert.NotEqual<short>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt32PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        uint unexpectedNumber = default(uint);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
        // Assert
        Assert.NotEqual<uint>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<uint> ph = sut.Create<PropertyHolder<uint>>();
        // Act
        PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
        // Assert
        Assert.NotEqual<uint>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt32PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        int unexpectedNumber = default(int);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
        // Assert
        Assert.NotEqual<int>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<int> ph = sut.Create<PropertyHolder<int>>();
        // Act
        PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
        // Assert
        Assert.NotEqual<int>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt64PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        ulong unexpectedNumber = default(ulong);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
        // Assert
        Assert.NotEqual<ulong>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<ulong> ph = sut.Create<PropertyHolder<ulong>>();
        // Act
        PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
        // Assert
        Assert.NotEqual<ulong>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt64PropertyWillAssignNonDefaultValue()
    {
        // Arrange
        long unexpectedNumber = default(long);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
        // Assert
        Assert.NotEqual<long>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<long> ph = sut.Create<PropertyHolder<long>>();
        // Act
        PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
        // Assert
        Assert.NotEqual<long>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithDecimalPropertyWillAssignNonDefaultValue()
    {
        // Arrange
        decimal unexpectedNumber = default(decimal);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
        // Assert
        Assert.NotEqual<decimal>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<decimal> ph = sut.Create<PropertyHolder<decimal>>();
        // Act
        PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
        // Assert
        Assert.NotEqual<decimal>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithSinglePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        float unexpectedNumber = default(float);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
        // Assert
        Assert.NotEqual<float>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<float> ph = sut.Create<PropertyHolder<float>>();
        // Act
        PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
        // Assert
        Assert.NotEqual<float>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithDoublePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        double unexpectedNumber = default(double);
        Fixture sut = new Fixture();
        // Act
        PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
        // Assert
        Assert.NotEqual<double>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        Fixture sut = new Fixture();
        PropertyHolder<double> ph = sut.Create<PropertyHolder<double>>();
        // Act
        PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
        // Assert
        Assert.NotEqual<double>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithDoubleMixedWholeNumericPropertiesWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<DoublePropertyHolder<int, long>>();
        // Assert
        Assert.NotEqual(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithDoubleMixedSmallWholeNumericPropertiesWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<DoublePropertyHolder<byte, short>>();
        // Assert
        Assert.NotEqual(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithDoubleMixedFloatingPointNumericPropertiesWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<DoublePropertyHolder<double, float>>();
        // Assert
        Assert.NotEqual(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithDoubleMixedNumericPropertiesWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<DoublePropertyHolder<long, float>>();
        // Assert
        Assert.NotEqual(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithReadonlyCollectionPropertiesBehaviorFillsCollections()
    {
        // Arrange
        var sut = new Fixture();
        sut.Behaviors.Add(new ReadonlyCollectionPropertiesBehavior());
        // Act
        var result = sut.Create<CollectionHolder<string>>();
        // Assert
        Assert.NotEmpty(result.Collection);
    }

    [Fact]
    public void CreateAnonymousWithReadonlyCollectionPropertiesBehaviorFillsCollectionsWithRepeatCount()
    {
        // Arrange
        var sut = new Fixture();
        sut.Behaviors.Add(new ReadonlyCollectionPropertiesBehavior());
        var expectedCount = 6;
        sut.RepeatCount = expectedCount;
        // Act
        var result = sut.Create<CollectionHolder<string>>();
        // Assert
        Assert.Equal(expectedCount, result.Collection.Count);
    }

    [Fact]
    public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedWholeNumericPropertiesWillAssignSameValue()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new NumericSequencePerTypeCustomization());
        var result = sut.Create<DoublePropertyHolder<int, long>>();
        // Assert
        Assert.Equal(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedSmallWholeNumericPropertiesWillAssignSameValue()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new NumericSequencePerTypeCustomization());
        var result = sut.Create<DoublePropertyHolder<byte, short>>();
        // Assert
        Assert.Equal(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedFloatingPointNumericPropertiesWillAssignSameValue()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new NumericSequencePerTypeCustomization());
        var result = sut.Create<DoublePropertyHolder<double, float>>();
        // Assert
        Assert.Equal(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedNumericPropertiesWillAssignSameValue()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new NumericSequencePerTypeCustomization());
        var result = sut.Create<DoublePropertyHolder<long, float>>();
        // Assert
        Assert.Equal(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithRandomNumericSequenceCustomizationReturnsRandomNumbers()
    {
        // Arrange
        var sut = new Fixture();
        sut.Customizations.Add(new RandomNumericSequenceGenerator(15, 30));
        var definedNumbers = new object[]
        {
            1,
            2U,
            (byte)3,
            (sbyte)4,
            5L,
            6UL,
            (short)7,
            (ushort)8,
            9.0F,
            10.0D,
            11M
        };
        // Act
        var randomNumbers = new object[]
        {
            sut.Create<int>(),
            sut.Create<uint>(),
            sut.Create<byte>(),
            sut.Create<sbyte>(),
            sut.Create<long>(),
            sut.Create<ulong>(),
            sut.Create<short>(),
            sut.Create<ushort>(),
            sut.Create<float>(),
            sut.Create<double>(),
            sut.Create<decimal>()
        };
        var result = randomNumbers.Intersect(definedNumbers);
        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void InjectCustomUpperLimitWillCauseSutToReturnNumbersInLimit()
    {
        // Arrange
        int lower = -9;
        int upper = -1;
        var sut = new Fixture();
        sut.Customizations.Add(new RandomNumericSequenceGenerator(lower, upper));
        // Act
        var result = sut.Create<DoublePropertyHolder<int, long>>();
        // Assert
        Assert.True(
            (result.Property1 >= lower && result.Property1 <= upper) &&
            (result.Property2 >= lower && result.Property2 <= upper));
    }

    [Fact]
    public void CreateAnonymousWithGuidPropertyWillAssignNonDefaultValue()
    {
        // Arrange
        Guid unexpectedGuid = default(Guid);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Guid>>();
        // Assert
        Assert.NotEqual<Guid>(unexpectedGuid, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        var ph = sut.Create<PropertyHolder<Guid>>();
        // Act
        var result = sut.Create<PropertyHolder<Guid>>();
        // Assert
        Assert.NotEqual<Guid>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithFlagEnumPropertyTwiceWillAssignDifferentValues()
    {
        // Arrange
        var sut = new Fixture();
        var ph = sut.Create<PropertyHolder<ActivityScope>>();
        // Act
        var result = sut.Create<PropertyHolder<ActivityScope>>();
        // Assert
        Assert.NotEqual<ActivityScope>(ph.Property, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithFlagEnumPropertyMultipleTimesWillAssignValidValues()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<PropertyHolder<ActivityScope?>>().CreateMany(100);
        // Assert
        long activityMin = 0;
        long activityMax = (long)ActivityScope.All;
        foreach (var propertyHolder in result)
        {
            long activityScope = (long)propertyHolder.Property;
            Assert.InRange(activityScope, activityMin, activityMax);
        }
    }

    [Fact]
    public void CreateAnonymousWithDoubleDateTimePropertiesWillAssignDifferentDateTimes()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
        // Assert
        Assert.NotEqual(result.Property1, result.Property2);
    }

    [Fact]
    public void CreateAnonymousWithDateTimePropertyAndIncrementingDateTimeCustomizationTwiceWithinMsReturnsDateTimesExactlyOneDayApart()
    {
        // Arrange
        var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
        var sut = new Fixture();
        sut.Customize(new IncrementingDateTimeCustomization());
        // Act
        var firstResult = sut.Create<PropertyHolder<DateTime>>();
        Thread.Sleep(nowResolution + nowResolution);
        var secondResult = sut.Create<PropertyHolder<DateTime>>();
        // Assert
        Assert.Equal(firstResult.Property.AddDays(1), secondResult.Property);
    }

    [Fact]
    public void CreateAnonymousWithDoubleDateTimePropertiesAndCurrentDateTimeCustomizationWillAssignEqualDates()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new CurrentDateTimeCustomization());
        var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
        // Assert
        Assert.Equal(result.Property1.Date, result.Property2.Date);
    }

    [Fact]
    public void CreateAnonymousWithArrayPropertyCorrectlyAssignsArray()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<int[]>>();
        // Assert
        Assert.NotEmpty(result.Property);
        Assert.True(result.Property.All(i => i != 0));
    }

    [Fact]
    public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Action);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action>>();
        // Assert
        Assert.NotEqual<Action>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Action<object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<object>>>();
        // Assert
        Assert.NotEqual<Action<object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedValue = default(Action<string>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<string>>>();
        // Assert
        Assert.NotEqual<Action<string>>(unexpectedValue, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedValue = default(Action<int>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<int>>>();
        // Assert
        Assert.NotEqual<Action<int>>(unexpectedValue, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidDoubleObjectParametersDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Action<object, object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<object, object>>>();
        // Assert
        Assert.NotEqual<Action<object, object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidDoubleSpecializedObjectParametersDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Action<string, string>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<string, string>>>();
        // Assert
        Assert.NotEqual<Action<string, string>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidDoubleValueParametersDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Action<int, bool>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action<int, bool>>>();
        // Assert
        Assert.NotEqual<Action<int, bool>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignDelegateNotThrowing()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Action>>();
        // Assert
        Assert.Null(Record.Exception(() => ((Action)result.Property).Invoke()));
    }

    [Fact]
    public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<object>>>();
        // Assert
        Assert.NotEqual<Func<object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnObjectSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<object, object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<object, object>>>();
        // Assert
        Assert.NotEqual<Func<object, object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnObjectSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<string, object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<string, object>>>();
        // Assert
        Assert.NotEqual<Func<string, object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnObjectSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<int, object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<int, object>>>();
        // Assert
        Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<int>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<int>>>();
        // Assert
        Assert.NotEqual<Func<int>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnValueSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<int, object>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<int, object>>>();
        // Assert
        Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnValueSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
    {
        // Arrange
        var unexpectedDelegate = default(Func<int, string>);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<int, string>>>();
        // Assert
        Assert.NotEqual<Func<int, string>>(unexpectedDelegate, result.Property);
    }

    [Fact]
    public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
    {
        // Arrange
        var unexpectedResult = default(string);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<string>>>();
        // Assert
        var actualResult = ((Func<string>)result.Property).Invoke();
        Assert.NotEqual<string>(unexpectedResult, actualResult);
    }

    [Fact]
    public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
    {
        // Arrange
        var unexpectedResult = default(int);
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<Func<int>>>();
        // Assert
        var actualResult = ((Func<int>)result.Property).Invoke();
        Assert.NotEqual<int>(unexpectedResult, actualResult);
    }

    [Fact]
    public void CreateAnonymousWithTypeWithFactoryMethodWillInvokeFactoryMethod()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<TypeWithFactoryMethod>();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateAnonymousWithTypeWithFactoryPropertyWillInvokeFactoryProperty()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<TypeWithFactoryProperty>();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateAnonymousEnumReturnsCorrectResult()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<TriState>();
        // Assert
        Assert.Equal(TriState.First, result);
    }

    [Fact]
    public void CreateManyAnonymousFlagEnumsReturnsCorrectResult()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.CreateMany<ActivityScope>(100).ToArray().Last();
        // Assert
        Assert.Equal(ActivityScope.Standalone, result);
    }

    [Fact]
    public void CreateMultipleHoldersOfNullableEnumsReturnsCorrectResultForLastItem()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.CreateMany<PropertyHolder<TriState?>>(4);
        // Assert
        Assert.InRange(result.Last().Property.GetValueOrDefault(), TriState.First, TriState.Third);
    }

    [Fact]
    public void CreateMultipleSingleParameterTypesWithEnumReturnsCorrectResultForLastItem()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.CreateMany<PropertyHolder<SingleParameterType<TriState>>>(4);
        // Assert
        Assert.InRange(result.Last().Property.Parameter, TriState.First, TriState.Third);
    }
}
