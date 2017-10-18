using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;

namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithMethodReturningOtherInterface
    {
        IInterfaceWithMethod Method();
    }
}