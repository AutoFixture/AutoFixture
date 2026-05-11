using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void CreateAnonymousIntPtrThrowsCorrectException()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        var creationEx = Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create<IntPtr>());
        Assert.IsAssignableFrom<IllegalRequestException>(creationEx.InnerException);
    }

    [Fact]
    public void CreateAnonymousStructWithConstructorReturnsInstance()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<MutableValueType>();
        // Assert
        Assert.NotNull(result.Property1);
        Assert.NotNull(result.Property2);
        Assert.NotNull(result.Property3);
    }

    [Fact]
    public void CreateAnonymousStructWithoutConstructorThrowsException()
    {
        // Arrange
        var fixture = new Fixture();
        // Act & assert
        Assert.ThrowsAny<ObjectCreationException>(() => fixture.Create<MutableValueTypeWithoutConstructor>());
    }

    /// <summary>
    /// This test is just to make sure that edge cases as decimal which is not primitive type and is a structure will not fall within
    /// struct checking mechanism.
    /// </summary>
    [Fact]
    public void CreateDecimalDoesNotThrowException()
    {
        // Arrange
        var fixture = new Fixture();
        // Act & assert
        Assert.Null(Record.Exception(() => fixture.Create<decimal>()));
    }

    [Fact]
    public void CreateAnonymousStructWithoutConstructorUsingCustomizationReturnsInstance()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = new SupportMutableValueTypesCustomization();
        sut.Customize(fixture);
        // Act & assert
        var result = fixture.Create<MutableValueTypeWithoutConstructor>();
        // Assert
        Assert.NotNull(result.Property1);
        Assert.NotNull(result.Property2);
    }

    [Fact]
    public void CreateNonGenericTaskReturnsAwaitableTask()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        Task result = sut.Create<Task>();
        // Assert
        Thread thread = new Thread(result.Wait);
        thread.Start();
        bool ranToCompletion = thread.Join(1000);

        Assert.True(ranToCompletion);
    }

    [Fact]
    public void CreateGenericTaskReturnsAwaitableTask()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        Task<int> result = sut.Create<Task<int>>();
        // Assert
        Thread thread = new Thread(result.Wait);
        thread.Start();
        bool ranToCompletion = thread.Join(1000);

        Assert.True(ranToCompletion);
    }

    [Fact]
    public void CreateGenericTaskReturnsTaskWhoseResultWasResolvedByFixture()
    {
        // Arrange
        Fixture sut = new Fixture();
        int frozenInt = sut.Freeze<int>();

        // Act
        Task<int> result = sut.Create<Task<int>>();

        // Assert
        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
#pragma warning disable xUnit1031 // Test asserts the task is completed and the result is accessible synchronously
        Assert.Equal(frozenInt, result.Result);
#pragma warning restore xUnit1031
    }

    [Fact]
    public void CreateLazyInitializedTypeReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<Lazy<string>>();
        var actual = result.Value;
        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public void CreatesEnumerator()
    {
        // Arrange
        var fixture = new Fixture();
        // Act & assert
        IEnumerator<int> result = null;
        Assert.Null(Record.Exception(() => result = fixture.Create<IEnumerator<int>>()));
        Assert.NotNull(result);
    }

    [Fact]
    public void CreatesEnumeratorWithSpecifiedNumberOfItems()
    {
        // Arrange
        var fixture = new Fixture();
        var expectedCount = fixture.RepeatCount;
        // Act
        var result = fixture.Create<IEnumerator<int>>();
        // Assert
        int count = 0;
        while (result.MoveNext())
        {
            count++;
            Assert.True(count <= expectedCount);
        }
        Assert.Equal(expectedCount, count);
    }

    [Fact]
    public void FixtureCanCreateCultureInfo()
    {
        var fixture = new Fixture();
        var actual = fixture.Create<System.Globalization.CultureInfo>();
        Assert.NotNull(actual);
    }

    [Fact]
    public void FixtureCanCreateEncoding()
    {
        var fixture = new Fixture();
        var actual = fixture.Create<System.Text.Encoding>();
        Assert.NotNull(actual);
    }

    [Fact]
    public void FixtureCanCreateIPAddress()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var actual = fixture.Create<System.Net.IPAddress>();
        // Assert
        Assert.NotNull(actual);
    }

#if SYSTEM_NET_MAIL
    [Fact]
    public void CreateAnonymousWithMailAddressReturnsValidResult()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        var mailAddress = fixture.Create<System.Net.Mail.MailAddress>();

        // Assert
        Assert.NotNull(mailAddress);
    }
#endif

    [Fact]
    public void ShouldReturnDifferentTimeZoneInfoEveryTime()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result1 = sut.Create<TimeZoneInfo>();
        var result2 = sut.Create<TimeZoneInfo>();
        var result3 = sut.Create<TimeZoneInfo>();

        // Assert
        Assert.NotEqual(result1, result2);
        Assert.NotEqual(result1, result3);
    }

    [Fact]
    public void CreateAnonymousEnumerableWithoutCustomizationWorks()
    {
        var sut = new Fixture();
        var actual = sut.Create<IEnumerable<decimal>>();
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void CreateAnonymousObservableCollectionWithoutCustomizationWorks()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<ObservableCollection<decimal>>();
        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateAnonymousListDoesFillWithItemsPerDefault()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<List<string>>();
        // Assert
        Assert.True(result.Any());
    }

    [Fact]
    public void CreateAnonymousTypeReturnsInstance()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<Type>();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateHashSetReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<HashSet<string>>();
        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateSortedSetReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<SortedSet<string>>();
        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateSortedDictionaryReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<SortedDictionary<string, object>>();
        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateSortedListReturnsCorrectResult()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        var result = fixture.Create<SortedList<int, string>>();
        // Assert
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData(typeof(string[][,,][]))]
    [InlineData(typeof(object[,,][][,]))]
    public void CreateComplexArrayTypeReturnsCorrectResult(Type request)
    {
        var sut = new Fixture();
        var context = new SpecimenContext(sut);
        Assert.NotNull(context.Resolve(request));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void CreateComplexArrayTypeReturnsArrayReflectingCorrectRepeatCount(int repeatCount)
    {
        var sut = new Fixture { RepeatCount = repeatCount };

        var actual = sut.Create<int[,][]>();

        Assert.Equal(repeatCount, actual.GetLength(0));
        Assert.Equal(repeatCount, actual.GetLength(1));
        Assert.Equal(repeatCount, actual[0, 0].Length);
    }

    [Fact]
    public void ShouldResolveReadOnlyCollectionByDefault()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyCollection<string>>();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void ResolvedIReadOnlyCollectionShouldBeReadOnly()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyCollection<string>>();

        // Assert
        Assert.False(result is Collection<string>);
        Assert.False(result is List<string>);
    }

    [Fact]
    public void ShouldResolveReadOnlyListByDefault()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyList<string>>();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<string>>(result);
    }

    [Fact]
    public void ResolvedIReadOnlyListShouldBeReadOnly()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyList<string>>();

        // Assert
        Assert.False(result is List<string>);
    }

    [Fact]
    public void ShouldResolveReadOnlyDictionaryByDefault()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyDictionary<int, string>>();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyDictionary<int, string>>(result);
    }

    [Fact]
    public void ResolvedIReadOnlyDictionaryShouldBeReadOnly()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<IReadOnlyDictionary<int, string>>();

        // Assert
        Assert.False(result is Dictionary<int, string>);
    }

    [Fact]
    public void ShouldResolveISetByDefault()
    {
        // Arrange
        var sut = new Fixture();

        // Act
        var result = sut.Create<ISet<string>>();

        // Assert
        Assert.IsAssignableFrom<ISet<string>>(result);
    }
}
