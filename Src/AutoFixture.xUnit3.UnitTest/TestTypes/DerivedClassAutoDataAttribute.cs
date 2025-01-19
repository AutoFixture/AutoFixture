using System;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class DerivedClassAutoDataAttribute : ClassAutoDataAttribute
    {
        public DerivedClassAutoDataAttribute(Type sourceType)
            : base(sourceType)
        {
        }

        public DerivedClassAutoDataAttribute(Func<IFixture> fixtureFactory, Type sourceType, params object[] parameters)
            : base(fixtureFactory, sourceType, parameters)
        {
        }
    }
}
