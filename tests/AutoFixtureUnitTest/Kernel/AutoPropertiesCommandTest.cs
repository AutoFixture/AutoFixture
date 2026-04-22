using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel;

public class AutoPropertiesCommandTest : IDisposable
{
    [Fact]
    public void InitializeNonGenericSutWithNullTypeThrows()
    {
        // Arrange
        // Act & assert
        Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand((Type)null));
    }

    [Fact]
    public void InitializeNonGenericSutWithNullSpecificationThrows()
    {
        // Arrange
        var dummyType = typeof(object);
        // Act & assert
        Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand(dummyType, null));
    }

    [Fact]
    public void InitializeNonGenericSutWithOnlyNullSpecificationThrow()
    {
        // Arrange
        // Act & assert
        Assert.Throws<ArgumentNullException>(
            () => new AutoPropertiesCommand((IRequestSpecification)null));
    }

    [Fact]
    public void ExecuteOnNonGenericWillAssignProperty()
    {
        // Arrange
        var specimen = new PropertyHolder<object>();
        var sut = new AutoPropertiesCommand(specimen.GetType());

        var expectedPropertyValue = new object();
        var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };
        // Act
        sut.Execute(specimen, container);
        // Assert
        Assert.Equal(expectedPropertyValue, specimen.Property);
    }

    [Fact]
    public void ExecuteOnUnTypedNonGenericWillAssignProperty()
    {
        // Arrange
        var sut = new AutoPropertiesCommand();

        var expectedPropertyValue = new object();
        var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

        var specimen = new PropertyHolder<object>();
        // Act
        sut.Execute(specimen, container);
        // Assert
        Assert.Equal(expectedPropertyValue, specimen.Property);
    }

    [Fact]
    public void ExecuteOnNonGenericTrueSpecifiedAssignsProperty()
    {
        // Arrange
        var trueSpecification = new DelegatingRequestSpecification
        {
            OnIsSatisfiedBy = x => true
        };
        var sut = new AutoPropertiesCommand(trueSpecification);

        var expectedPropertyValue = new object();
        var context = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

        var specimen = new PropertyHolder<object>();
        // Act
        sut.Execute(specimen, context);
        // Assert
        Assert.Equal(expectedPropertyValue, specimen.Property);
    }

    [Fact]
    public void ExecuteOnNonGenericFalseSpecifiedDoesNotAssignProperty()
    {
        // Arrange
        var falseSpecification = new DelegatingRequestSpecification
        {
            OnIsSatisfiedBy = x => false
        };
        var sut = new AutoPropertiesCommand(falseSpecification);

        var dummyPropertyValue = new object();
        var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

        var specimen = new PropertyHolder<object>();
        // Act
        sut.Execute(specimen, context);
        // Assert
        Assert.NotEqual(dummyPropertyValue, specimen.Property);
    }

    [Fact]
    public void NonTypedUsesExplicitlySpecifiedTypeForFieldsAndPropertiesResolve()
    {
        // Arrange
        var sut = new AutoPropertiesCommand(typeof(object));

        var dummyPropertyValue = new object();
        var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

        var specimen = new PropertyHolder<object>();

        // Act
        sut.Execute(specimen, context);

        // Assert
        Assert.NotEqual(dummyPropertyValue, specimen.Property);
    }

    [Fact]
    public void NonTypedWithSpecificationUsesExplicitlySpecifiedTypeForFieldsAndPropertiesResolve()
    {
        // Arrange
        var trueSpec = new TrueRequestSpecification();
        var sut = new AutoPropertiesCommand(typeof(object), trueSpec);

        var dummyPropertyValue = new object();
        var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

        var specimen = new PropertyHolder<object>();

        // Act
        sut.Execute(specimen, context);

        // Assert
        Assert.NotEqual(dummyPropertyValue, specimen.Property);
    }

    [Fact]
    public void NonTypedReturnsSpecimenTypeIfProvidedInCtor()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var sut = new AutoPropertiesCommand(type);

        // Assert
        Assert.Equal(type, sut.ExplicitSpecimenType);
    }

    [Fact]
    public void NonTypedWithSpecificationReturnsSpecimenTypeIfProvidedInCtor()
    {
        // Arrange
        var type = typeof(string);
        var spec = new TrueRequestSpecification();

        // Act
        var sut = new AutoPropertiesCommand(type, spec);

        // Assert
        Assert.Equal(type, sut.ExplicitSpecimenType);
    }

    [Fact]
    public void NonTypedReturnsNullSpecimenTypeIfNotProvided()
    {
        // Arrange
        // Act
        var sut = new AutoPropertiesCommand();

        // Assert
        Assert.Null(sut.ExplicitSpecimenType);
    }

    [Fact]
    public void NonTypedWithSpecificationReturnsNullSpecimenTypeIfNotProvided()
    {
        // Arrange
        // Act
        var sut = new AutoPropertiesCommand();

        // Assert
        Assert.Null(sut.ExplicitSpecimenType);
    }

    public void Dispose()
    {
        StaticPropertyHolder<object>.Property = null;
        StaticFieldHolder<object>.Field = null;
    }
}