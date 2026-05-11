using System;
using AutoFixture;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void DoOnCommandWithNullSingleParameterActionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Do<object>(null));
    }

    [Fact]
    public void DoOnCommandWithSingleParameterWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new CommandMock<string>();
        mock.OnCommand = x => methodInvoked = true;
        // Act
        sut.Do((string s) => mock.Command(s));
        // Assert
        Assert.True(methodInvoked, "Command method invoked");
    }

    [Fact]
    public void DoOnCommandWithSingleParameterWillInvokeMethodWithCorrectParameter()
    {
        // Arrange
        int expectedNumber = 94;

        var sut = new Fixture();
        sut.Register<int>(() => expectedNumber);

        var mock = new CommandMock<int>();
        mock.OnCommand = x => Assert.Equal<int>(expectedNumber, x);
        // Act
        sut.Do((int i) => mock.Command(i));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithNullTwoParameterActionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Do<object, object>(null));
    }

    [Fact]
    public void DoOnCommandWithTwoParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new CommandMock<string, long>();
        mock.OnCommand = (x1, x2) => methodInvoked = true;
        // Act
        sut.Do((string x1, long x2) => mock.Command(x1, x2));
        // Assert
        Assert.True(methodInvoked, "Command method invoked");
    }

    [Fact]
    public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        double expectedNumber = 25364.37;

        var sut = new Fixture();
        sut.Register<double>(() => expectedNumber);

        var mock = new CommandMock<double, object>();
        mock.OnCommand = (x1, x2) => Assert.Equal<double>(expectedNumber, x1);
        // Act
        sut.Do((double x1, object x2) => mock.Command(x1, x2));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        short expectedNumber = 3734;

        var sut = new Fixture();
        sut.Register<short>(() => expectedNumber);

        var mock = new CommandMock<DateTime, short>();
        mock.OnCommand = (x1, x2) => Assert.Equal<short>(expectedNumber, x2);
        // Act
        sut.Do((DateTime x1, short x2) => mock.Command(x1, x2));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithNullThreeParameterActionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Do<object, object, object>(null));
    }

    [Fact]
    public void DoOnCommandWithThreeParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new CommandMock<object, object, object>();
        mock.OnCommand = (x1, x2, x3) => methodInvoked = true;
        // Act
        sut.Do((object x1, object x2, object x3) => mock.Command(x1, x2, x3));
        // Assert
        Assert.True(methodInvoked, "Command method invoked");
    }

    [Fact]
    public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        DateTime expectedDateTime = new DateTime(1004328837);

        var sut = new Fixture();
        sut.Register<DateTime>(() => expectedDateTime);

        var mock = new CommandMock<DateTime, long, short>();
        mock.OnCommand = (x1, x2, x3) => Assert.Equal<DateTime>(expectedDateTime, x1);
        // Act
        sut.Do((DateTime x1, long x2, short x3) => mock.Command(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        TimeSpan expectedTimeSpan = TimeSpan.FromHours(53);

        var sut = new Fixture();
        sut.Register<TimeSpan>(() => expectedTimeSpan);

        var mock = new CommandMock<uint, TimeSpan, TimeSpan>();
        mock.OnCommand = (x1, x2, x3) => Assert.Equal<TimeSpan>(expectedTimeSpan, x2);
        // Act
        sut.Do((uint x1, TimeSpan x2, TimeSpan x3) => mock.Command(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
    {
        // Arrange
        var expectedText = "Anonymous text";

        var sut = new Fixture();
        sut.Register<string>(() => expectedText);

        var mock = new CommandMock<double, uint, string>();
        mock.OnCommand = (x1, x2, x3) => Assert.Equal(expectedText, x3);
        // Act
        sut.Do((double x1, uint x2, string x3) => mock.Command(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithNullFourParameterActionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Do<object, object, object, object>(null));
    }

    [Fact]
    public void DoOnCommandWithFourParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new CommandMock<uint, ushort, int, bool>();
        mock.OnCommand = (x1, x2, x3, x4) => methodInvoked = true;
        // Act
        sut.Do((uint x1, ushort x2, int x3, bool x4) => mock.Command(x1, x2, x3, x4));
        // Assert
        Assert.True(methodInvoked, "Command method invoked");
    }

    [Fact]
    public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        uint expectedNumber = 294;

        var sut = new Fixture();
        sut.Register<uint>(() => expectedNumber);

        var mock = new CommandMock<uint, bool, string, Guid>();
        mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<uint>(expectedNumber, x1);
        // Act
        sut.Do((uint x1, bool x2, string x3, Guid x4) => mock.Command(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        decimal expectedNumber = 92183.28m;

        var sut = new Fixture();
        sut.Register<decimal>(() => expectedNumber);

        var mock = new CommandMock<ushort, decimal, Guid, bool>();
        mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<decimal>(expectedNumber, x2);
        // Act
        sut.Do((ushort x1, decimal x2, Guid x3, bool x4) => mock.Command(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
    {
        // Arrange
        Guid expectedGuid = Guid.NewGuid();

        var sut = new Fixture();
        sut.Register<Guid>(() => expectedGuid);

        var mock = new CommandMock<bool, string, Guid, string>();
        mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<Guid>(expectedGuid, x3);
        // Act
        sut.Do((bool x1, string x2, Guid x3, string x4) => mock.Command(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
    {
        // Arrange
        var expectedObj = new ConcreteType();

        var sut = new Fixture();
        sut.Register<ConcreteType>(() => expectedObj);

        var mock = new CommandMock<int?, DateTime, TimeSpan, ConcreteType>();
        mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<ConcreteType>(expectedObj, x4);
        // Act
        sut.Do((int? x1, DateTime x2, TimeSpan x3, ConcreteType x4) => mock.Command(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnCommandWithNullSingleParameterFunctionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Get<object, object>(null));
    }

    [Fact]
    public void GetOnQueryWithSingleParameterWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new QueryMock<ulong, bool>();
        mock.OnQuery = x =>
        {
            methodInvoked = true;
            return true;
        };
        // Act
        sut.Get((ulong s) => mock.Query(s));
        // Assert
        Assert.True(methodInvoked, "Query method invoked");
    }

    [Fact]
    public void GetOnQueryWithSingleParameterWillInvokeMethodWithCorrectParameter()
    {
        // Arrange
        double? expectedNumber = 23892;

        var sut = new Fixture();
        sut.Register<double?>(() => expectedNumber);

        var mock = new QueryMock<double?, string>();
        mock.OnQuery = x =>
        {
            Assert.Equal<double?>(expectedNumber, x);
            return "Anonymous text";
        };
        // Act
        sut.Get((double? x) => mock.Query(x));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithSingleParameterWillReturnCorrectResult()
    {
        // Arrange
        var expectedVersion = new Version(2, 45);
        var sut = new Fixture();

        var mock = new QueryMock<int?, Version>();
        mock.OnQuery = x => expectedVersion;
        // Act
        var result = sut.Get((int? x) => mock.Query(x));
        // Assert
        Assert.Equal<Version>(expectedVersion, result);
    }

    [Fact]
    public void GetOnCommandWithNullDoubleParameterFunctionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Get<object, object, object>(null));
    }

    [Fact]
    public void GetOnQueryWithTwoParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new QueryMock<string, int, long>();
        mock.OnQuery = (x1, x2) =>
        {
            methodInvoked = true;
            return 148;
        };
        // Act
        sut.Get((string x1, int x2) => mock.Query(x1, x2));
        // Assert
        Assert.True(methodInvoked, "Query method invoked");
    }

    [Fact]
    public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        byte expectedByte = 213;

        var sut = new Fixture();
        sut.Register<byte>(() => expectedByte);

        var mock = new QueryMock<byte, int, double>();
        mock.OnQuery = (x1, x2) =>
        {
            Assert.Equal<byte>(expectedByte, x1);
            return 9823829;
        };
        // Act
        sut.Get((byte x1, int x2) => mock.Query(x1, x2));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        sbyte expectedByte = -29;

        var sut = new Fixture();
        sut.Register<sbyte>(() => expectedByte);

        var mock = new QueryMock<DateTime, sbyte, bool>();
        mock.OnQuery = (x1, x2) =>
        {
            Assert.Equal<sbyte>(expectedByte, x2);
            return false;
        };
        // Act
        sut.Get((DateTime x1, sbyte x2) => mock.Query(x1, x2));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithTwoParametersWillReturnCorrectResult()
    {
        // Arrange
        byte? expectedByte = 198;
        var sut = new Fixture();

        var mock = new QueryMock<DateTime, TimeSpan, byte?>();
        mock.OnQuery = (x1, x2) => expectedByte;
        // Act
        var result = sut.Get((DateTime x1, TimeSpan x2) => mock.Query(x1, x2));
        // Assert
        Assert.Equal<byte?>(expectedByte, result);
    }

    [Fact]
    public void GetOnCommandWithNullTripleParameterFunctionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Get<object, object, object, object>(null));
    }

    [Fact]
    public void GetOnQueryWithThreeParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new QueryMock<object, object, object, object>();
        mock.OnQuery = (x1, x2, x3) =>
        {
            methodInvoked = true;
            return new object();
        };
        // Act
        sut.Get((object x1, object x2, object x3) => mock.Query(x1, x2, x3));
        // Assert
        Assert.True(methodInvoked, "Query method invoked");
    }

    [Fact]
    public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        sbyte? expectedByte = -56;

        var sut = new Fixture();
        sut.Register<sbyte?>(() => expectedByte);

        var mock = new QueryMock<sbyte?, bool, string, float>();
        mock.OnQuery = (x1, x2, x3) =>
        {
            Assert.Equal<sbyte?>(expectedByte, x1);
            return 3646.77f;
        };
        // Act
        sut.Get((sbyte? x1, bool x2, string x3) => mock.Query(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        float expectedNumber = -927.2f;

        var sut = new Fixture();
        sut.Register<float>(() => expectedNumber);

        var mock = new QueryMock<bool, float, TimeSpan, object>();
        mock.OnQuery = (x1, x2, x3) =>
        {
            Assert.Equal<float>(expectedNumber, x2);
            return new object();
        };
        // Act
        sut.Get((bool x1, float x2, TimeSpan x3) => mock.Query(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
    {
        // Arrange
        var expectedText = "Anonymous text";

        var sut = new Fixture();
        sut.Register<string>(() => expectedText);

        var mock = new QueryMock<long, short, string, decimal?>();
        mock.OnQuery = (x1, x2, x3) =>
        {
            Assert.Equal(expectedText, x3);
            return 111.11m;
        };
        // Act
        sut.Get((long x1, short x2, string x3) => mock.Query(x1, x2, x3));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithThreeParametersWillReturnCorrectResult()
    {
        // Arrange
        var expectedDateTime = new DateTimeOffset(2839327192831219387, TimeSpan.FromHours(-2));
        var sut = new Fixture();

        var mock = new QueryMock<short, long, Guid, DateTimeOffset>();
        mock.OnQuery = (x1, x2, x3) => expectedDateTime;
        // Act
        var result = sut.Get((short x1, long x2, Guid x3) => mock.Query(x1, x2, x3));
        // Assert
        Assert.Equal<DateTimeOffset>(expectedDateTime, result);
    }

    [Fact]
    public void GetOnCommandWithNullQuadrupleParameterFunctionThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Get<object, object, object, object, object>(null));
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillInvokeMethod()
    {
        // Arrange
        bool methodInvoked = false;
        var sut = new Fixture();

        var mock = new QueryMock<object, object, object, object, object>();
        mock.OnQuery = (x1, x2, x3, x4) =>
        {
            methodInvoked = true;
            return new object();
        };
        // Act
        sut.Get((object x1, object x2, object x3, object x4) => mock.Query(x1, x2, x3, x4));
        // Assert
        Assert.True(methodInvoked, "Query method invoked");
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
    {
        // Arrange
        var expectedTimeSpan = TimeSpan.FromSeconds(23);

        var sut = new Fixture();
        sut.Register<TimeSpan>(() => expectedTimeSpan);

        var mock = new QueryMock<TimeSpan, Version, Random, Guid, EventArgs>();
        mock.OnQuery = (x1, x2, x3, x4) =>
        {
            Assert.Equal<TimeSpan>(expectedTimeSpan, x1);
            return EventArgs.Empty;
        };
        // Act
        sut.Get((TimeSpan x1, Version x2, Random x3, Guid x4) => mock.Query(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
    {
        // Arrange
        var expectedDateTimeKind = DateTimeKind.Utc;

        var sut = new Fixture();
        sut.Register<DateTimeKind>(() => expectedDateTimeKind);

        var mock = new QueryMock<Random, DateTimeKind, DateTime, string, float>();
        mock.OnQuery = (x1, x2, x3, x4) =>
        {
            Assert.Equal<DateTimeKind>(expectedDateTimeKind, x2);
            return 77f;
        };
        // Act
        sut.Get((Random x1, DateTimeKind x2, DateTime x3, string x4) => mock.Query(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
    {
        // Arrange
        var expectedDayOfWeek = DayOfWeek.Friday;

        var sut = new Fixture();
        sut.Register<DayOfWeek>(() => expectedDayOfWeek);

        var mock = new QueryMock<int, float, DayOfWeek, string, ConsoleColor>();
        mock.OnQuery = (x1, x2, x3, x4) =>
        {
            Assert.Equal<DayOfWeek>(expectedDayOfWeek, x3);
            return ConsoleColor.Black;
        };
        // Act
        sut.Get((int x1, float x2, DayOfWeek x3, string x4) => mock.Query(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
    {
        // Arrange
        var expectedNumber = 42;

        var sut = new Fixture();
        sut.Register<int>(() => expectedNumber);

        var mock = new QueryMock<Version, ushort, string, int, ConsoleColor>();
        mock.OnQuery = (x1, x2, x3, x4) =>
        {
            Assert.Equal<int>(expectedNumber, x4);
            return ConsoleColor.Cyan;
        };
        // Act
        sut.Get((Version x1, ushort x2, string x3, int x4) => mock.Query(x1, x2, x3, x4));
        // Assert (done by mock)
    }

    [Fact]
    public void GetOnQueryWithFourParametersWillReturnCorrectResult()
    {
        // Arrange
        var expectedColor = ConsoleColor.DarkGray;
        var sut = new Fixture();

        var mock = new QueryMock<int, int, int, int, ConsoleColor>();
        mock.OnQuery = (x1, x2, x3, x4) => expectedColor;
        // Act
        var result = sut.Get((int x1, int x2, int x3, int x4) => mock.Query(x1, x2, x3, x4));
        // Assert
        Assert.Equal<ConsoleColor>(expectedColor, result);
    }
}
