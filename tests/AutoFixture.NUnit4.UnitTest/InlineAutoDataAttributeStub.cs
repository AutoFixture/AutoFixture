using System;

namespace AutoFixture.NUnit4.UnitTest;

/// <summary>
/// A stub of <see cref="InlineAutoDataAttribute"/> for the benefit of unit testing.
/// </summary>
public class InlineAutoDataAttributeStub : InlineAutoDataAttribute
{
  public InlineAutoDataAttributeStub(Func<IFixture> fixtureFactory, params object[] arguments)
        : base(fixtureFactory, arguments)
    {
    }
}