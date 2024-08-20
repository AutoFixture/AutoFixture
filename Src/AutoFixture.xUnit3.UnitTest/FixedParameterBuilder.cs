using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.UnitTest
{
    internal class FixedParameterBuilder<T> : FilteringSpecimenBuilder
    {
        public FixedParameterBuilder(string name, T value)
            : base(new FixedBuilder(value), new ParameterSpecification(typeof(T), name))
        {
        }
    }
}