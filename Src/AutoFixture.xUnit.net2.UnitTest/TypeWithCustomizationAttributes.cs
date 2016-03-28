using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    internal class TypeWithCustomizationAttributes
    {
        public void CreateWithFrozenAndGreedy([Frozen, Greedy] ConcreteType sut)
        {
        }

        public void CreateWithGreedyAndFrozen([Greedy, Frozen] ConcreteType sut)
        {
        }
    }
}