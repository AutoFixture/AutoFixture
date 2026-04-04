using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public abstract class MarkerNodeTests<T>
    where T : ISpecimenBuilderNode
{
    [Fact]
    public void BuilderIsCorrect()
    {
        // Arrange
        var expected = new DelegatingSpecimenBuilder();
        var sut = CreateSut(expected);
        // Act
        ISpecimenBuilder actual = GetBuilder(sut);
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ComposeReturnsCorrectResult()
    {
        // Arrange
        var dummy = new DelegatingSpecimenBuilder();
        var sut = CreateSut(dummy);
        // Act
        var expected = new[]
        {
            new DelegatingSpecimenBuilder(),
            new DelegatingSpecimenBuilder(),
            new DelegatingSpecimenBuilder()
        };
        var actual = sut.Compose(expected);
        // Assert
        var mn = Assert.IsAssignableFrom<T>(actual);
        var builders =
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(
                GetBuilder(mn));
        Assert.True(expected.SequenceEqual(builders));
    }

    [Fact]
    public void ComposeSingleItemReturnsCorrectResult()
    {
        // Arrange
        var dummy = new DelegatingSpecimenBuilder();
        var sut = CreateSut(dummy);
        var expected = new DelegatingSpecimenBuilder();
        // Act
        var actual = sut.Compose(new[] { expected });
        // Assert
        var mn = Assert.IsAssignableFrom<T>(actual);
        Assert.Equal(expected, GetBuilder(mn));
    }

    [Fact]
    public void CreateReturnsCorrectResult()
    {
        // Arrange
        var request = new object();
        var context = new DelegatingSpecimenContext();
        var expected = new object();
        var stub = new DelegatingSpecimenBuilder
        {
            OnCreate = (r, c) =>
            {
                Assert.Equal(request, r);
                Assert.Equal(context, c);
                return expected;
            }
        };
        var sut = CreateSut(stub);
        // Act
        var actual = sut.Create(request, context);
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SutYieldsDecoratedBuilder()
    {
        // Arrange
        var expected = new DelegatingSpecimenBuilder();
        // Act
        var sut = CreateSut(expected);
        // Assert
        Assert.True(new[] { expected }.SequenceEqual(sut));
        Assert.True(new object[] { expected }.SequenceEqual(
            ((System.Collections.IEnumerable)sut).Cast<object>()));
    }

    [Fact]
    public void ConstructWithNullBuilderThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
            CreateSut(null));
    }

    public abstract T CreateSut(ISpecimenBuilder builder);

    public abstract ISpecimenBuilder GetBuilder(T sut);
}

public class BehaviorRootTests : MarkerNodeTests<BehaviorRoot>
{
    public override BehaviorRoot CreateSut(ISpecimenBuilder builder)
    {
        return new BehaviorRoot(builder);
    }

    public override ISpecimenBuilder GetBuilder(BehaviorRoot sut)
    {
        return sut.Builder;
    }
}

public class CustomizationNodeTests : MarkerNodeTests<CustomizationNode>
{
    public override CustomizationNode CreateSut(ISpecimenBuilder builder)
    {
        return new CustomizationNode(builder);
    }

    public override ISpecimenBuilder GetBuilder(CustomizationNode sut)
    {
        return sut.Builder;
    }
}

public class ResidueCollectorNodeTests : MarkerNodeTests<ResidueCollectorNode>
{
    public override ResidueCollectorNode CreateSut(ISpecimenBuilder builder)
    {
        return new ResidueCollectorNode(builder);
    }

    public override ISpecimenBuilder GetBuilder(ResidueCollectorNode sut)
    {
        return sut.Builder;
    }
}

public class AutoPropertiesTargetTests : MarkerNodeTests<AutoPropertiesTarget>
{
    public override AutoPropertiesTarget CreateSut(ISpecimenBuilder builder)
    {
        return new AutoPropertiesTarget(builder);
    }

    public override ISpecimenBuilder GetBuilder(AutoPropertiesTarget sut)
    {
        return sut.Builder;
    }
}