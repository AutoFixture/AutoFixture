using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using AutoFixture;
using AutoFixture.DataAnnotations;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.DataAnnotations;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(
            result.Property >= RangeValidatedType.Minimum && result.Property <= RangeValidatedType.Maximum,
            string.Format(
                "Expected result to fall into the interval [{0}, {1}], but was {2}",
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum,
                result.Property));
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Field >= RangeValidatedType.Minimum && result.Field <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Property2 >= RangeValidatedType.DoubleMinimum && result.Property2 <= RangeValidatedType.DoubleMaximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(
            Convert.ToDecimal(RangeValidatedType.DoubleMinimum) <= result.Property3 && result.Property3 <= Convert.ToDecimal(RangeValidatedType.DoubleMaximum),
            string.Format(
                "Expected result to fall into the interval [{0}, {1}], but was {2}",
                RangeValidatedType.DoubleMinimum,
                RangeValidatedType.DoubleMaximum,
                result.Property3));
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Property4 >= Convert.ToDecimal(RangeValidatedType.StringMinimum) && result.Property4 <= Convert.ToDecimal(RangeValidatedType.StringMaximum));
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Property5 >= RangeValidatedType.Minimum && result.Property5 <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Property6 >= RangeValidatedType.Minimum && result.Property6 <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.Property7 >= RangeValidatedType.Minimum && result.Property7 <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.InRange(
            result.UnsignedShortProperty,
            RangeValidatedType.Minimum,
            RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.InRange(
            (int)result.UnsignedIntProperty,
            RangeValidatedType.Minimum,
            RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.InRange(
            (int)result.UnsignedLongProperty,
            RangeValidatedType.Minimum,
            RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRange()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.InRange(
            (int)result.SignedByteProperty,
            RangeValidatedType.Minimum,
            RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValue()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.PropertyWithMinimumDoubleMinValue <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValue()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.PropertyWithMaximumDoubleMaxValue >= RangeValidatedType.Minimum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValue()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.PropertyWithMinimumFloatMinValue <= RangeValidatedType.Maximum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValue()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<RangeValidatedType>();
        // Assert
        Assert.True(result.PropertyWithMaximumFloatMaxValue >= RangeValidatedType.Minimum);
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Field)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property2)
            where n < RangeValidatedType.DoubleMinimum && n > RangeValidatedType.DoubleMaximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property3)
            where n < Convert.ToDecimal(RangeValidatedType.DoubleMinimum) && n > Convert.ToDecimal(RangeValidatedType.DoubleMaximum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property4)
            where n < Convert.ToDecimal(RangeValidatedType.StringMinimum) && n > Convert.ToDecimal(RangeValidatedType.StringMaximum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property5)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property6)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property7)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedShortProperty)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedIntProperty)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedLongProperty)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().SignedByteProperty)
            where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValueOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumDoubleMinValue)
            where n > Convert.ToDouble(RangeValidatedType.Maximum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValueOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumDoubleMaxValue)
            where n < Convert.ToDouble(RangeValidatedType.Minimum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValueOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumFloatMinValue)
            where n > Convert.ToSingle(RangeValidatedType.Maximum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    [UseCulture("en-US")]
    public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValueOnMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumFloatMaxValue)
            where n < Convert.ToSingle(RangeValidatedType.Minimum)
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    public void CreateAnonymousWithRegularExpressionValidatedTypeReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<RegularExpressionValidatedType>();
        // Assert
        Assert.Matches(RegularExpressionValidatedType.Pattern, result.Property);
    }

    [Fact]
    public void CreateManyAnonymousWithRegularExpressionValidatedTypeReturnsDifferentResults()
    {
        // This test exposes an issue with Xeger/Random.
        // Xeger(pattern) internally creates an instance of Random with the default seed.
        // This means that the RegularExpressionGenerator might create identical strings
        // if called multiple times within a short time.

        // Arrange
        var fixture = new Fixture();
        var result = fixture.CreateMany<RegularExpressionValidatedType>(10).Select(x => x.Property).ToArray();
        // Assert
        Assert.Equal(result.Distinct(), result);
    }

    [Fact]
    public void CreateAnonymousWithStringLengthValidatedTypeReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        var result = fixture.Create<StringLengthValidatedType>();
        // Assert
        Assert.True(result.Property.Length <= StringLengthValidatedType.MaximumLength);
    }

    [Fact]
    public void CreateAnonymousWithStringLengthValidatedReturnsCorrectResultMultipleCall()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<StringLengthValidatedType>().Property.Length)
            where n > StringLengthValidatedType.MaximumLength
            select n;
        // Assert
        Assert.False(result.Any());
    }

    [Fact]
    public void CreateAnonymousWithUriReturnsValidResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        Uri result;
        bool succeed = Uri.TryCreate(
            fixture.Create<Uri>().OriginalString,
            UriKind.Absolute,
            out result);
        // Assert
        Assert.True(succeed && result != null);
    }

    [Fact]
    public void RangedRequestIsPresentInFailurePath()
    {
        // Arrange
        var sut = new Fixture();
        var request = new RangedRequest(typeof(ConcreteType), typeof(ConcreteType), 10, 42);
        var context = new SpecimenContext(sut);

        // Act & assert
        var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create(request, context));

        var requestPath = ex.Message.Substring(ex.Message.IndexOf("Request path:", StringComparison.Ordinal));

        Assert.Contains("RangedRequest", requestPath);
    }

    [Fact]
    public void RangedNumberRequestIsPresentInFailurePath()
    {
        // Arrange
        var sut = new Fixture();
        var request = new RangedNumberRequest(typeof(ConcreteType), 10, 42);
        var context = new SpecimenContext(sut);

        // Act & assert
        var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create(request, context));

        var requestPath = ex.Message.Substring(ex.Message.IndexOf("Request path:", StringComparison.Ordinal));

        Assert.Contains("RangedNumberRequest", requestPath);
    }

    private class TypeWithRangedEnumProperties
    {
        [Range(1, 3)]
        public EnumType RangedEnumProperty { get; set; }

        [Range(typeof(EnumType), nameof(EnumType.First), nameof(EnumType.Third))]
        public EnumType LiteralRangedEnumProperty { get; set; }

        [Range(typeof(EnumType), "1", "3")]
        public EnumType NumericRangedEnumProperty { get; set; }

        [Range(2, 2)]
        public EnumType Equal2EnumProperty { get; set; }

        [Range(10, 20)]
        public EnumType OutOfRangeEnumProperty { get; set; }
    }

    /// <summary>
    /// Scenario for: https://github.com/AutoFixture/AutoFixture/issues/722.
    /// </summary>
    [Fact]
    public void ShouldCorrectlyResolveEnumPropertiesDecoratedWithRange()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithRangedEnumProperties>();
        // Assert
        Assert.InRange(result.RangedEnumProperty, EnumType.First, EnumType.Third);
        Assert.InRange(result.LiteralRangedEnumProperty, EnumType.First, EnumType.Third);
        Assert.InRange(result.NumericRangedEnumProperty, EnumType.First, EnumType.Third);
        Assert.Equal(EnumType.Second, result.Equal2EnumProperty);
    }

    [Fact]
    public void ShouldGenerateOutOfRangeValueIfRangeAttributeIsOutOfEnumRange()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<TypeWithRangedEnumProperties>();
        // Assert
        var numericValue = (int)result.OutOfRangeEnumProperty;
        Assert.InRange(numericValue, 10, 20);
    }

    public class TypeWithDataAnnotationUsage
    {
        [Range(-42, -42)]
        public int FixedValue { get; set; }
    }

    [Fact]
    public void NoDataAnnotationCustomizationShouldDisableAnnotationSupport()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize(new NoDataAnnotationsCustomization());
        var result = sut.Create<TypeWithDataAnnotationUsage>();
        // Assert
        Assert.NotEqual(-42, result.FixedValue);
    }

    public interface IIssue970_ValueHolder
    {
        string Value { get; set; }
    }

    public class Issue970_ValueHolderImpl : IIssue970_ValueHolder
    {
        public string Value { get; set; }
    }

    [Fact]
    public void Issue970_DoesNotFailWhenHelperExtensionMethodIsUsed()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        sut.Customize<Issue970_ValueHolderImpl>(c => ConfigurePropertyField(c));
        var result = sut.Create<Issue970_ValueHolderImpl>();
        // Assert
        Assert.Equal("42", result.Value);

        IPostprocessComposer<T> ConfigurePropertyField<T>(IPostprocessComposer<T> composer)
            where T : IIssue970_ValueHolder
        {
            return composer.With(x => x.Value, "42");
        }
    }

    private class TypeWithRangedTimeSpanProperty
    {
        [Range(typeof(TimeSpan), "02:00:00", "12:00:00")]
        public TimeSpan StringRangedTimeSpanProperty { get; set; }
    }

    [Fact]
    public void ShouldCorrectlyResolveTimeSpanPropertiesDecoratedWithRange()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithRangedTimeSpanProperty>();

        // Assert
        Assert.InRange(result.StringRangedTimeSpanProperty, TimeSpan.FromHours(2), TimeSpan.FromHours(12));
    }

    [Fact]
    public void TimeSpanDecoratedWithRangeCreatedByFixtureShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var timeSpan = sut.Create<TypeWithRangedTimeSpanProperty>();

        // Assert
        Validator.ValidateObject(timeSpan, new ValidationContext(timeSpan), true);
    }

    private class TypeWithStringPropertyWithMinLength
    {
        [MinLength(100)]
        public string PropertyWithMinLength { get; set; }
    }

    [Fact]
    public void ShouldCorrectlyResolveStringPropertyDecoratedWithMinLength()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMinLength>();

        // Assert
        Assert.True(item.PropertyWithMinLength.Length >= 100);
    }

    [Fact]
    public void StringDecoratedWithMinLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMinLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    private class TypeWithStringPropertyWithMaxLength
    {
        [MaxLength(5)]
        public string PropertyWithMaxLength { get; set; }
    }

    [Fact]
    public void ShouldCorrectlyResolveStringPropertyDecoratedWithMaxLength()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMaxLength>();

        // Assert
        Assert.True(item.PropertyWithMaxLength.Length <= 5);
    }

    [Fact]
    public void StringDecoratedWithMaxLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMaxLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    private class TypeWithStringPropertyWithMinAndMaxLength
    {
        [MinLength(5)]
        [MaxLength(10)]
        public string PropertyWithMinAndMaxLength { get; set; }
    }

    [Fact]
    public void ShouldCorrectlyResolveStringPropertyDecoratedWithMinAndMaxLength()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMinAndMaxLength>();

        // Assert
        Assert.True(item.PropertyWithMinAndMaxLength.Length >= 5);
        Assert.True(item.PropertyWithMinAndMaxLength.Length <= 10);
    }

    [Fact]
    public void StringDecoratedWithMinAndMaxLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithMinAndMaxLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    private class TypeWithArrayPropertyWithMinAndMaxLength
    {
        [MinLength(5)]
        [MaxLength(10)]
        public string[] PropertyWithMinAndMaxLength { get; set; }
    }

    [Fact]
    public void ShouldCorrectlyResolveArrayPropertyDecoratedWithMinAndMaxLength()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithArrayPropertyWithMinAndMaxLength>();

        // Assert
        Assert.True(item.PropertyWithMinAndMaxLength.Length >= 5);
        Assert.True(item.PropertyWithMinAndMaxLength.Length <= 10);
    }

    [Fact]
    public void ArrayDecoratedWithMinAndMaxLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithArrayPropertyWithMinAndMaxLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    private class TypeWithArrayPropertyWithMinLength
    {
        [MinLength(5)]
        public string[] PropertyWithMinLength { get; set; }
    }

    [Fact]
    public void ArrayDecoratedWithMinLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithArrayPropertyWithMinLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    private class TypeWithArrayPropertyWithMaxLength
    {
        [MaxLength(5)]
        public string[] PropertyWithMaxLength { get; set; }
    }

    [Fact]
    public void ArrayDecoratedWithMaxLengthAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithArrayPropertyWithMaxLength>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldResolveRangedSequenceRequest()
    {
        // Arrange
        var sut = new Fixture();
        var minLength = 5;
        var maxLength = 10;
        var rsr = new RangedSequenceRequest(typeof(string), minLength, maxLength);

        // Act
        var result = sut.Create(rsr, new SpecimenContext(sut));

        // Assert
        Assert.IsNotType<NoSpecimen>(result);
        var resultSeq = ((IEnumerable<object>)result).Cast<string>();
        Assert.InRange(resultSeq.Count(), minLength, maxLength);
    }

    [Fact]
    public void ShouldResolveFixedNumberOfItemsForRangedSequenceRequest()
    {
        // Arrange
        var sut = new Fixture();
        var expectedLength = 5;
        var request = new RangedSequenceRequest(typeof(string), expectedLength, expectedLength);

        // Act
        var result = sut.Create(request, new SpecimenContext(sut));

        // Assert
        Assert.IsNotType<NoSpecimen>(result);
        var resultSeq = ((IEnumerable<object>)result).Cast<string>();
        Assert.Equal(expectedLength, resultSeq.Count());
    }

    private class TypeWithStringPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public string PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void StringDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithStringPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveStringPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithStringPropertyWithEnumDataType>();

        // Assert
        Assert.Equal(EnumType.First.ToString(), result.PropertyWithEnumDataType);
    }

    private class TypeWithBytePropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public byte PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void ByteDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithBytePropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveBytePropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithBytePropertyWithEnumDataType>();

        // Assert
        Assert.Equal((byte)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithSbytePropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public sbyte PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void SbyteDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithSbytePropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveSbytePropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithSbytePropertyWithEnumDataType>();

        // Assert
        Assert.Equal((sbyte)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithShortPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public short PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void ShortDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithShortPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveShortPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithShortPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((short)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithUshortPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public ushort PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void UshortDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithUshortPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveUshortPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithUshortPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((ushort)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithIntPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public int PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void IntDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithIntPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveIntPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithIntPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((int)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithUintPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public uint PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void UintDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithUintPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveUintPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithUintPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((uint)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithLongPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public long PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void LongDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithLongPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveLongPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithLongPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((long)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithUlongPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public ulong PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void UlongDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithUlongPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveUlongPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithUlongPropertyWithEnumDataType>();

        // Assert
        Assert.Equal((ulong)EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithObjectPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public object PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void ObjectDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithObjectPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveObjectPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithObjectPropertyWithEnumDataType>();

        // Assert
        Assert.Equal(EnumType.First, result.PropertyWithEnumDataType);
    }

    private class TypeWithEnumPropertyWithEnumDataType
    {
        [EnumDataType(typeof(EnumType))]
        public EnumType PropertyWithEnumDataType { get; set; }
    }

    [Fact]
    public void EnumDecoratedWithEnumDataTypeAttributeShouldPassValidation()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var item = sut.Create<TypeWithEnumPropertyWithEnumDataType>();

        // Assert
        Validator.ValidateObject(item, new ValidationContext(item), true);
    }

    [Fact]
    public void ShouldCorrectlyResolveEnumPropertiesDecoratedWithEnumDataTypeAttribute()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<TypeWithEnumPropertyWithEnumDataType>();

        // Assert
        Assert.Equal(EnumType.First, result.PropertyWithEnumDataType);
    }
}
