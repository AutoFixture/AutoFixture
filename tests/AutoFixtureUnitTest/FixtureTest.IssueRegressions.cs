using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void CreateAbstractTypeThreeTimesGivesTheSameErrorMessage()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & assert
        var ocex1 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<AbstractType>>());

        var ocex2 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<AbstractType>>());

        var ocex3 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<AbstractType>>());

        Assert.Equal(ocex1.Message, ocex2.Message);
        Assert.Equal(ocex2.Message, ocex3.Message);
    }

    [Fact]
    public void CreateRecursiveTypeExceptionMessageIsStable()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & assert
        var ocex1 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

        var ocex2 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

        var ocex3 = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

        Assert.Equal(ocex1.Message, ocex2.Message);
        Assert.Equal(ocex2.Message, ocex3.Message);
    }

    [Fact]
    public void TraceOutputForRecursiveTypeCreationFailureIsStable()
    {
        // Arrange
        var fixture = new Fixture();
        var outputWriter = new StringWriter();
        fixture.Behaviors.Add(new TracingBehavior(outputWriter));

        // Act & assert
        Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

        var traceOutput1 = outputWriter.ToString();
        outputWriter.GetStringBuilder().Clear();

        Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

        var traceOutput2 = outputWriter.ToString();
        outputWriter.GetStringBuilder().Clear();

        Assert.Equal(traceOutput1, traceOutput2);
    }

    [Fact]
    public void Issue691_DontUseCastOperatorsToCreateSpecimen()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & assert
        var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
            fixture.Create<TypeWithCastOperatorsWithoutPublicConstructor>());

        Assert.Contains("most likely because it has no public constructor", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWithValue()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            return fixture
                .Build<PropertyHolder<ConcreteType>>()
                .With(x => x.Property.Property1, "Dummy");
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWith()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            return fixture
                .Build<PropertyHolder<ConcreteType>>()
                .With(x => x.Property.Property1);
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWithout()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            return fixture
                .Build<PropertyHolder<ConcreteType>>()
                .Without(x => x.Property.Property1);
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWithValue()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            fixture.Customize<PropertyHolder<ConcreteType>>(c => c.With(x => x.Property.Property1, "dummy"));
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWith()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            fixture.Customize<PropertyHolder<ConcreteType>>(c => c.With(x => x.Property.Property1));
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWithout()
    {
        // Arrange
        var fixture = new Fixture();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            fixture.Customize<PropertyHolder<ConcreteType>>(c => c.Without(x => x.Property.Property1));
        });
        Assert.Contains("nested property or field", ex.Message);
    }

    [Fact]
    public void Issue871_FailsWithArgumentExceptionForNullRequest()
    {
        // Arrange
        var sut = new Fixture();
        var specimenContext = new DelegatingSpecimenContext();

        // Act & assert
        Assert.Throws<ArgumentNullException>(() => sut.Create(null, specimenContext));
    }

    [Fact]
    public void Issue871_FailsWithArgumentExceptionForNullRequestContext()
    {
        // Arrange
        var sut = new Fixture();
        var request = new object();

        // Act & assert
        Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
    }

    [Fact]
    public void Issue453_RangeAttributeShouldFailWithMeaningfulException()
    {
        // Arrange
        var sut = new Fixture();

        // Act & assert
        var actualEx = Assert.ThrowsAny<ObjectCreationException>(
            () => sut.Create<Issue453_AnnotationWithOverflow>());

        Assert.IsType<OverflowException>(actualEx.InnerException);
        Assert.Contains("To solve the issue", actualEx.InnerException.Message);
    }

    private class Issue453_AnnotationWithOverflow
    {
        [Range(short.MinValue, long.MaxValue, ErrorMessage = "Id is not in range")]
        public long CustomerId { get; set; }
    }

    private class RangeAnnotationWithLargeStringBoundaries
    {
        [Range(typeof(long), "1", /* long.MaxValue */ "9223372036854775807")]
        public long PropertyWithStringValueRange1 { get; set; }

        [Range(typeof(long), /* long.MinValue */ "-9223372036854775808", "-1")]
        public long PropertyWithStringValueRange2 { get; set; }
    }

    [Fact]
    public void RangeAttributeWithLargeStringRangeShouldWork()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<RangeAnnotationWithLargeStringBoundaries>();

        // Assert
        Assert.NotEqual(0, result.PropertyWithStringValueRange1);
        Assert.NotEqual(0, result.PropertyWithStringValueRange2);
    }

    [Fact]
    public void ShouldNotDuplicateRequestPathTwiceInCaseOfRecursionGuardException()
    {
        // Arrange
        var sut = new Fixture();
        var requestToLookFor = typeof(RecursiveArrayNode).GetConstructors().Single().GetParameters().Single();

        // Act & assert
        var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create<RecursiveArrayNode>());
        int numberOfRequestOccurence =
            new Regex(Regex.Escape(requestToLookFor.ToString())).Matches(actualEx.Message).Count;

        Assert.Equal(1, numberOfRequestOccurence);
    }

    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(int))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(long))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    public void Issue897_ShouldCorrectlyHandleRequestsWithSameMinimumAndMaximumValue(Type type)
    {
        // Arrange
        var expectedValue = Convert.ChangeType(42, type, CultureInfo.InvariantCulture);
        var fakeMember = new FakeMemberInfo(
            new ProvidedAttribute(
                new RangeAttribute(type, "42", "42"),
                inherited: false));

        var sut = new Fixture();

        // Act
        var result = sut.Create(fakeMember, new SpecimenContext(sut));

        // Assert
        Assert.Equal(expectedValue, result);
    }
}
