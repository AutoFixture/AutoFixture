using TestTypeFoundation;

namespace AutoFixture.Xunit.UnitTest
{
    internal class TypeWithCustomizationAttributes
    {
        public void CreateWithFrozenAndFavorArrays([Frozen, FavorArrays] ConcreteType sut)
        {
        }

        public void CreateWithFavorArraysAndFrozen([FavorArrays, Frozen] ConcreteType sut)
        {
        }

        public void CreateWithFrozenAndFavorEnumerables([Frozen, FavorEnumerables] ConcreteType sut)
        {
        }

        public void CreateWithFavorEnumerablesAndFrozen([FavorEnumerables, Frozen] ConcreteType sut)
        {
        }

        public void CreateWithFrozenAndFavorLists([Frozen, FavorLists] ConcreteType sut)
        {
        }

        public void CreateWithFavorListsAndFrozen([FavorLists, Frozen] ConcreteType sut)
        {
        }

        public void CreateWithFrozenAndGreedy([Frozen, Greedy] ConcreteType sut)
        {
        }

        public void CreateWithGreedyAndFrozen([Greedy, Frozen] ConcreteType sut)
        {
        }

        public void CreateWithFrozenAndModest([Frozen, Modest] ConcreteType sut)
        {
        }

        public void CreateWithModestAndFrozen([Modest, Frozen] ConcreteType sut)
        {
        }

        public void CreateWithFrozenAndNoAutoProperties([Frozen, NoAutoProperties] ConcreteType sut)
        {
        }

        public void CreateWithNoAutoPropertiesAndFrozen([NoAutoProperties, Frozen] ConcreteType sut)
        {
        }
    }
}